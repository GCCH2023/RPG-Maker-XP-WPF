using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP
{
    public class Command
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; }
        public Command()
        {
           
        }
        public Command(string name, bool enable = true)
        {
            this.Name = name;
            this.Enable = enable;
        }
    }
}
