using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using q4_api.Dto;

namespace q4_api.Context;

public partial class Q4DbContext : DbContext
{
    public Q4DbContext()
    {
    }

    public Q4DbContext(DbContextOptions<Q4DbContext> options)
        : base(options)
    {
    }

    public IQueryable<MonitorData> MonitoringData =>
        from a in MonitoringData202009s
        select new MonitorData
        {
            Codes = new List<int> { a.Code, a.Code2 },
            Duration = a.ShotTime,
            Timestamp = a.Timestamp ?? DateTime.UnixEpoch,
        };

    public IQueryable<Reading> Readings =>
        from a in Tellerbases
        join b in Tellerstandens on a.Id equals b.TellerbasisId
        select new Reading
        {
            TreeviewId = a.Id,
            Name = a.Naam,
            Description = a.Omschrijving,
            Active = a.Actief == 1,
            MaxValue = a.MaxWaarde,
            Values = Tellerstandens.Where(ts => ts.Id == b.Id).Select(ts => new ReadingValue
                { Total = ts.Totaal, Value = ts.Waarde, Timestamp = ts.Datum }).ToList()
        };

    public IQueryable<ReadingValue> ReadingValues =>
        from a in Tellerstandens
        select new ReadingValue
        {
            Value = a.Waarde,
            Total = a.Totaal,
            Timestamp = a.Datum,
            TreeviewId = a.TreeviewId
        };

    public IQueryable<Machine> Machines =>
        from a in ProductionData
        join b in Treeviews on a.TreeviewId equals b.Id
        select new Machine
        {
            Id = b.Id,
            Name = b.Naam,
            Readings = ReadingValues.Where(v => v.TreeviewId == b.Id).ToList(),
        };

    public IQueryable<Mold> Molds =>
        from a in ProductionData
        join b in Treeviews on a.Treeview2Id equals b.Id
        select new Mold
        {
            Id = a.TreeviewId,
            Name = b.Naam
        };

    public virtual DbSet<MachineMonitoringPoorten> MachineMonitoringPoortens { get; set; }

    public virtual DbSet<MonitoringData202009> MonitoringData202009s { get; set; }

    public virtual DbSet<ProductionDatum> ProductionData { get; set; }

    public virtual DbSet<Tellerbasis> Tellerbases { get; set; }

    public virtual DbSet<Tellerstanden> Tellerstandens { get; set; }

