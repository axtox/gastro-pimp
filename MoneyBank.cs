using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gastro_pimp
{
    public class MoneyBank
    {
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
    }
}
