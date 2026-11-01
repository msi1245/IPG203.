namespace ApplianceMaintenanceSystem.Models;

public class AirConditioner : Appliance
{
    private double _coolingCapacity;
    public double CoolingCapacity
    {
        get => _coolingCapacity;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _coolingCapacity = value;
        }
    }

    public AirConditioner(string serial, string brand, string model, DateTime delivery,
                          decimal partsCost, double coolingCapacity)
        : base(serial, brand, model, delivery, partsCost)
    {
        CoolingCapacity = coolingCapacity;
    }

    public override decimal CalculateRepairCost() => 95m + PartsCost + (decimal)CoolingingCapacityCost();

    private double CoolingingCapacityCost() => CoolingCapacity * 0.08;

    public override void DisplayInformation()
    {
        base.DisplayInformation();
        Console.WriteLine($"Cooling capacity: {CoolingCapacity:0.0} BTU");
    }
}
