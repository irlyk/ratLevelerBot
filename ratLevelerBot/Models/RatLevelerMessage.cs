namespace RatLevelerBot.Models;

public class RatLevelerMessage<T> where T : class
{
    public string Message { get; set;} = null!;

    public T? Item { get; set; }
}