namespace Domain.Entities;

public class Tellerbasis
{
    public int Id { get; set; }

    public string Naam { get; set; } = null!;

    public string Omschrijving { get; set; } = null!;

    public int Optie { get; set; }

    public int Actief { get; set; }

    public string Afkorting { get; set; } = null!;

    public double MaxWaarde { get; set; }
}