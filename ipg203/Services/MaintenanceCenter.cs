using ApplianceMaintenanceSystem.Models;

namespace ApplianceMaintenanceSystem.Services;

public class MaintenanceCenter
{
    private readonly List<Appliance> _appliances = new();
    private static decimal _totalRepairCost;

    public static decimal TotalRepairCost => _totalRepairCost;
    public IReadOnlyList<Appliance> Appliances => _appliances.AsReadOnly();

    public MaintenanceCenter()
    {
        // The list is the central collection used for polymorphic processing.
    }

    public void AddAppliance(Appliance appliance)
    {
        if (_appliances.Any(a => a.SerialNumber.Equals(appliance.SerialNumber,
                StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("A device with this serial number already exists.");

        appliance.MaintenanceStatusChanged += OnMaintenanceStatusChanged;
        _appliances.Add(appliance);
        _totalRepairCost += appliance.CalculateRepairCost();
    }

    public Appliance? FindBySerial(string serial) =>
        _appliances.FirstOrDefault(a =>
            a.SerialNumber.Equals(serial, StringComparison.OrdinalIgnoreCase));

    public bool UpdateStatus(string serial, MaintenanceStatus status)
    {
        Appliance? appliance = FindBySerial(serial);
        if (appliance is null) return false;
        appliance.UpdateStatus(status);
        return true;
    }

    public void DisplayAll()
    {
        if (_appliances.Count == 0)
        {
            Console.WriteLine("No devices have been registered.");
            return;
        }

        foreach (Appliance appliance in _appliances)
        {
            appliance.DisplayInformation();
            Console.WriteLine(new string('-', 45));
        }
    }

    public void DisplayByStatus(MaintenanceStatus status)
    {
        var matches = _appliances.Where(a => a.Status == status).ToList();
        if (matches.Count == 0)
        {
            Console.WriteLine($"No devices with status: {status}");
            return;
        }

        foreach (Appliance appliance in matches)
        {
            appliance.DisplayInformation();
            Console.WriteLine(new string('-', 45));
        }
    }

    public void CheckAllDeliveryDates()
    {
        foreach (Appliance appliance in _appliances)
            appliance.CheckDeliveryDelay();
    }

    public int CountByStatus(MaintenanceStatus status) =>
        _appliances.Count(a => a.Status == status);

    private static void OnMaintenanceStatusChanged(Appliance appliance, string message)
    {
        Console.WriteLine($"[MAINTENANCE EVENT] {message}");
    }
}
