namespace _MAIN.Scripts.Core.Commands.Database
{
    public abstract class CmdDatabaseExtension
    {
        public static void Extend(CommandsDatabase database) { }
        protected static CommandParameters ConvertDataToParameters(string[] data, int startingIndex = 0) => new CommandParameters(data, startingIndex);
    }
}
