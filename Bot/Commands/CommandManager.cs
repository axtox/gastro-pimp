using System;
using System.Linq;
using Telegram.Bot.Types;

namespace GastroPimp.Commands
{
    public class CommandManager
    {
        public Command ProcessCommand(Message commandMessage)
        {
            var rawCommand = commandMessage.EntityValues.First();
            return new Command
            {
                AttachedMessage = commandMessage.Text.Replace(rawCommand, ""),
                Name = ParseCommand(rawCommand)
            };
        }

        private CommandType ParseCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                return CommandType.none;

            var rawCommand = command.Split('@')[0].Replace("/", "");
            Enum.TryParse(rawCommand, out CommandType parsedCommand);

            return parsedCommand;
        }
    }
}
