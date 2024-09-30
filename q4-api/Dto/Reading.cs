namespace q4_api.Dto;

public class Reading
{
    public int TreeviewId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Active { get; set; }
    public double MaxValue { get; set; }

    public List<ReadingValue> Values { get; set; } = null!;
}