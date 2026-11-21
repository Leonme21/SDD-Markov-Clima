## ADDED Requirements

### Requirement: API-01 Predicción HTTP
El sistema SHALL publicar POST /api/predicciones.

#### Scenario: se envía una matriz válida y estado inicial 2
- **WHEN** se envía una matriz válida y estado inicial 2
- **THEN** la respuesta 200 contiene cuatro distribuciones en orden cronológico.

### Requirement: API-02 Validación
El sistema SHALL rechazar entradas inválidas mediante HTTP 400.

#### Scenario: el estado está fuera de rango o la matriz es irregular
- **WHEN** el estado está fuera de rango o la matriz es irregular
- **THEN** se devuelve ProblemDetails.

### Requirement: API-03 Ejemplo
El sistema SHALL publicar GET /api/modelo/ejemplo.

#### Scenario: un cliente solicita el ejemplo
- **WHEN** un cliente solicita el ejemplo
- **THEN** recibe la matriz y el estado inicial utilizables en POST.
