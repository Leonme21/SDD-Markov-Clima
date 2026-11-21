# simulador Specification

## Purpose

Explorar y comparar la distribución de los tres estados mediante una matriz editable, una gráfica y una tabla accesible.

## Requirements

### Requirement: WEB-01 Editar y restaurar
El sistema SHALL permitir elegir estado, editar las nueve celdas y restaurar el ejemplo.

#### Scenario: se cambia a Soleado y luego se restaura
- **WHEN** se cambia a Soleado y luego se restaura
- **THEN** el primer día cambia a 70/20/10 y vuelve a 20/30/50 con Lluvioso.

### Requirement: WEB-02 Errores
El sistema SHALL ocultar resultados anteriores y señalar la fila inválida.

#### Scenario: una fila suma 95 o contiene una celda vacía
- **WHEN** una fila suma 95 o contiene una celda vacía
- **THEN** se ocultan tabla y gráfica y se muestra el problema; con suma 95 se informa la diferencia de 5 puntos.

### Requirement: WEB-03 Visualización
El sistema SHALL mostrar cuatro grupos de tres barras con escala común de 0 a 100 %, tabla y explicación de la fórmula.

#### Scenario: se abre el ejemplo o se cambia una matriz válida
- **WHEN** se abre el ejemplo o se cambia una matriz válida
- **THEN** la tabla y las doce barras reflejan el mismo cálculo para 1, 3, 5 y 10 días.
