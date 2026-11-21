import { after, before, test } from 'node:test';
import assert from 'node:assert/strict';
import { chromium } from 'playwright-core';

let browser;
before(async () => {
  browser = await chromium.launch({ executablePath: process.env.CHROMIUM_PATH || undefined });
});
after(async () => { await browser?.close(); });

async function withSimulator(check) {
  const page = await browser.newPage({ locale: 'es-CO' });
  try {
    await page.goto(process.env.MARKOV_URL || 'http://127.0.0.1:5057');
    // El primer render interactivo habilita los controles después de conectar el circuito.
    await page.locator('#initial:enabled').waitFor();
    await check(page);
  } finally {
    await page.close();
  }
}

test('WEB-01: cambiar el estado y restaurar el ejemplo', () => withSimulator(async page => {
  await page.getByLabel('Estado inicial').selectOption('0');
  const first = page.locator('.results tbody tr').first();
  await first.getByText('70,00 %').waitFor();
  await page.getByRole('button', {name:'Restaurar ejemplo'}).click();
  await first.getByText('50,00 %').waitFor();
  assert.equal(await page.getByLabel('Estado inicial').inputValue(), '2');
}));

test('WEB-02: señalar filas inválidas, retirar resultados y recuperar', () => withSimulator(async page => {
  const cell = page.getByLabel('Soleado a Soleado', {exact:true});
  await cell.fill('65');
  await page.getByRole('alert').getByText(/Fila 1.*95.*5 puntos/).waitFor();
  assert.equal(await page.locator('.plot, .results').count(), 0);
  await cell.fill('');
  await page.getByRole('alert').getByText(/introduce un porcentaje/).waitFor();
  await cell.fill('70');
  await page.locator('.results').waitFor();
}));

test('WEB-03: tabla y barras coinciden tras editar una matriz válida', () => withSimulator(async page => {
  await page.getByLabel('Lluvioso a Soleado').fill('25,5');
  await page.getByLabel('Lluvioso a Lluvioso').fill('44,5');
  await page.locator('.results tbody tr').first().getByText('25,50 %').waitFor();
  const rows = await page.locator('.results tbody tr').all();
  assert.equal(rows.length, 4);
  assert.equal(await page.locator('.bar').count(), 12);
  for (const [i,row] of rows.entries()) {
    assert.equal(await row.locator('th').innerText(), String([1,3,5,10][i]));
    const values = await row.locator('td').allTextContents();
    const heights = await page.locator('.group').nth(i).locator('.bar').evaluateAll(
      bars => bars.map(b => parseFloat(b.style.height))
    );
    for (const [j,value] of values.entries())
      assert.ok(Math.abs(heights[j] - parseFloat(value.replace(',', '.'))) <= 0.0051);
  }
}));
