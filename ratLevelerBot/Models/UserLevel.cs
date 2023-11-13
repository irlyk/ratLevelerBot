namespace RatLevelerBot.Models;

public class UserLevel 
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    public long LevelId { get; set; }
    public Level Level { get; set; } = null!;
    public long ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
    public long Exp;
}