namespace RatLevelerBot.Models;

public class Chat 
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<UserLevel> UserLevels { get; set; } = new List<UserLevel>();
}