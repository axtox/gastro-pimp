using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gastro_pimp
{
    public class Command
    {
        public CommandManager.Commands Name { get; set; }
        public string AttachedMessage { get; set; }
    }
}
