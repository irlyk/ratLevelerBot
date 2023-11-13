
namespace RatLevelerBot.Models;

public class Level
{
    public static int FirstLevel = 0;
    public long Id {get; set;}
	public int Value { get; set; }
    public string Name {get; set;} = null!;
	public long Exp { get; set; }
    public ICollection<UserLevel> UserLevels { get; set; } = new List<UserLevel>();
}

