using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MJU23v_D10_inl_sveng
{
    public delegate void CommandAction(string[] args);

    internal class Command
    {
        private string Cmd { get; }
        private string[] Aliases { get; }
        private CommandAction CmdAction { get; }

        public Command(string command, string[] alias, CommandAction action)
        {
            this.Cmd = command;
            this.Aliases = alias;
            this.CmdAction = action;
        }

        public string GetCommand()
        {
            return this.Cmd;
        }

        public string[] GetAlias()
        {
            return this.Aliases;
        }

        public void ExecuteCommand(string[] args)
        {
            this.CmdAction.Invoke(args);
        }
    }
}