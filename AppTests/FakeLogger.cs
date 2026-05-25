using System.Collections.Generic;
using App.Data;

namespace AppTests
{
    internal class FakeLogger : ILogger
    {
        public List<string> Messages { get; }
            = new List<string>();

        public void Log(string message)
        {
            Messages.Add(message);
        }
    }
}