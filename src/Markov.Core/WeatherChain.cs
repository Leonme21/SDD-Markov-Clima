namespace Markov.Core;

public sealed record Forecast(int Day, decimal[] Probabilities);

public static class WeatherChain
{
    public static decimal[,] Example() => new decimal[,] {{70,20,10}, {30,40,30}, {20,30,50}};

    public static IReadOnlyList<Forecast> Predict(decimal[,] percentages, int initialState)
    {
        ArgumentNullException.ThrowIfNull(percentages);
        if (percentages.GetLength(0) != 3 || percentages.GetLength(1) != 3)
            throw new ArgumentException("La matriz debe tener tres filas y tres columnas.");
        if (initialState is < 0 or > 2)
            throw new ArgumentOutOfRangeException(nameof(initialState));

        for (int row = 0; row < 3; row++)
        {
            decimal total = 0;
            for (int column = 0; column < 3; column++)
            {
                decimal value = percentages[row, column];
                if (value is < 0 or > 100)
                    throw new ArgumentException($"Fila {row + 1}: cada porcentaje debe estar entre 0 y 100.");
                total += value;
            }
            if (total != 100)
                throw new ArgumentException($"Fila {row + 1}: suma {total} %. Ajusta {Math.Abs(100-total)} puntos para llegar a 100 %.");
        }

        var forecasts = new List<Forecast>();
        var current = new decimal[3];
        current[initialState] = 1;
        for (int day = 1; day <= 10; day++)
        {
            var next = new decimal[3];
            for (int from = 0; from < 3; from++)
                for (int to = 0; to < 3; to++)
                    next[to] += current[from] * (percentages[from, to] / 100m);
            if (day is 1 or 3 or 5 or 10)
                forecasts.Add(new Forecast(day, next));
            current = next;
        }
        return forecasts;
    }
}
