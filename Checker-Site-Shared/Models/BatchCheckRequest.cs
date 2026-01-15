namespace Checker_Site_Shared.Models;

// request model for checking multiple accounts
public class BatchCheckRequest
{
    // list of accounts to check
    public List<AccountCheckRequest> Accounts { get; set; } = new();
    
    // service types for all accounts
    public string? ServiceType { get; set; }
    
    // max concurrent checks (default = 5)
    public int? MaxConcurrentChecks { get; set; }
}