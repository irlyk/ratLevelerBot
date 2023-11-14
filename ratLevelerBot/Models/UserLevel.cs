namespace RatLevelerBot.Models;

public class UserLevel 
{
    public long Id { get; set; }
    public User User { get; set; } = null!;
    public Level Level { get; set; } = null!;
    public Chat Chat { get; set; } = null!;
    public long Exp { get; set; }
}