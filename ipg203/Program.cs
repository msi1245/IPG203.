using ApplianceMaintenanceSystem.Models;
using ApplianceMaintenanceSystem.Services;

namespace ApplianceMaintenanceSystem;

internal class Program
{
    private static readonly MaintenanceCenter Center = new();

    private static void Main()
    {
        SeedSampleData();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n=== HOME APPLIANCE MAINTENANCE CENTER ===");
            Console.WriteLine("1. Add refrigerator");
            Console.WriteLine("2. Add washing machine");
            Console.WriteLine("3. Add air conditioner");
            Console.WriteLine("4. Display all devices");
            Console.WriteLine("5. Search device by serial number");
            Console.WriteLine("6. Update maintenance status");
            Console.WriteLine("7. Display devices by status");
            Console.WriteLine("8. Check delayed deliveries");
            Console.WriteLine("9. Display statistics");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            switch (Console.ReadLine())
            {
                case "1": AddRefrigerator(); break;
                case "2": AddWashingMachine(); break;
                case "3": AddAirConditioner(); break;
                case "4": Center.DisplayAll(); break;
                case "5": SearchDevice(); break;
                case "6": UpdateStatus(); break;
                case "7": DisplayByStatus(); break;
                case "8": Center.CheckAllDeliveryDates(); break;
                case "9": DisplayStatistics(); break;
                case "0": running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }

    private static void SeedSampleData()
    {
        Center.AddAppliance(new Refrigerator("REF-1001", "Samsung", "RT40",
            DateTime.Today.AddDays(5), 35m, 400));
        Center.AddAppliance(new WashingMachine("WAS-1002", "LG", "F2J",
            DateTime.Today.AddDays(3), 25m, 9));
        Center.AddAppliance(new AirConditioner("AIR-1003", "Gree", "Pular",
            DateTime.Today.AddDays(7), 40m, 18000));
    }

    private static void AddRefrigerator()
    {
        try
        {
            Console.Write("Serial number: "); string serial = ReadRequired();
            Console.Write("Brand: "); string brand = ReadRequired();
            Console.Write("Model: "); string model = ReadRequired();
            Console.Write("Expected delivery date (yyyy-mm-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Parts cost: "); decimal parts = decimal.Parse(Console.ReadLine()!);
            Console.Write("Capacity in liters: "); int capacity = int.Parse(Console.ReadLine()!);
            Center.AddAppliance(new Refrigerator(serial, brand, model, date, parts, capacity));
            Console.WriteLine("Refrigerator added successfully.");
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    private static void AddWashingMachine()
    {
        try
        {
            Console.Write("Serial number: "); string serial = ReadRequired();
            Console.Write("Brand: "); string brand = ReadRequired();
            Console.Write("Model: "); string model = ReadRequired();
            Console.Write("Expected delivery date (yyyy-mm-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Parts cost: "); decimal parts = decimal.Parse(Console.ReadLine()!);
            Console.Write("Load capacity in kg: "); int capacity = int.Parse(Console.ReadLine()!);
            Center.AddAppliance(new WashingMachine(serial, brand, model, date, parts, capacity));
            Console.WriteLine("Washing machine added successfully.");
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    private static void AddAirConditioner()
    {
        try
        {
            Console.Write("Serial number: "); string serial = ReadRequired();
            Console.Write("Brand: "); string brand = ReadRequired();
            Console.Write("Model: "); string model = ReadRequired();
            Console.Write("Expected delivery date (yyyy-mm-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Parts cost: "); decimal parts = decimal.Parse(Console.ReadLine()!);
            Console.Write("Cooling capacity (BTU): "); double capacity = double.Parse(Console.ReadLine()!);
            Center.AddAppliance(new AirConditioner(serial, brand, model, date, parts, capacity));
            Console.WriteLine("Air conditioner added successfully.");
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    private static void SearchDevice()
    {
        Console.Write("Enter serial number: ");
        Appliance? appliance = Center.FindBySerial(ReadRequired());
        if (appliance is null) Console.WriteLine("Device not found.");
        else appliance.DisplayInformation();
    }

    private static void UpdateStatus()
    {
        Console.Write("Enter serial number: "); string serial = ReadRequired();
        Console.WriteLine("0- Received | 1- UnderMaintenance | 2- Repaired | 3- Delivered");
        Console.Write("New status: ");
        if (!int.TryParse(Console.ReadLine(), out int value) ||
            !Enum.IsDefined(typeof(MaintenanceStatus), value))
        {
            Console.WriteLine("Invalid status.");
            return;
        }

        Console.WriteLine(Center.UpdateStatus(serial, (MaintenanceStatus)value)
            ? "Status updated." : "Device not found.");
    }

    private static void DisplayByStatus()
    {
        Console.WriteLine("0- Received | 1- UnderMaintenance | 2- Repaired | 3- Delivered");
        Console.Write("Status: ");
        if (int.TryParse(Console.ReadLine(), out int value) &&
            Enum.IsDefined(typeof(MaintenanceStatus), value))
            Center.DisplayByStatus((MaintenanceStatus)value);
        else
            Console.WriteLine("Invalid status.");
    }

    private static void DisplayStatistics()
    {
        Console.WriteLine($"Total devices created: {Appliance.TotalDevices}");
        Console.WriteLine($"Devices registered here: {Center.Appliances.Count}");
        Console.WriteLine($"Total estimated repair cost: {MaintenanceCenter.TotalRepairCost:0.00}");
        foreach (MaintenanceStatus status in Enum.GetValues<MaintenanceStatus>())
            Console.WriteLine($"{status}: {Center.CountByStatus(status)}");
    }

    private static string ReadRequired()
    {
        string? value = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("This field is required.");
        return value.Trim();
    }
}
