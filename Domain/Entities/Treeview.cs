namespace Domain.Entities;

public class Treeview
{
    public int Id { get; set; }

    public string Object { get; set; } = null!;

    public string Naam { get; set; } = null!;

    public string Omschrijving { get; set; } = null!;

    public int BoomVolgorde { get; set; }

    public string Stamkaart { get; set; } = null!;

    public int TreeviewtypeId { get; set; }

    public string Serienummer { get; set; } = null!;

    public string Bouwjaar { get; set; } = null!;

    public short Actief { get; set; }

    public int Wijzigactief { get; set; }

    public short Vrijgegeven { get; set; }

    public int Installatiedatum { get; set; }

    public int Garantietot { get; set; }

    public decimal Aanschafwaarde { get; set; }

    public int Afschrijving { get; set; }

    public int Jaarafschrijving { get; set; }

    public short Afschrijvingeen { get; set; }

    public decimal Budgetvorig { get; set; }

    public decimal Budgetnu { get; set; }

    public int Melden { get; set; }

    /// <summary>
    ///     Mag er op dit object correctief gemeld worden?
    /// </summary>
    public bool Correctief { get; set; }

    /// <summary>
    ///     Mag er op dit object een werkopdracht aangemaakt worden?
    /// </summary>
    public bool Werkopdracht { get; set; }

    public int FabrikantenId { get; set; }

    public int LeverancierenId { get; set; }

    public int LocatiesId { get; set; }

    public int KostenplaatsId { get; set; }

    public int Parent { get; set; }

    public int NewId { get; set; }

    public DateOnly OldDatum { get; set; }

    public string Treeviewtype { get; set; } = null!;

    public int KeuringsinstantieId { get; set; }

    public int RieNr { get; set; }

    public int Onderhoud { get; set; }

    /// <summary>
    ///     Mag dit object worden opgenomen in een onderhoudstemplate?
    /// </summary>
    public bool Onderhoudstemplate { get; set; }

    public int TreeviewsoortId { get; set; }

    public bool ShowVisual { get; set; }

    public string Gecodeerd { get; set; } = null!;

    public int EigenaarId { get; set; }

    public int Keuringsplichtig { get; set; }

    public DateOnly? Laatstgeteld { get; set; }

    /// <summary>
    ///     Derden - Voor onderhoud bedr.
    /// </summary>
    public int OnderhoudsbedrijfId { get; set; }

    public string? StamkaartOld { get; set; }

    public int ObjecttemplateId { get; set; }

    public int KoppelrelatieId { get; set; }

    public int Nlsfb2Id { get; set; }

    public decimal VastgoedAantal { get; set; }

    public int VastgoedEenhedenId { get; set; }

    public int? KoppelpersoonId { get; set; }

    public int? Koppelrelatie2Id { get; set; }

    public int? Koppelpersoon2Id { get; set; }

    /// <summary>
    ///     personen_id ( Gemaakt voor Joulz 01-03-2012 )
    /// </summary>
    public int MedewerkerId { get; set; }

    public int OmschrijvingId { get; set; }

    public int OpgenomenInBegroting { get; set; }

    public DateTime? OpgenomenInBegrotingDatum { get; set; }

    public int UitleenMagazijnId { get; set; }

    public int UitleenTreeviewsoortId { get; set; }

    public int IsUitgeleend { get; set; }

    public int UitleenStatus { get; set; }

    public int Uitleenbaar { get; set; }

    public string Barcode { get; set; } = null!;

    public DateTime AangemaaktOp { get; set; }

    public string AangemaaktDoor { get; set; } = null!;

    /// <summary>
    ///     Aparte status voor Joulz voor wanneer een object kapot is, of gestolen, etc.. 20-06-2013
    /// </summary>
    public int NonactiefId { get; set; }

    public string Dragernr { get; set; } = null!;

    public int StamkaartenId { get; set; }

    public string Maat { get; set; } = null!;

    public int DeliveryaddressNumber { get; set; }

    public string DeliveryaddressName { get; set; } = null!;

    public int Kastnr { get; set; }

    public int Vak { get; set; }

    public DateOnly DatumInbehandeling { get; set; }

    public DateOnly DatumLaatsteWasbeurt { get; set; }

    public string Kenteken { get; set; } = null!;

    public DateOnly DatumUitleen { get; set; }

    public DateOnly GeplandeDatumOntvangst { get; set; }

    public DateOnly DatumLaatsteUitscan { get; set; }

    public DateOnly DatumAflevering { get; set; }

    /// <summary>
    ///     Wat was de tellerstand bij de laatste beurt voor de ingebruikname van dit object
    /// </summary>
    public double LaatsteTellerstand { get; set; }

    /// <summary>
    ///     Wanneer is de laatste beurt uitgevoerd voor ingebruikname van dit object.
    /// </summary>
    public DateOnly LaatsteBeurtDatum { get; set; }

    /// <summary>
    ///     Dit geeft aan of de tellerstand al eens is opgegeven. M.a.w. moet er een aparte berekening worden uitgevoerd bij de
    ///     opnamelijst.
    /// </summary>
    public bool TellerstandOpgenomen { get; set; }

    /// <summary>
    ///     JOU-1: Is het object geaccepteerd als klant
    /// </summary>
    public bool Geaccepteerd { get; set; }

    /// <summary>
    ///     Mogen er werkorders middels de SWEEP API ingeschoten worden voor dit object?
    /// </summary>
    public bool SweepApi { get; set; }

    /// <summary>
    ///     Dit is frequentie van de laatste beurt
    /// </summary>
    public int LaatsteBeurt { get; set; }

    /// <summary>
    ///     Welke servicebeurt is het laatste uitgevoerd voor ingebruikname van dit object
    /// </summary>
    public int? LaatsteServicebeurtId { get; set; }

    public int HoofdprocessenId { get; set; }

    public int ProcesstappenId { get; set; }

    public int MachineMonitoringenTimeoutSec { get; set; }

    public int ToegangTypeId { get; set; }

    public bool ExternToegangsbeleid { get; set; }

    public double MachineMonitoringStreefCyclusTijd { get; set; }

    public string Adres { get; set; } = null!;

    public string Postcode { get; set; } = null!;

    public string Plaats { get; set; } = null!;
}