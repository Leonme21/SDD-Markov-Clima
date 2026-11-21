namespace Markov.Contracts;

public sealed record PredictionRequest(decimal[][]? Matrix, int? InitialState);
public sealed record DayPrediction(int Day, decimal[] Probabilities);
