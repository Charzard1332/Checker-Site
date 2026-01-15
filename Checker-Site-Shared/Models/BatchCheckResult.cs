namespace Checker_Site_Shared.Models;

// result of a batch check operation
public class BatchCheckResult
{
    // batch operation id
    public Guid BatchId { get; set; } = Guid.NewGuid();
    
    // individual results for each account
    public List<AccountCheckResult> Results { get; set; } = new();
    
    // total number of accounts checked
    public int TotalAccountsChecked { get; set; }
    
    // total number of valid accounts found
    public int TotalValidAccounts { get; set; }
    
    // total number of invalid accounts found
    public int TotalInvalidAccounts { get; set; }
    
    // number of errors encountered during the check
    public int ErrorCount { get; set; }
    
    // time taken to perform the check (MS)
    public long CheckDurationMs { get; set; }
    
    // timestamp of the started check
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    
    // timestamp of the completed check
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    
    // total skins found across all accounts
    public int TotalSkinsFound { get; set; }
    
    // total v-bucks found across all accounts
    public int TotalVBucksFound { get; set; }
    
    // all unique skin IDs found across all valid accounts
    public List<string> AllSkinIds { get; set; } = new();
}