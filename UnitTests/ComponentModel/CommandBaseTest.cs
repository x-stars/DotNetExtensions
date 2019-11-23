using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class CommandBaseTest
    {
        [TestMethod]
        public void DelegateCommand_WriteToConsole_WorksProperly()
        {
            var command = default(DelegateCommand);
            command = new DelegateCommand(
                () =>
                {
                    var token = command.GetCanExecuteToken();
                    token.IsExecutable = false;
                    Console.WriteLine(DateTime.Now);
                    token.IsExecutable = true;
                });
            command.CanExecuteChanged += (sender, e) => Console.WriteLine(
                $"{nameof(command.CanExecuteChanged)}: {command.CanExecute()}");
            command.Execute();
        }
    }
}
