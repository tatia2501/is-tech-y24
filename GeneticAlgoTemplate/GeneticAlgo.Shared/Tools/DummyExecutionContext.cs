using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class DummyExecutionContext : IExecutionContext
{
    private readonly int _maximumValue;
    private readonly BarrierCircle[] _circles;
    private readonly Algorithm _algorithm;
    private readonly Point[] _pointForCirclesDrawing;

    public DummyExecutionContext(int pointNum, int circleNum, int maximumValue, double dt, BarrierCircle[] circles)
    {
        _maximumValue = maximumValue;
        var points = Enumerable.Range(0, pointNum)
            .Select(i => new Statistic(i, new Point(0, 0), Math.Sqrt(2 * Math.Pow(maximumValue, 2))))
            .ToArray();
        _circles = circles;
        _algorithm = new Algorithm(points, pointNum, dt);
        _pointForCirclesDrawing = new CirclesDrawing(_circles, circleNum).MakePointsForDrawing();
    }

    public void Reset() { }

    public Task<IterationResult> ExecuteIterationAsync()
    {
        return Task.FromResult(IterationResult.IterationFinished);
    }

    public bool ReportStatistics(IStatisticsConsumer statisticsConsumer)
    {
        Statistic[] statistics = _algorithm.DoGeneticAlgo(_circles, _maximumValue);

        statisticsConsumer.Consume(statistics, _pointForCirclesDrawing);
        return _algorithm.IsFinishReached;
    }
}