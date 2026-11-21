using Markov.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Markov.Web;

public class PredictionClient(HttpClient http)
{
    public async Task<PredictionRequest> ExampleAsync() =>
        await http.GetFromJsonAsync<PredictionRequest>("api/modelo/ejemplo")
        ?? throw new HttpRequestException("Respuesta vacía.");

    public async Task<DayPrediction[]> PredictAsync(PredictionRequest request)
    {
        using var response = await http.PostAsJsonAsync("api/predicciones", request);
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            throw new ArgumentException(problem?.Detail ?? "Revisa la matriz.");
        }
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DayPrediction[]>()
            ?? throw new HttpRequestException("Respuesta vacía.");
    }
}
