public class UserInfoResponse
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public int Lemons { get; set; }
    public string TotalOnlineTime { get; set; } = string.Empty;
    public bool CanClaimDailyBonus { get; set; }
    public DateTime? LastDailyBonusClaimed { get; set; }
}