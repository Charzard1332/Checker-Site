namespace Checker_Site_Shared.Models;

// request model for checking a single account (not needed but its handy)
public class AccountCheckRequest
{
    // username/email to check
    public string Email { get; set; } = string.Empty;
    
    // password for the account
    public string Password { get; set; } = string.Empty;
    
    // service type like (Epic, Steam, Discord, etc..)
    public string? ServiceType { get; set; }
    
    // this is optional and not needed. but here it is anyway
    // proxy to use for the request
    public string? Proxy { get; set; }
}