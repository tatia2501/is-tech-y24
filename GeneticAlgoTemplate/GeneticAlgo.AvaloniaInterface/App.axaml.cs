using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GeneticAlgo.AvaloniaInterface.ViewModels;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;
using Microsoft.Extensions.DependencyInjection;
using Point = GeneticAlgo.Shared.Models.Point;

namespace GeneticAlgo.AvaloniaInterface
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                CirclesData data = new CirclesData(1, 100, 4,
                    new BarrierCircle[4] {new(new Point(33.21, 35.92), 5.81), 
                    new(new Point(82.11, 76.03), 4.29), 
                    new(new Point(65.58, 56.25), 8.16), 
                    new(new Point(15.51, 19.19), 2.69)});

                var collection = new ServiceCollection();
                collection.AddSingleton<MainWindowViewModel>();
                collection.AddSingleton<IExecutionContext>(_ => new DummyExecutionContext( 10000, data.CirclesNum, data.Fmax, data.Dt, data.Circles));
                collection.AddSingleton(new ExecutionConfiguration(TimeSpan.FromMilliseconds(1000), data.Fmax, 0));

                var provider = collection.BuildServiceProvider();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}