## Why
Permitir a clientes externos ejecutar el mismo modelo que Blazor.

## What Changes
Separar contrato JSON, dominio puro, caso de uso y adaptadores HTTP. Alojar API y web en procesos independientes. La predicción es un cálculo sin persistencia.

## Capabilities
### New Capabilities
- `api`: Permitir a clientes externos ejecutar el mismo modelo que Blazor.

## Impact
Separar aplicación y adaptadores. Verificar contrato HTTP y simulador conectado
