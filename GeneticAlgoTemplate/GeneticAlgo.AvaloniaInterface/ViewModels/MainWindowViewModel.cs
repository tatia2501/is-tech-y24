using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.UIShared;
using OxyPlot;
using OxyPlot.Series;

namespace GeneticAlgo.AvaloniaInterface.ViewModels;

public class MainWindowViewModel : AvaloniaObject
{
    private readonly IStatisticsConsumer _statisticsConsumer;
    private readonly IExecutionContext _executionContext;
    private readonly ExecutionConfiguration _configuration;

    public MainWindowViewModel(
        IExecutionContext executionContext,
        ExecutionConfiguration configuration)
    {
        _executionContext = executionContext;
        _configuration = configuration;

        IsRunning = AvaloniaProperty
            .RegisterAttached<MainWindowViewModel, bool>(nameof(IsRunning), typeof(MainWindowViewModel));

        var lineSeries = new ScatterSeries
        {
            MarkerSize = 3,
            MarkerStroke = OxyColors.ForestGreen,
            MarkerType = MarkerType.Plus,
        };

        var circleSeries = new ScatterSeries
        {
            MarkerSize = 3,
            MarkerStroke = OxyColors.Red,
            MarkerType = MarkerType.Plus,
        };

        ScatterModel = new PlotModel
        {
            Title = "Points",
            Series = { circleSeries, lineSeries },
        };

        var barSeries = new LinearBarSeries
        {
            DataFieldX = nameof(FitnessModel.X),
            DataFieldY = nameof(FitnessModel.Y),
        };

        BarModel = new PlotModel
        {
            Title = "Distance",
            Series = { barSeries },
        };

        _statisticsConsumer = new PlotStatisticConsumer(circleSeries, lineSeries, barSeries);
    }

    public PlotModel ScatterModel { get; }
    public PlotModel BarModel { get; }

    public AttachedProperty<bool> IsRunning { get; }

    public async Task RunAsync()
    {
        foreach (var series in ScatterModel.Series.OfType<XYAxisSeries>())
        {
            series.XAxis.Maximum = _configuration.MaximumValue;
            series.XAxis.Minimum = _configuration.MinimumValue;
            series.YAxis.Maximum = _configuration.MaximumValue;
            series.YAxis.Minimum = _configuration.MinimumValue;
        }

        SetValue(IsRunning, true);
        _executionContext.Reset();

        IterationResult iterationResult;

        do
        {
            iterationResult = await _executionContext.ExecuteIterationAsync();
            bool flag = _executionContext.ReportStatistics(_statisticsConsumer);

            SetValue(IsRunning, flag);
            
            ScatterModel.InvalidatePlot(true);
            BarModel.InvalidatePlot(true);

            await Task.Delay(_configuration.IterationDelay);
        }
        while (iterationResult == IterationResult.IterationFinished && GetValue(IsRunning));
    }

    public void Stop()
    {
        SetValue(IsRunning, false);
    }
}