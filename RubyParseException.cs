using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    class RubyParseException : Exception
    {
        public static uint BaseAddress{get;set;}
        public RubyParseException(uint address, string message)
            : base(string.Format("偏移量 {0}, {1}", (address - BaseAddress), message))
        {

        }
    }
}
