using gastro_pimp.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace gastro_pimp
{
    public class Slavery
    {
        private readonly MessagingBroker _messagingBroker;
        private readonly MoneyBank _moneyBank;

        //list of slaves
        public static List<long> SlaveList = new(8)
        {
            //kefx
            325533383,
            //axtox
            21381613,
            //irina
            843236890,
            //vania
            412156112,
            //mashka
            109681764,
            //nastya
            106491927
        };

        public static User CurrentSlave;
        private string _currentOrder;

        public Slavery(MessagingBroker messagingBroker)
        {
            _messagingBroker = messagingBroker;
            _moneyBank = new MoneyBank(10000);
        }

        public bool CheckIfUserIsSlave(long userId)
        {
            return SlaveList.Any(slaveId => slaveId == userId);
        }

        public async Task<int> ShowHowMuchMoneyLeft(long chatId, int reduceBy = 0)
        {
            if (reduceBy != 0)
                _moneyBank.Decrease(reduceBy);
            
            var money = _moneyBank.GetCurrentAmount();
            await _messagingBroker.SendMessage($"Баланс: {money}р ({MoneyBank.RubleToTenge(money)} ₸)", chatId);
            return money;
        }

        public async Task AssignSlave(CallbackQuery callback)
        {
            if (CurrentSlave != null)
            {
                await _messagingBroker.SendMessage($"Да все иди нахуй. Уже опоздал. Ща @{CurrentSlave.Username} все сделает", callback.From.Id);
                return;
            }

            if (_moneyBank.GetCurrentAmount() <= 0)
            {
                await _messagingBroker.SendMessage($"все, деняк нет.", callback.From.Id);
                return;
            }

            if(_currentOrder == null)
            {
                await _messagingBroker.SendMessage($"Послушай, у королевы нет сейчас заказов. Пойди займись чем-нибудь полезным наконец, что-ли.\nБоже бля как вы вообще живете я не понимаю реально", callback.From.Id);
                return;
            }

            await _messagingBroker.SendMessage($"Всё, збс! Давай бегом, чертила", callback.From.Id);

            CurrentSlave = callback.From;

            await _messagingBroker.SendSlaveAssignedMessage();
        }

        public async Task RetireSlave(Message whosAsking, string attachedMessage)
        {
            if (CurrentSlave == null)
            {
                await _messagingBroker.SendMessage($"ща никого нет назнеаченного иди отсюда", whosAsking.Chat.Id);
                return;
            }

            if (whosAsking.From.Id != CurrentSlave.Id)
            {
                await _messagingBroker.SendRetireCommandNotAllowedMessage(whosAsking.Chat.Id);
                return;
            }

            var money = _moneyBank.ParseMoneyFromString(attachedMessage);
            if (money == 0)
            {
                await _messagingBroker.SendMessage("Бля ну ты просто ебанат натрия. Сумму укажи", whosAsking.Chat.Id);
                return;
            }

            _moneyBank.Decrease(money);

            CurrentSlave = null;
            _currentOrder = null;

            await _messagingBroker.SendDoneMessage(_moneyBank.GetCurrentAmount());
        }

        public async Task QueenWantsSomeFood(string herMajestyMessage, long herMajestyId, long chatId)
        {
            if (CheckIfUserIsSlave(herMajestyId))
            {
                await _messagingBroker.SendKushotsCommandNotAllowedMessage(chatId);
                return;
            }
            if(CurrentSlave != null)
            {
                await _messagingBroker.SendMessage($"Ваще восхищенство! Ваш раб уже назначен - это @{CurrentSlave.Username}", chatId);
                return;
            }

            if (_moneyBank.GetCurrentAmount() <= 0)
            {
                await _messagingBroker.SendMessage($"Госпожа, у вас кончились деньги, теперь вы холоп как мы. Ваш баланс {_moneyBank.GetCurrentAmount()}р, это в тенге будет {MoneyBank.RubleToTenge(_moneyBank.GetCurrentAmount())}", chatId);
                return;
            }

            if (_currentOrder != null)
            {
                await _messagingBroker.SendWarmWordsToOurQueen();
                return;
            }

            // слушгаюсь и повинуюсь, отправил ублюдкам сообщеньку
            await _messagingBroker.SendWarmWordsToOurQueen();
            // send her message
            if(!string.IsNullOrWhiteSpace(herMajestyMessage))
                await _messagingBroker.SendAngryNotificationToTheGroup("ЗАКАЗ: " + herMajestyMessage);

            await _messagingBroker.AskSlavesForFood(herMajestyMessage);

            _currentOrder = herMajestyMessage;
        }
    }
}
