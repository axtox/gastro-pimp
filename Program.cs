using GastroPimp.Commands;
using GastroPimp.Messaging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GastroPimp
{
    class Program
    {
        private static TelegramBotClient _botClient;
        private static MessagingBroker _messagingBroker;
        private static CommandManager _commandManager;
        private static Slavery _slavery;

        static async Task Main(string[] args)
        {
            _botClient = new TelegramBotClient("2040809860:AAGVkRBZI7St9r9NufLdon3mWwnI2lbKWi8");
            var me = await _botClient.GetMeAsync();
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            _messagingBroker = new MessagingBroker(_botClient);
            _commandManager = new CommandManager();
            _slavery = new Slavery(_messagingBroker);

            using var cts = new CancellationTokenSource();
            
            _botClient.StartReceiving(
                new DefaultUpdateHandler(OnSuccess, OnError),
                cts.Token);
            
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            
            cts.Cancel();
        }

        static Task OnError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception.Message;

            Console.WriteLine(errorMessage);

            return Task.CompletedTask;
        }

        static async Task OnSuccess(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _slavery.AssignSlave(update.CallbackQuery);
                return;
            }
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;
            
            var chatId = update.Message.Chat.Id;
            var isCommand = update.Message.Entities != null &&
                            update.Message.Entities.FirstOrDefault()?.Type == MessageEntityType.BotCommand;
            if (!isCommand)
                return;

            var command = _commandManager.ProcessCommand(update.Message);
            switch (command.Name)
            {
                case CommandType.done:
                    await _slavery.RetireSlave(update.Message, command.AttachedMessage);
                    break;
                case CommandType.jrat:
                    await _slavery.QueenWantsSomeFood(command.AttachedMessage, update.Message.From.Id, chatId);
                    break;
                case CommandType.money:
                    if (int.TryParse(command.AttachedMessage, out var reduceBy))
                        await _slavery.ShowHowMuchMoneyLeft(chatId, reduceBy);
                    else
                        await _slavery.ShowHowMuchMoneyLeft(chatId);
                    break;
                case CommandType.start:
                    await _messagingBroker.SendInstructions(chatId);
                    break;
            }
        }
    }
}
