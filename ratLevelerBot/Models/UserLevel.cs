namespace RatLevelerBot.Models;

public class UserLevel 
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public Level Level { get; set; } = null!;
    public long ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
    public long Exp;
}