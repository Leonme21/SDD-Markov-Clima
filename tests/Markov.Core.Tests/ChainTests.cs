using Markov.Core;

namespace Markov.Core.Tests;

public class ChainTests
{
    [Fact]
    public void RainMatchesIndependentReference()
    {
        var points = WeatherChain.Predict(WeatherChain.Example(), 2);
        Assert.Equal(new[] {1, 3, 5, 10}, points.Select(p => p.Day));
        decimal[][] expected = [
            [.2m, .3m, .5m], [.396m, .298m, .306m],
            [.44292m, .28624m, .27084m], [.4561989168m, .2826951952m, .261105888m]
        ];
        for (int i = 0; i < points.Count; i++)
        {
            Assert.Equal(expected[i], points[i].Probabilities);
            Assert.Equal(1m, points[i].Probabilities.Sum());
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void IdentityAndCycleHaveKnownDestinations(int start)
    {
        decimal[,] identity = {{100,0,0},{0,100,0},{0,0,100}};
        decimal[,] cycle = {{0,100,0},{0,0,100},{100,0,0}};
        foreach (var point in WeatherChain.Predict(identity, start))
            Assert.Equal(1m, point.Probabilities[start]);
        foreach (var point in WeatherChain.Predict(cycle, start))
            Assert.Equal(1m, point.Probabilities[(start + point.Day) % 3]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(65)]
    public void InvalidPercentageOrSumIsRejected(int value)
    {
        var matrix = WeatherChain.Example();
        matrix[0, 0] = value;
        Assert.Throws<ArgumentException>(() => WeatherChain.Predict(matrix, 0));
    }

    [Fact]
    public void ShapeAndInitialStateAreChecked()
    {
        Assert.Throws<ArgumentException>(() => WeatherChain.Predict(new decimal[2,3], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => WeatherChain.Predict(WeatherChain.Example(), 3));
    }
}
