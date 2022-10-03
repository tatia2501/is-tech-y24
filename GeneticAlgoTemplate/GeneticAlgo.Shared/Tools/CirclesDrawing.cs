using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class CirclesDrawing
{
    private const int PointsCount = 360;
    private readonly BarrierCircle[] _circles;
    private readonly int _circleNum;

    public CirclesDrawing(BarrierCircle[] circles, int circleNum)
    {
        _circles = circles;
        _circleNum = circleNum;
    }
    public Point[] MakePointsForDrawing()
    {
        Point[] circlesPoints = new Point[PointsCount * _circleNum];
        for (int i = 1; i <= _circleNum; i++)
        {
            for (int dir = 1; dir <= PointsCount; dir++)
            {
                double x = _circles[i-1].Center.X + _circles[i-1].Radius * Math.Cos(dir);
                double y = _circles[i-1].Center.Y + _circles[i-1].Radius * Math.Sin(dir);
                circlesPoints[i * dir - 1] = new Point(x, y);
            }
        }
    
        return circlesPoints;
    }
}