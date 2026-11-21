import test from 'node:test';
import assert from 'node:assert/strict';

const base = process.env.MARKOV_API_URL || 'http://127.0.0.1:5058';
function predict(body) {
  return fetch(base + '/api/predicciones', {method:'POST', headers:{'Content-Type':'application/json'}, body:JSON.stringify(body)});
}

test('el ejemplo publicado produce las cuatro distribuciones', async () => {
  const example = await fetch(base + '/api/modelo/ejemplo');
  assert.equal(example.status, 200);
  const response = await predict(await example.json());
  assert.equal(response.status, 200);
  const days = await response.json();
  assert.deepEqual(days.map(d => d.day), [1,3,5,10]);
  assert.deepEqual(days[0].probabilities, [0.2,0.3,0.5]);
});

for (const [name,input] of Object.entries({
  irregular:{matrix:[[100],[100],[100]],initialState:0},
  estado:{matrix:[[100,0,0],[0,100,0],[0,0,100]],initialState:3},
  suma:{matrix:[[95,0,0],[0,100,0],[0,0,100]],initialState:0}
})) {
  test('rechaza ' + name + ' con ProblemDetails', async () => {
    const response = await predict(input);
    assert.equal(response.status, 400);
    assert.match(response.headers.get('content-type'), /problem\+json/);
    assert.equal((await response.json()).status, 400);
  });
}
