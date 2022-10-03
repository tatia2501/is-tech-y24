namespace GeneticAlgo.Shared.Models;

public class StatisticComparer : IComparer<Statistic>
{
    public int Compare(Statistic p1, Statistic p2)
    {
        return (int) (p1.Fitness - p2.Fitness);
    }
}