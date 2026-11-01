namespace ApplianceMaintenanceSystem.Models;

public delegate void MaintenanceStatusHandler(Appliance appliance, string message);

public abstract class Appliance : IAppliance
{
    private readonly string _serialNumber;
    private string _brand;
    private string _model;
    private MaintenanceStatus _status;
    private DateTime _receivedDate;
    private DateTime _expectedDeliveryDate;
    private decimal _partsCost;

    public static int TotalDevices { get; private set; }

    public event MaintenanceStatusHandler? MaintenanceStatusChanged;

    public string SerialNumber => _serialNumber;
    public string Brand
    {
        get => _brand;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Brand cannot be empty.");
            _brand = value.Trim();
        }
    }
    public string Model
    {
        get => _model;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Model cannot be empty.");
            _model = value.Trim();
        }
    }
    public MaintenanceStatus Status => _status;
    public DateTime ReceivedDate => _receivedDate;
    public DateTime ExpectedDeliveryDate
    {
        get => _expectedDeliveryDate;
        set
        {
            if (value.Date < _receivedDate.Date)
                throw new ArgumentException("Delivery date cannot be before received date.");
            _expectedDeliveryDate = value;
        }
    }
    public decimal PartsCost
    {
        get => _partsCost;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            _partsCost = value;
        }
    }

    protected Appliance(string serialNumber, string brand, string model,
                        DateTime expectedDeliveryDate, decimal partsCost)
    {
        if (!InputValidator.IsValidSerialNumber(serialNumber))
            throw new ArgumentException("Serial number must contain 4-20 letters or digits.");
        if (!InputValidator.IsValidText(brand) || !InputValidator.IsValidText(model))
            throw new ArgumentException("Brand and model are required.");
        if (expectedDeliveryDate.Date < DateTime.Today)
            throw new ArgumentException("Expected delivery date cannot be in the past.");
        if (partsCost < 0) throw new ArgumentOutOfRangeException(nameof(partsCost));

        _serialNumber = serialNumber.Trim();
        _brand = brand.Trim();
        _model = model.Trim();
        _receivedDate = DateTime.Today;
        _expectedDeliveryDate = expectedDeliveryDate.Date;
        _partsCost = partsCost;
        _status = MaintenanceStatus.Received;
        TotalDevices++;
    }

    public void UpdateStatus(MaintenanceStatus newStatus)
    {
        if (_status == MaintenanceStatus.Delivered)
            throw new InvalidOperationException("A delivered device cannot be updated.");

        _status = newStatus;
        MaintenanceStatusChanged?.Invoke(this, $"Device {SerialNumber} status changed to {newStatus}.");

        if (newStatus == MaintenanceStatus.Repaired)
            MaintenanceStatusChanged?.Invoke(this, $"Repair completed for device {SerialNumber}.");
    }

    public void CheckDeliveryDelay()
    {
        if (DateTime.Today > ExpectedDeliveryDate.Date &&
            Status != MaintenanceStatus.Repaired &&
            Status != MaintenanceStatus.Delivered)
        {
            MaintenanceStatusChanged?.Invoke(this, $"WARNING: Device {SerialNumber} is overdue.");
        }
    }

    public abstract decimal CalculateRepairCost();

    public virtual void DisplayInformation()
    {
        Console.WriteLine($"Type: {GetType().Name}");
        Console.WriteLine($"Serial: {SerialNumber} | Brand: {Brand} | Model: {Model}");
        Console.WriteLine($"Status: {Status} | Received: {ReceivedDate:yyyy-MM-dd}");
        Console.WriteLine($"Expected delivery: {ExpectedDeliveryDate:yyyy-MM-dd}");
        Console.WriteLine($"Repair cost: {CalculateRepairCost():0.00}");
    }
}
