using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class Algorithm
{
    private readonly Statistic[] _points;
    private readonly int _pointsNum;
    private readonly double _dt;
    private readonly Random _random;
    private readonly int _numOfBadPoints;

    public Algorithm(Statistic[] points, int pointsNum, double dt)
    {
        _points = points;
        _pointsNum = pointsNum;
        _dt = dt;
        _random = new Random();
        _numOfBadPoints = (int) (0.25 * _pointsNum);
        IsFinishReached = true;
    }
    
    public bool IsFinishReached { get; set; }
    
    private double FindLength(double l1, double l2)
    {
        return Math.Sqrt(Math.Pow(l1, 2) + Math.Pow(l2, 2));
    }

    public Statistic[] DoGeneticAlgo(BarrierCircle[] circles, int maxValue)
    {
        for (int i = 0; i < _pointsNum; i++)
        {
            var point = _points[i].Point;
            double newX = point.X + _dt * _random.NextDouble() * Math.Pow(-1, _random.Next());
            double newY = point.Y + _dt * _random.NextDouble() * Math.Pow(-1, _random.Next());
            if (newX < 0 || newX > maxValue || newY < 0 || newY > maxValue)
            {
                newX = point.X;
                newY = point.Y;
            }

            if (Math.Abs(newX - maxValue) < _dt && Math.Abs(newY - maxValue) < _dt)
            {
                IsFinishReached = false;
            }

            foreach (var circle in circles)
            {
                double lengthToCentre = FindLength(circle.Center.Y - newY, circle.Center.X - newX);
                if (circle.Radius >= lengthToCentre)
                {
                    newX = point.X;
                    newY = point.Y;
                }
            }
            
            double length = FindLength(maxValue - newY, maxValue - newX);

            _points[i] = new Statistic(i, new Point(newX, newY), length);
        }
        
        Array.Sort(_points, new StatisticComparer());
        
        for (int i = _pointsNum - 1; i > _pointsNum - _numOfBadPoints; i--)
        {
            _points[i] = _points[0];
        }

        return _points;
    }
}