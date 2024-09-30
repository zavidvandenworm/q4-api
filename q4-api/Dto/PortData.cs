namespace q4_api.Dto;

public class PortData
{
    public int Board { get; set; }
    public int Port { get; set; }
    public List<MonitorData> Usage { get; set; } = null!;
}