namespace Domain.Entities;

public class MachineMonitoringPoorten
{
    public int Id { get; set; }

    public int Board { get; set; }

    public int Port { get; set; }

    public string? Name { get; set; }

    public int Volgorde { get; set; }

    public bool Visible { get; set; }
}