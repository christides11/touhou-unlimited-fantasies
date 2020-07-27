using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TUF.Console
{
    public class ConsoleCommand
    {
        protected ConsoleInputProcessor processor;

        public virtual ConsoleCommand Initialize(ConsoleInputProcessor processor)
        {
            this.processor = processor;
            return this;
        }


        public async virtual Task<string> Do()
        {
            return $"Command {this.GetType().Name} was ran.";
        }
    }
}