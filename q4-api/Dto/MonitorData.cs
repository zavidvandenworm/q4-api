namespace q4_api.Dto;

public class MonitorData
{
    public List<int> Codes { get; set; }
    public DateTime Timestamp { get; set; }
    public double Duration { get; set; }
    public int Machine { get; set; }
    public int Mold { get; set; }
}