namespace Domain.Entities;

public class Tellerstanden
{
    public int Id { get; set; }

    public decimal Waarde { get; set; }

    public decimal Totaal { get; set; }

    public int TreeviewId { get; set; }

    public int Datum { get; set; }

    public int TellerbasisId { get; set; }
}