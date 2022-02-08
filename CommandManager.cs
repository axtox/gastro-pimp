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
            var command = new Command
            {
                AttachedMessage = commandMessage.Text.Replace(rawCommand, ""),
                Name = ParseCommand(rawCommand)
            };
            switch (command.Name)
            {
                case Commands.start:
                    SendInstructions(commandMessage.Chat.Id);
                    return command;
                default:
                    return command;
            }
        }

        private async Task SendInstructions(long chatId)
        {
            await _messagingBroker.SendMessage("Дорогая кека! В честь особенного дня твоего двадцатипятилетия мы хотим, " +
                                               "чтобы ты вообразила чувство настоящей заботы и любви друзей. Чувство, " +
                                               "когда понимаешь, что их бессонные ночи совместной работы, бесконечные " +
                                               "чаты, потоки мозговых штурмов, поиски и разочарования – все это пройдено" +
                                               "ради тебя. Вообразила? Теперь забывай нахер, нормальных друзей искать надо было!\n\n" +
                                               "Давай начистоту: мы хуевые прогеры, посредственные менеджеры, на троечку фотографы и " +
                                               "так себе прямо скажем продюсеры. Но главное, мы любим тебя, а ты – пожрать. " +
                                               "Поэтому встречай! Единственный в своём роде сервис доставки бесплатной еды – " +
                                               "PIIIIIIZZZZZZZAAAAAA HOLOP!\n\n" + "С этого дня ты назначаешься КОРОЛЕВОЙ этого неприятного " +
                                               "общества, в любой момент, когда ты проголодаешься, просто напиши /jrat и опиши, чего бы" +
                                               " тебе хотелось заказать и из какой доставки. Твои личные холопы выполнят всю грязную работу " +
                                               $"за тебя! Количество заказов не ограничено, суммарный лимит 10.000 рубенов.", chatId);
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
