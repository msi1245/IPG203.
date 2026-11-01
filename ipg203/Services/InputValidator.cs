using System.Text.RegularExpressions;

namespace ApplianceMaintenanceSystem.Services;

public static class InputValidator
{
    public static bool IsValidSerialNumber(string? serial) =>
        !string.IsNullOrWhiteSpace(serial) &&
        Regex.IsMatch(serial.Trim(), @"^[A-Za-z0-9-]{4,20}$");

    public static bool IsValidText(string? value) =>
        !string.IsNullOrWhiteSpace(value) && value.Trim().Length <= 50;

    public static bool IsValidDate(DateTime date) => date.Date >= DateTime.Today;

    public static bool IsValidCost(decimal cost) => cost >= 0;
}
