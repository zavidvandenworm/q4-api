namespace Domain.Entities;

public class ProductionDatum
{
    public int Id { get; set; }

    public int TreeviewId { get; set; }

    /// <summary>
    ///     Een &quot;Hot half&quot;
    /// </summary>
    public int Treeview2Id { get; set; }

    public DateOnly StartDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public DateOnly EndDate { get; set; }

    public TimeOnly EndTime { get; set; }

    /// <summary>
    ///     Hoeveel wordt er per run geproduceerd
    /// </summary>
    public double Amount { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public sbyte Port { get; set; }

    public sbyte Board { get; set; }
}