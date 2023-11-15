namespace RatLevelerBot.Helpers;

public static class BotCommandsMapper
{
    public static BotCommands Map(string? command)
        => command switch
        {
            Consts.StartComand => BotCommands.Start,
            Consts.ChatLevelsComand => BotCommands.ChatLevels,
            Consts.LevelCommand  => BotCommands.Level,
            Consts.NewRatCommand => BotCommands.NewRat,
            Consts.ResetLevelCommand => BotCommands.ResetLevel,
            Consts.SetLevelCommand => BotCommands.SetLevel,
            Consts.IncreaseExpCommand => BotCommands.IncreaseExp,
            _ => BotCommands.Unknown
        };
    
    public static List<string> BotCommandsList = new List<string>(){
        Consts.StartComand,
        Consts.ChatLevelsComand,
        Consts.LevelCommand,
        Consts.NewRatCommand,
        Consts.ResetLevelCommand,
        Consts.SetLevelCommand,
        Consts.IncreaseExpCommand
    };
}

public enum BotCommands
{
    Start,
    ChatLevels,
    Level,
    SetLevel,
    NewRat,
    ResetLevel,
    IncreaseExp,
    Unknown
}
