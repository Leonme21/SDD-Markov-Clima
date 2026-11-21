using Markov.Application;
using Markov.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Markov.Api.Controllers;

[ApiController]
[Route("api")]
public class PredictionsController(PredictWeather predict) : ControllerBase
{
    [HttpGet("modelo/ejemplo")]
    public PredictionRequest Example() => predict.Example();

    [HttpPost("predicciones")]
    public ActionResult<DayPrediction[]> Calculate(PredictionRequest request)
    {
        try { return Ok(predict.Execute(request)); }
        catch (ArgumentException error)
        {
            return Problem(statusCode: 400, title: "Modelo inválido", detail: error.Message);
        }
    }
}