    public virtual DbSet<Treeview> Treeviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(
            Environment.GetEnvironmentVariable("CONNECTION_STRING"),
            ServerVersion.Parse("5.7.44-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<MachineMonitoringPoorten>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("machine_monitoring_poorten");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Board)
                .HasColumnType("int(11)")
                .HasColumnName("board");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Port)
                .HasColumnType("int(11)")
                .HasColumnName("port");
            entity.Property(e => e.Visible).HasColumnName("visible");
            entity.Property(e => e.Volgorde)
                .HasColumnType("int(11)")
                .HasColumnName("volgorde");
        });

        modelBuilder.Entity<MonitoringData202009>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("monitoring_data_202009");

            entity.HasIndex(e => e.Board, "board");

            entity.HasIndex(e => new { e.Board, e.Port }, "board_port");

            entity.HasIndex(e => new { e.Board, e.Port, e.Datum }, "board_port_datum");

            entity.HasIndex(e => e.Code, "code");

            entity.HasIndex(e => e.Datum, "datum");

            entity.HasIndex(e => e.Port, "port");

            entity.HasIndex(e => e.Timestamp, "timestamp");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Board)
                .HasColumnType("tinyint(4)")
                .HasColumnName("board");
            entity.Property(e => e.Code)
                .HasColumnType("int(11)")
                .HasColumnName("code");
            entity.Property(e => e.Code2)
                .HasColumnType("int(11)")
                .HasColumnName("code2");
            entity.Property(e => e.Com)
                .HasColumnType("tinyint(4)")
                .HasColumnName("com");
            entity.Property(e => e.Datum).HasColumnName("datum");
            entity.Property(e => e.MacAddress)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("mac_address");
            entity.Property(e => e.Port)
                .HasColumnType("tinyint(4)")
                .HasColumnName("port");
            entity.Property(e => e.PreviousShotId)
                .HasColumnType("int(11)")
                .HasColumnName("previous_shot_id");
            entity.Property(e => e.ShotTime)
                .HasColumnType("double(11,6)")
                .HasColumnName("shot_time");
            entity.Property(e => e.Timestamp)
                .HasMaxLength(6)
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<ProductionDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("production_data");

            entity.HasIndex(e => e.Board, "board");

            entity.HasIndex(e => new { e.EndDate, e.EndTime }, "end_date_end_time");

            entity.HasIndex(e => e.Port, "port");

            entity.HasIndex(e => new { e.StartDate, e.StartTime }, "start_date_start_time");

            entity.HasIndex(e => e.TreeviewId, "treeview_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasComment("Hoeveel wordt er per run geproduceerd")
                .HasColumnType("double(11,2)")
                .HasColumnName("amount");
            entity.Property(e => e.Board)
                .HasColumnType("tinyint(4)")
                .HasColumnName("board");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasDefaultValueSql("'0000-00-00'")
                .HasColumnName("end_date");
            entity.Property(e => e.EndTime)
                .HasDefaultValueSql("'00:00:00'")
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("name");
            entity.Property(e => e.Port)
                .HasColumnType("tinyint(4)")
                .HasColumnName("port");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("'0000-00-00'")
                .HasColumnName("start_date");
            entity.Property(e => e.StartTime)
                .HasDefaultValueSql("'00:00:00'")
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.Treeview2Id)
                .HasComment("Een \"Hot half\"")
                .HasColumnType("int(11)")
                .HasColumnName("treeview2_id");
            entity.Property(e => e.TreeviewId)
                .HasColumnType("int(11)")
                .HasColumnName("treeview_id");
        });

        modelBuilder.Entity<Tellerbasis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tellerbasis")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Actief, "actief");

            entity.HasIndex(e => e.Naam, "naam");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Actief)
                .HasColumnType("int(1)")
                .HasColumnName("actief");
            entity.Property(e => e.Afkorting)
                .HasMaxLength(255)
                .HasColumnName("afkorting");
            entity.Property(e => e.MaxWaarde)
                .HasColumnType("double(11,2)")
                .HasColumnName("max_waarde");
            entity.Property(e => e.Naam)
                .HasDefaultValueSql("''")
                .HasColumnName("naam");
            entity.Property(e => e.Omschrijving)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("omschrijving");
            entity.Property(e => e.Optie)
                .HasColumnType("int(11)")
                .HasColumnName("optie");
        });

        modelBuilder.Entity<Tellerstanden>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tellerstanden")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.TellerbasisId, "tellerbasis_id");

            entity.HasIndex(e => e.TreeviewId, "treeview_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Datum)
                .HasColumnType("int(11)")
                .HasColumnName("datum");
            entity.Property(e => e.TellerbasisId)
                .HasColumnType("int(11)")
                .HasColumnName("tellerbasis_id");
            entity.Property(e => e.Totaal)
                .HasPrecision(11, 2)
                .HasColumnName("totaal");
            entity.Property(e => e.TreeviewId)
                .HasColumnType("int(11)")
                .HasColumnName("treeview_id");
            entity.Property(e => e.Waarde)
                .HasPrecision(11, 2)
                .HasColumnName("waarde");
        });

        modelBuilder.Entity<Treeview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("treeview")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Actief, "actief");

            entity.HasIndex(e => e.Bouwjaar, "bouwjaar");

            entity.HasIndex(e => e.EigenaarId, "eigenaar_id");

            entity.HasIndex(e => e.FabrikantenId, "fabrikanten_id");

            entity.HasIndex(e => e.HoofdprocessenId, "hoofdprocessen_id");

            entity.HasIndex(e => e.IsUitgeleend, "is_uitgeleend");

            entity.HasIndex(e => e.Jaarafschrijving, "jaarafschrijving");

            entity.HasIndex(e => e.KeuringsinstantieId, "keuringsinstantie_id");

            entity.HasIndex(e => e.Keuringsplichtig, "keuringsplichtig");

            entity.HasIndex(e => e.KoppelpersoonId, "koppelpersoon_id");

            entity.HasIndex(e => e.Koppelrelatie2Id, "koppelrelatie2_id");

            entity.HasIndex(e => e.KoppelrelatieId, "koppelrelatie_id");

            entity.HasIndex(e => e.KostenplaatsId, "kostenplaats_id");

            entity.HasIndex(e => e.LeverancierenId, "leverancieren_id");

            entity.HasIndex(e => e.LocatiesId, "locaties_id");

            entity.HasIndex(e => e.MedewerkerId, "medewerker_id");

            entity.HasIndex(e => e.Naam, "naam");

            entity.HasIndex(e => e.NewId, "new_id");

            entity.HasIndex(e => e.Nlsfb2Id, "nlsfb2_id");

            entity.HasIndex(e => e.OmschrijvingId, "omschrijving_id");

            entity.HasIndex(e => e.OnderhoudsbedrijfId, "onderhoudsbedrijf_id");

            entity.HasIndex(e => e.Parent, "parent");

            entity.HasIndex(e => e.ProcesstappenId, "processtappen_id");

            entity.HasIndex(e => e.RieNr, "rie_nr");

            entity.HasIndex(e => e.Serienummer, "serienummer");

            entity.HasIndex(e => e.ShowVisual, "show_visual");

            entity.HasIndex(e => e.ToegangTypeId, "toegang_type_id");

            entity.HasIndex(e => e.TreeviewsoortId, "treeviewsoort_id");

            entity.HasIndex(e => e.TreeviewtypeId, "treeviewtype_id");

            entity.HasIndex(e => e.UitleenMagazijnId, "uitleen_magazijn_id");

            entity.HasIndex(e => e.UitleenTreeviewsoortId, "uitleen_treeviewsoort_id");

            entity.HasIndex(e => e.VastgoedEenhedenId, "vastgoed_eenheden_id");

            entity.HasIndex(e => e.Wijzigactief, "wijzigactief");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AangemaaktDoor)
                .HasMaxLength(255)
                .HasColumnName("aangemaakt_door");
            entity.Property(e => e.AangemaaktOp)
                .HasColumnType("datetime")
                .HasColumnName("aangemaakt_op");
            entity.Property(e => e.Aanschafwaarde)
                .HasPrecision(11, 2)
                .HasColumnName("aanschafwaarde");
            entity.Property(e => e.Actief)
                .HasDefaultValueSql("'1'")
                .HasColumnType("smallint(6)")
                .HasColumnName("actief");
            entity.Property(e => e.Adres)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("adres");
            entity.Property(e => e.Afschrijving)
                .HasColumnType("int(11)")
                .HasColumnName("afschrijving");
            entity.Property(e => e.Afschrijvingeen)
                .HasColumnType("smallint(4)")
                .HasColumnName("afschrijvingeen");
            entity.Property(e => e.Barcode)
                .HasMaxLength(255)
                .HasColumnName("barcode");
            entity.Property(e => e.BoomVolgorde)
                .HasColumnType("int(11)")
                .HasColumnName("boom_volgorde");
            entity.Property(e => e.Bouwjaar)
                .HasDefaultValueSql("''")
                .HasColumnName("bouwjaar");
            entity.Property(e => e.Budgetnu)
                .HasPrecision(11, 2)
                .HasColumnName("budgetnu");
            entity.Property(e => e.Budgetvorig)
                .HasPrecision(11, 2)
                .HasColumnName("budgetvorig");
            entity.Property(e => e.Correctief)
                .HasComment("Mag er op dit object correctief gemeld worden?")
                .HasColumnName("correctief");
            entity.Property(e => e.DatumAflevering).HasColumnName("datum_aflevering");
            entity.Property(e => e.DatumInbehandeling).HasColumnName("datum_inbehandeling");
            entity.Property(e => e.DatumLaatsteUitscan).HasColumnName("datum_laatste_uitscan");
            entity.Property(e => e.DatumLaatsteWasbeurt).HasColumnName("datum_laatste_wasbeurt");
            entity.Property(e => e.DatumUitleen).HasColumnName("datum_uitleen");
            entity.Property(e => e.DeliveryaddressName)
                .HasMaxLength(255)
                .HasColumnName("deliveryaddress_name");
            entity.Property(e => e.DeliveryaddressNumber)
                .HasColumnType("int(11)")
                .HasColumnName("deliveryaddress_number");
            entity.Property(e => e.Dragernr)
                .HasMaxLength(255)
                .HasColumnName("dragernr");
            entity.Property(e => e.EigenaarId)
                .HasColumnType("int(11)")
                .HasColumnName("eigenaar_id");
            entity.Property(e => e.ExternToegangsbeleid).HasColumnName("extern_toegangsbeleid");
            entity.Property(e => e.FabrikantenId)
                .HasColumnType("int(11)")
                .HasColumnName("fabrikanten_id");
            entity.Property(e => e.Garantietot)
                .HasColumnType("int(11)")
                .HasColumnName("garantietot");
            entity.Property(e => e.Geaccepteerd)
                .HasComment("JOU-1: Is het object geaccepteerd als klant")
                .HasColumnName("geaccepteerd");
            entity.Property(e => e.Gecodeerd)
                .HasMaxLength(65)
                .HasDefaultValueSql("''")
                .HasColumnName("gecodeerd");
            entity.Property(e => e.GeplandeDatumOntvangst).HasColumnName("geplande_datum_ontvangst");
            entity.Property(e => e.HoofdprocessenId)
                .HasColumnType("int(11)")
                .HasColumnName("hoofdprocessen_id");
            entity.Property(e => e.Installatiedatum)
                .HasColumnType("int(11)")
                .HasColumnName("installatiedatum");
            entity.Property(e => e.IsUitgeleend)
                .HasColumnType("int(1)")
                .HasColumnName("is_uitgeleend");
            entity.Property(e => e.Jaarafschrijving)
                .HasColumnType("int(11)")
                .HasColumnName("jaarafschrijving");
            entity.Property(e => e.Kastnr)
                .HasColumnType("int(11)")
                .HasColumnName("kastnr");
            entity.Property(e => e.Kenteken)
                .HasMaxLength(255)
                .HasColumnName("kenteken");
            entity.Property(e => e.KeuringsinstantieId)
                .HasColumnType("int(11)")
                .HasColumnName("keuringsinstantie_id");
            entity.Property(e => e.Keuringsplichtig)
                .HasColumnType("int(11)")
                .HasColumnName("keuringsplichtig");
            entity.Property(e => e.Koppelpersoon2Id)
                .HasColumnType("int(11)")
                .HasColumnName("koppelpersoon2_id");
            entity.Property(e => e.KoppelpersoonId)
                .HasColumnType("int(11)")
                .HasColumnName("koppelpersoon_id");
            entity.Property(e => e.Koppelrelatie2Id)
                .HasColumnType("int(11)")
                .HasColumnName("koppelrelatie2_id");
            entity.Property(e => e.KoppelrelatieId)
                .HasColumnType("int(11)")
                .HasColumnName("koppelrelatie_id");
            entity.Property(e => e.KostenplaatsId)
                .HasColumnType("int(11)")
                .HasColumnName("kostenplaats_id");
            entity.Property(e => e.LaatsteBeurt)
                .HasComment("Dit is frequentie van de laatste beurt ")
                .HasColumnType("int(11)")
                .HasColumnName("laatste_beurt");
            entity.Property(e => e.LaatsteBeurtDatum)
                .HasComment("Wanneer is de laatste beurt uitgevoerd voor ingebruikname van dit object.")
                .HasColumnName("laatste_beurt_datum");
            entity.Property(e => e.LaatsteServicebeurtId)
                .HasComment("Welke servicebeurt is het laatste uitgevoerd voor ingebruikname van dit object")
                .HasColumnType("int(11)")
                .HasColumnName("laatste_servicebeurt_id");
            entity.Property(e => e.LaatsteTellerstand)
                .HasComment("Wat was de tellerstand bij de laatste beurt voor de ingebruikname van dit object")
                .HasColumnType("double(11,2)")
                .HasColumnName("laatste_tellerstand");
            entity.Property(e => e.Laatstgeteld).HasColumnName("laatstgeteld");
            entity.Property(e => e.LeverancierenId)
                .HasColumnType("int(11)")
                .HasColumnName("leverancieren_id");
            entity.Property(e => e.LocatiesId)
                .HasColumnType("int(11)")
                .HasColumnName("locaties_id");
            entity.Property(e => e.Maat)
                .HasMaxLength(255)
                .HasColumnName("maat");
            entity.Property(e => e.MachineMonitoringStreefCyclusTijd)
                .HasColumnType("double(11,2)")
                .HasColumnName("machine_monitoring_streef_cyclus_tijd");
            entity.Property(e => e.MachineMonitoringenTimeoutSec)
                .HasDefaultValueSql("'30'")
                .HasColumnType("int(11)")
                .HasColumnName("machine_monitoringen_timeout_sec");
            entity.Property(e => e.MedewerkerId)
                .HasComment("personen_id ( Gemaakt voor Joulz 01-03-2012 )")
                .HasColumnType("int(11)")
                .HasColumnName("medewerker_id");
            entity.Property(e => e.Melden)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(1)")
                .HasColumnName("melden");
            entity.Property(e => e.Naam)
                .HasDefaultValueSql("''")
                .HasColumnName("naam");
            entity.Property(e => e.NewId)
                .HasColumnType("int(11)")
                .HasColumnName("new_id");
            entity.Property(e => e.Nlsfb2Id)
                .HasColumnType("int(11)")
                .HasColumnName("nlsfb2_id");
            entity.Property(e => e.NonactiefId)
                .HasComment("Aparte status voor Joulz voor wanneer een object kapot is, of gestolen, etc.. 20-06-2013")
                .HasColumnType("int(11)")
                .HasColumnName("nonactief_id");
            entity.Property(e => e.Object)
                .HasMaxLength(1)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasColumnName("object");
            entity.Property(e => e.ObjecttemplateId)
                .HasColumnType("int(11)")
                .HasColumnName("objecttemplate_id");
            entity.Property(e => e.OldDatum)
                .HasDefaultValueSql("'0000-00-00'")
                .HasColumnName("old_datum");
            entity.Property(e => e.Omschrijving)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("omschrijving");
            entity.Property(e => e.OmschrijvingId)
                .HasColumnType("int(11)")
                .HasColumnName("omschrijving_id");
            entity.Property(e => e.Onderhoud)
                .HasColumnType("int(11)")
                .HasColumnName("onderhoud");
            entity.Property(e => e.OnderhoudsbedrijfId)
                .HasComment("Derden - Voor onderhoud bedr.")
                .HasColumnType("int(6)")
                .HasColumnName("onderhoudsbedrijf_id");
            entity.Property(e => e.Onderhoudstemplate)
                .HasComment("Mag dit object worden opgenomen in een onderhoudstemplate?")
                .HasColumnName("onderhoudstemplate");
            entity.Property(e => e.OpgenomenInBegroting)
                .HasColumnType("int(11)")
                .HasColumnName("opgenomen_in_begroting");
            entity.Property(e => e.OpgenomenInBegrotingDatum)
                .HasColumnType("datetime")
                .HasColumnName("opgenomen_in_begroting_datum");
            entity.Property(e => e.Parent)
                .HasColumnType("int(11)")
                .HasColumnName("parent");
            entity.Property(e => e.Plaats)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("plaats");
            entity.Property(e => e.Postcode)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("postcode");
            entity.Property(e => e.ProcesstappenId)
                .HasColumnType("int(11)")
                .HasColumnName("processtappen_id");
            entity.Property(e => e.RieNr)
                .HasColumnType("int(11)")
                .HasColumnName("rie_nr");
            entity.Property(e => e.Serienummer)
                .HasDefaultValueSql("''")
                .HasColumnName("serienummer");
            entity.Property(e => e.ShowVisual).HasColumnName("show_visual");
            entity.Property(e => e.Stamkaart)
                .HasColumnType("mediumtext")
                .HasColumnName("stamkaart");
            entity.Property(e => e.StamkaartOld)
                .HasColumnType("mediumtext")
                .HasColumnName("stamkaart_old");
            entity.Property(e => e.StamkaartenId)
                .HasColumnType("int(11)")
                .HasColumnName("stamkaarten_id");
            entity.Property(e => e.SweepApi)
                .HasComment("Mogen er werkorders middels de SWEEP API ingeschoten worden voor dit object?")
                .HasColumnName("sweep_api");
            entity.Property(e => e.TellerstandOpgenomen)
                .HasComment(
                    "Dit geeft aan of de tellerstand al eens is opgegeven. M.a.w. moet er een aparte berekening worden uitgevoerd bij de opnamelijst.")
                .HasColumnName("tellerstand_opgenomen");
            entity.Property(e => e.ToegangTypeId)
                .HasColumnType("int(11)")
                .HasColumnName("toegang_type_id");
            entity.Property(e => e.TreeviewsoortId)
                .HasColumnType("int(11)")
                .HasColumnName("treeviewsoort_id");
            entity.Property(e => e.Treeviewtype)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("treeviewtype");
            entity.Property(e => e.TreeviewtypeId)
                .HasColumnType("int(11)")
                .HasColumnName("treeviewtype_id");
            entity.Property(e => e.UitleenMagazijnId)
                .HasColumnType("int(11)")
                .HasColumnName("uitleen_magazijn_id");
            entity.Property(e => e.UitleenStatus)
                .HasColumnType("int(11)")
                .HasColumnName("uitleen_status");
            entity.Property(e => e.UitleenTreeviewsoortId)
                .HasColumnType("int(11)")
                .HasColumnName("uitleen_treeviewsoort_id");
            entity.Property(e => e.Uitleenbaar)
                .HasColumnType("int(11)")
                .HasColumnName("uitleenbaar");
            entity.Property(e => e.Vak)
                .HasColumnType("int(11)")
                .HasColumnName("vak");
            entity.Property(e => e.VastgoedAantal)
                .HasPrecision(11, 2)
                .HasColumnName("vastgoed_aantal");
            entity.Property(e => e.VastgoedEenhedenId)
                .HasColumnType("int(11)")
                .HasColumnName("vastgoed_eenheden_id");
            entity.Property(e => e.Vrijgegeven)
                .HasColumnType("smallint(6)")
                .HasColumnName("vrijgegeven");
            entity.Property(e => e.Werkopdracht)
                .HasComment("Mag er op dit object een werkopdracht aangemaakt worden?")
                .HasColumnName("werkopdracht");
            entity.Property(e => e.Wijzigactief)
                .HasColumnType("int(11)")
                .HasColumnName("wijzigactief");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}