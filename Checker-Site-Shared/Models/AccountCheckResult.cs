namespace Checker_Site_Shared.Models;

// result of an account check operation
public class AccountCheckResult
{
    // guid identifier for the check = session identifier
    // this allows multiple checks with different users without
    // cross-threading into other operations
    public Guid ID { get; set; } = Guid.NewGuid();
    
    // username that was checked
    public string Username { get; set; } = string.Empty;
    
    // password that was checked (stored for valid accounts)
    public string? Password { get; set; }
    
    // status of account check
    public AccountStatus Status { get; set; }
    
    // message about the check result
    public string Message { get; set; } = string.Empty;
    
    // timestamp of the check
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    
    // service type that was checked
    public string? ServiceType { get; set; }
    
    // time taken to perfrom the check (MS)
    public long CheckDurationMs { get; set; }
    
    // additional data related to the check only (if valid)
    public Dictionary<string, string>? AdditionalData { get; set; }
    
    // number of retries attempted
    public int RetryCount { get; set; }
    
    // total skins count (only if valid is found)
    public int? TotalSkins { get; set; }
    
    // number of rare skins (only if valid is found)
    public int? TotalRareSkins { get; set; }
    
    // list of rare skin names (only if valid is found)
    public List<string>? RareSkinNames { get; set; }
    
    // list of skins template IDs (only if valid is found)
    public List<int>? SkinTemplateIDs { get; set; }
    
    // total v-bucks for the account (only if valid is found)
    public int TotalVBucks { get; set; }
}

// status of an account check
public enum AccountStatus
{
    // valid
    Valid,
    // invalid
    Invalid,
    // could not determine status
    Unknown,
    // error occurred during check
    Error,
    // requires 2fa
    RequiresTwoFactor,
    // locked or banned
    LockedOrBanned
}