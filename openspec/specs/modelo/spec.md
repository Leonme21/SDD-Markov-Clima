# modelo Specification

## Purpose

Calcular la distribución futura del clima con una cadena de Markov homogénea de tres estados.

## Requirements

### Requirement: MOD-01 Matriz estocástica
El sistema SHALL aceptar exclusivamente matrices 3×3 con porcentajes entre 0 y 100 y suma 100 en cada fila.

#### Scenario: se recibe una dimensión incorrecta, un valor fuera de rango o una fila de suma 95
- **WHEN** se recibe una dimensión incorrecta, un valor fuera de rango o una fila de suma 95
- **THEN** se rechaza la matriz.

### Requirement: MOD-02 Predicción
El sistema SHALL devolver las distribuciones de los días 1, 3, 5 y 10 desde un estado inicial válido.

#### Scenario: se parte de lluvia con las filas (70,20,10), (30,40,30), (20,30,50)
- **WHEN** se parte de lluvia con las filas (70,20,10), (30,40,30), (20,30,50)
- **THEN** el día 1 da (20,30,50) % y el día 3 da (39.6,29.8,30.6) %.

### Requirement: MOD-03 Evolución
El sistema SHALL conservar la masa de probabilidad y aplicar exactamente n transiciones.

#### Scenario: se usa la identidad o el ciclo determinista soleado → nublado → lluvioso → soleado
- **WHEN** se usa la identidad o el ciclo determinista soleado → nublado → lluvioso → soleado
- **THEN** la identidad conserva el estado y el ciclo termina en el estado desplazado n posiciones módulo tres.
