using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.AvaloniaInterface;

public class CirclesData
{
    public CirclesData(double dt, int fmax, int circlesNum, BarrierCircle[] circles)
    {
        Dt = dt;
        Fmax = fmax;
        CirclesNum = circlesNum;
        Circles = circles;
    }

    public double Dt { get; set; }
    public int Fmax { get; set; }
    public BarrierCircle[] Circles { get; set; }

    public int CirclesNum { get; set; }
}