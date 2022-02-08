using System;
using System.Linq;

namespace GastroPimp
{
    public class MoneyBank
    {
        private const int MinimalAmountOfMoney = 100;
        private int _currentAmount;

        public MoneyBank(int startAmount)
        {
            _currentAmount = startAmount;
        }

        public int ParseMoneyFromString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            var digits = input.SkipWhile(c => !Char.IsDigit(c))
                .TakeWhile(Char.IsDigit)
                .ToArray();

            var str = new string(digits);
            if(int.TryParse(str, out var result))
                return result;
            return 0;
        }

        public bool Decrease(int by)
        {
            _currentAmount -= by;
            return _currentAmount > 0;
        }

        public int GetCurrentAmount()
        {
            return _currentAmount;
        }

        public static int RubleToTenge(int rubles)
        {
            return (int)(rubles * 6.03);
        }

        public bool StillHaveSomeMoney()
        {
            return _currentAmount > MinimalAmountOfMoney;
        }
    }
}
