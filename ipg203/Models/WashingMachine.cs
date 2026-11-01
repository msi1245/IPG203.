namespace ApplianceMaintenanceSystem.Models;

public class WashingMachine : Appliance
{
    private int _loadCapacityKg;
    public int LoadCapacityKg
    {
        get => _loadCapacityKg;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _loadCapacityKg = value;
        }
    }

    public WashingMachine(string serial, string brand, string model, DateTime delivery,
                          decimal partsCost, int loadCapacityKg)
        : base(serial, brand, model, delivery, partsCost)
    {
        LoadCapacityKg = loadCapacityKg;
    }

    public override decimal CalculateRepairCost() => 65m + PartsCost + LoadCapacityKg * 2.5m;

    public override void DisplayInformation()
    {
        base.DisplayInformation();
        Console.WriteLine($"Load capacity: {LoadCapacityKg} kg");
    }
}
