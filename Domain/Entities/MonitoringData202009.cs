namespace Domain.Entities;

public class MonitoringData202009
{
    public int Id { get; set; }

    public sbyte Board { get; set; }

    public sbyte Port { get; set; }

    public sbyte Com { get; set; }

    public int Code { get; set; }

    public int Code2 { get; set; }

    public DateTime? Timestamp { get; set; }

    public DateOnly? Datum { get; set; }

    public string MacAddress { get; set; } = null!;

    public double ShotTime { get; set; }

    public int PreviousShotId { get; set; }
}