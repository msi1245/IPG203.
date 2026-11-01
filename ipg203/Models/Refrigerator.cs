namespace ApplianceMaintenanceSystem.Models;

public class Refrigerator : Appliance
{
    private int _capacityLiters;
    public int CapacityLiters
    {
        get => _capacityLiters;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _capacityLiters = value;
        }
    }

    public Refrigerator(string serial, string brand, string model, DateTime delivery,
                        decimal partsCost, int capacityLiters)
        : base(serial, brand, model, delivery, partsCost)
    {
        CapacityLiters = capacityLiters;
    }

    public override decimal CalculateRepairCost() => 80m + PartsCost + CapacityLiters * 0.15m;

    public override void DisplayInformation()
    {
        base.DisplayInformation();
        Console.WriteLine($"Capacity: {CapacityLiters} liters");
    }
}
