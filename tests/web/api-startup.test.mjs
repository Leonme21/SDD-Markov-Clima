import test from 'node:test';
import assert from 'node:assert/strict';
import {spawn} from 'node:child_process';
import {chromium} from 'playwright-core';
import {setTimeout as delay} from 'node:timers/promises';

test('el circuito se recupera si la web inicia antes de la API', async () => {
  const start = (project, port, env={}) => spawn('dotnet', [
    'run','--project','src/Markov.'+project,'--no-build','-c',process.env.MARKOV_BUILD || 'Debug',
    '--no-launch-profile','--urls','http://127.0.0.1:'+port
  ], {env:{...process.env,...env},stdio:'ignore'});
  async function ready(url) {
    for(let attempt=0; attempt<80; attempt++) {
      try { if ((await fetch(url)).ok) return; } catch {}
      await delay(150);
    }
    throw Error('Servicio sin respuesta: '+url);
  }
  const web=start('Web',5067,{MarkovApi__BaseUrl:'http://127.0.0.1:5068/'});
  let api,browser;
  try {
    await ready('http://127.0.0.1:5067');
    browser=await chromium.launch({executablePath:process.env.CHROMIUM_PATH || undefined});
    const page=await browser.newPage();
    await page.goto('http://127.0.0.1:5067');
    await page.getByRole('alert').getByText(/API no está disponible/).waitFor();
    assert.ok(await page.locator('#initial').isDisabled());
    assert.equal(await page.locator('.matrix input:disabled').count(),9);
    api=start('Api',5068);
    await ready('http://127.0.0.1:5068/api/modelo/ejemplo');
    await page.getByRole('button',{name:'Restaurar ejemplo'}).click();
    await page.locator('#initial:enabled').waitFor();
    await page.locator('.results tbody tr').first().getByText('50,00 %').waitFor();
  } finally {
    await browser?.close();
    web.kill();
    api?.kill();
  }
});
