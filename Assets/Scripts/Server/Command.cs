using System;
using System.Collections.Generic;

namespace Pickup.Server
{
    public class Command
    {
        public string description;
        public KeyValuePair<string, CommandArgumentType>[] parameters;
        public Action<string[]> runner;
    }

    public enum CommandArgumentType
    {
        String, Number, Boolean
    }
}