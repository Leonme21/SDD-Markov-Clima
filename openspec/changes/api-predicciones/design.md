## Context
Permitir a clientes externos ejecutar el mismo modelo que Blazor.

## Decisions
Separar contrato JSON, dominio puro, caso de uso y adaptadores HTTP. Alojar API y web en procesos independientes. La predicción es un cálculo sin persistencia.
