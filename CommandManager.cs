using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace gastro_pimp
{
    public class CommandManager
    {
        private readonly MessagingBroker _messagingBroker;

        public enum Commands
        {
            none,
            start,
            done,
            jrat,
            money
        }

        public CommandManager(MessagingBroker messagingBroker)
        {
            _messagingBroker = messagingBroker;
        }

        public Command ProcessCommand(Message commandMessage)
        {
            var rawCommand = commandMessage.EntityValues.First();
            return new Command
            {
                AttachedMessage = commandMessage.Text.Replace(rawCommand, ""),
                Name = ParseCommand(rawCommand)
            };
        }

        private Commands ParseCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                return Commands.none;

            var rawCommand = command.Split('@')[0].Replace("/", "");
            Enum.TryParse(rawCommand, out Commands parsedCommand);

            return parsedCommand;
        }
    }
}
