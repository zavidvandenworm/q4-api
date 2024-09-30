namespace q4_api.Dto;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<ReadingValue> Readings { get; set; } = null!;
    public List<MonitorData> MonitoringData { get; set; } = null!;
}