namespace RatLevelerBot.Helpers;

public static class BotCommandsMapper
	{
    public static BotCommands Map(string? command)
        => command switch
        {
            "/start" => BotCommands.Start,
            "/level" => BotCommands.Level,
            "/newrat" => BotCommands.NewRat,
            "/resetlevel" => BotCommands.ResetLevel,
            "/setlevel" => BotCommands.SetLevel,
            // "🐀" => BotCommands.Unknown,
            _ => BotCommands.Unknown
        };
}

public enum BotCommands
{
    Start,
    Level,
    SetLevel,
    NewRat,
    ResetLevel,
    Unknown
}

