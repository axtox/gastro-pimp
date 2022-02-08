using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GastroPimp.Messaging
{
    public class MessagingBroker
    {
        private const long SlavesChatId = -720640119;

        private readonly TelegramBotClient _botClient;
        private readonly Random _randomSentence = new Random();

        public MessagingBroker(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        private string _instructions =
            "Дорогая кека! В честь особенного дня твоего двадцатипятилетия мы хотим, " +
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
            $"за тебя! Количество заказов не ограничено, суммарный лимит 10.000 рубенов.";
        public async Task SendInstructions(long chatId)
        {
            await SendMessage(_instructions, chatId);
        }

        private string[] _warmWordsForTheQueen = new[]
        {
            "Так атеншон, уроды, госпожа желает перекусить и побыстрее! Чья бесполезная жопа принесет хоть капельку пользы сегодня?",
            "РОТА ПОДЪЕМ БЫСТРО НАКОРМИЛИ НАШУ КОРОЛЕВУ ПОКА Я ВАС В СОСЕДНЕЙ ЧАТ ПО ЧИСТКЕ СОРТИРОВ НЕ ПРОДАЛ ЗА ТРИ КОПЕЙКИ",
            "Екатерина - наша Королева, а Королевы должны кушать по королевски, обслужите эту красотку по высшему разряду, гоблины",
            "Катюш, слышишь что то шебуршит? Это твои эльфы дерутся за право заказать тебе самое лучшее хрючево.",
            "Одну минутку, Величайшая! Мои упыри сейчас же закончат ковыряться в жопах и примутся выполнять Ваше поручение! Возможно они даже помоют руки!",
            "БЕЗДАРИ! Катя проголодалась и ей немедленно требуется ваше внимание! Не дай бог она окажется недовольна сервисом, на ноль будет умножен каждый!",
            "Мои рабы - лучшие рабы на континенте! Не переживай, Дорогая, хоть они и немного мерзкие, но работу свою знают. Все будет выполнено с особым старанием, дай нам минутку!",
            "Твоя сытость - наша работа. Один из свиноменеджеров по ублажению Королевских персон ответит с минуты на минуту."
        };
        public async Task SendWarmWordsToOurQueen()
        {
            await SendAngryNotificationToTheGroup(
                _warmWordsForTheQueen[_randomSentence.Next(0, _warmWordsForTheQueen.Length - 1)]);
        }

        private string[] _slaveAssignSentences = new[]
        {
            "Сегодня за работу возьмется @{0}, тебе повезло, это наименее гадкий холоп",
            "Обычно мы зовем его грязный Билли, но ты можешь звать его как хочешь, встречай @{0}",
            "МОЖЕШЬ ДЕЛАТЬ С ЭТИМ ЧУХАНОМ ЧТО ТЕБЕ ПОЖЕЛАЕТСЯ, К ТВОИМ РАСПОРЯЖЕНИЯМ @{0}",
            "@{0} сколько раз я тебе говорил не есть с пола, обслужи эту милую даму",
            "Екатерина, примите наши извинения за сложившуюся ситуацию, сегодня Вам не повезло, @{0} примет Ваш заказ",
            "@{0} ЕБАНЫЙ ШАШЛЫК, ПОРА БЫТЬ ХОТЬ ЧУТОЧКУ ПОЛЕЗНЫМ",
            "На этот раз тебе попалась лучшая из моих хрюшек - @{0}, спрашивай с этого свинтуса по полной, не стесняйся.",
            "ЧЕ МОЛЧИМ ОБСЛУГА? Важный человек проголодался! @{0} займись этим вопросом, не беси меня.",
            "Был у нас тут вонючий гоблин один, надеюсь хоть с этим справится. @{0} подъем, обслужи эту бейбу"
        };
        public async Task SendSlaveAssignedMessage()
        {
            await SendAngryNotificationToTheGroup(string.Format(
                _slaveAssignSentences[_randomSentence.Next(0, _slaveAssignSentences.Length - 1)], Slavery.CurrentSlave.Username));
        }

        private string[] _retireCommandNotAllowedSentences = new[]
        {
            "э ты кто такой, сча @{0} у руля, а тебе нельзя юзать эту команду",
            "ну не брат, ща @{0} все сделает, а ты иди лесом"
        };
        public async Task SendRetireCommandNotAllowedMessage(long chatId)
        {
            await SendMessage(string.Format(
                _retireCommandNotAllowedSentences[_randomSentence.Next(0, _retireCommandNotAllowedSentences.Length - 1)], Slavery.CurrentSlave.Username)
                , chatId);
        }

        private string[] _kushotsCommandNotAllowedSentences = new[]
        {
            "че ебанулся пиздец ты кто бля ахах",
            "ЗНАЙ СВОЕ МЕСТО НАСЕКОМОЕ, НЕ С ТОЙ ФАМИЛИЕЙ РОДИЛСЯ",
            "чел, не смеши всех вокруг, сам себе еду заказывай",
            "я сделаю вид, что не видел этого позорища",
            "бля иди корешки собери, какая тебе доставка"
        };
        public async Task SendKushotsCommandNotAllowedMessage(long chatId)
        {
            await SendMessage(_kushotsCommandNotAllowedSentences[_randomSentence.Next(0, _kushotsCommandNotAllowedSentences.Length - 1)], chatId);
        }

        private string[] _doneSentences = new[]
        {
            "Величайшая, заказ оформлен и скоро будет у Вас, надеюсь опыт общения с нашим поводырем не сильно Вам испортил аппетит. На счету осталось {0}р (ну примерно {1} тенге)",
            "Спасибо за использование наших авиалиний, температура за бортом идеальная для трапезы. Баланс {0}р",
            "Даже я уже утомился от этого зануды, заказ оформлен и слава богу Вам больше не придется потратить ни одной своей секунды.Приятного аппетита. На счету осталось {0}р (около {1} тенге)",
            "Заказ оформлен, но мне конечно немного стыдно за оказанную работу. Так я и знал, что рекрутер обманывает, когда подсовывал мне этого холопа и говорил, что у него IQ больше 45. Приятного аппетита, на балансе {0}р",
            "душнила хоспаде, хромой осел сделал бы все в 10 раз быстрее.Спасибо за Ваш заказ, Ваш баланс {0}р",
            "Заказ оформлен, но этот даун работает на меня не первый год. Жди что тебе привезут печеного голубя, а не твой заказ. На счету осталось {0}р (где-то {1} тенге)"
        };
        public async Task SendDoneMessage(int money)
        {
            await SendAngryNotificationToTheGroup(string.Format(
                _doneSentences[_randomSentence.Next(0, _doneSentences.Length - 1)], money, MoneyBank.RubleToTenge(money)));
        }


        public async Task SendAngryNotificationToTheGroup(string alterText = null)
        {
            await SendMessage(alterText, SlavesChatId);
        }

        private string[] _buttonNames = new[]
        {
            "ХОЛОП ТУТ Я",
            "Я РАБ",
            "ХОЧУ УБЛАЖИТЬ КОРОЛЕВУ",
            "Я ЛУЧШИЙ РАБ",
            "НАДЕЮСЬ ОНА МЕНЯ ЗАМЕТИТ",
            "ПРИНЕСУ ЕЙ ЕДЫ",
            "Я ГОТОВ СЛУЖИТЬ"
        };
        public async Task AskSlavesForFood(string order)
        {
            foreach (var slaveId in Slavery.SlaveList)
                await _botClient.SendTextMessageAsync(slaveId,
                    $"Королева трапезничать желает! Пора поднапрячься и заслужить её снисхождение! Возможно она оставила комментарий к заказу:\n{order}",
                    replyMarkup: new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithCallbackData(_buttonNames[_randomSentence.Next(0, _buttonNames.Length)])));
        }

        public async Task SendMessage(string message, long chatId)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: message
            );
        }
    }
}
