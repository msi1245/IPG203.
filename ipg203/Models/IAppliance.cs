namespace ApplianceMaintenanceSystem.Models;

public interface IAppliance
{
    string SerialNumber { get; }
    string Brand { get; }
    string Model { get; }
    string Status { get; }
    decimal CalculateRepairCost();
    void DisplayInformation();
}
