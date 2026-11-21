using Markov.Contracts;
using Markov.Core;

namespace Markov.Application;

public class PredictWeather
{
    public PredictionRequest Example()
    {
        var matrix = WeatherChain.Example();
        return new(Enumerable.Range(0, 3)
            .Select(row => Enumerable.Range(0, 3).Select(col => matrix[row,col]).ToArray()).ToArray(), 2);
    }

    public DayPrediction[] Execute(PredictionRequest request)
    {
        if (request.Matrix is not { Length: 3 } rows || rows.Any(row => row?.Length != 3))
            throw new ArgumentException("La matriz debe tener tres filas y tres columnas.");
        if (request.InitialState is not (>= 0 and <= 2))
            throw new ArgumentException("El estado inicial debe ser 0, 1 o 2.");
        var matrix = new decimal[3,3];
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                matrix[r,c] = rows[r][c];
        return WeatherChain.Predict(matrix, request.InitialState.Value)
            .Select(p => new DayPrediction(p.Day, p.Probabilities)).ToArray();
    }
}
