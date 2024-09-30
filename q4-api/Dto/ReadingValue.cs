namespace q4_api.Dto;

public class ReadingValue
{
    public int TreeviewId { get; set; }
    public decimal Value { get; set; }
    public decimal Total { get; set; }
    public int Timestamp { get; set; }
}