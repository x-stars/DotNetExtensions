using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Windows.Input
{
    [TestClass]
    public class CommandBaseTest
    {
        [TestMethod]
        public void DelegateCommand_WriteToConsole_WorksProperly()
        {
            var canExecute = true;
            var command = default(DelegateCommand);
            command = new DelegateCommand(
                _ =>
                {
                    if (canExecute)
                    {
                        canExecute = false;
                        command!.NotifyCanExecuteChanged();
                        Console.WriteLine(DateTime.Now);
                        canExecute = true;
                        command!.NotifyCanExecuteChanged();
                    }
                },
                _ => canExecute);
            command.CanExecuteChanged += (sender, e) => Console.WriteLine(
                $"{nameof(command.CanExecuteChanged)}: {command.CanExecute()}");
            command.Execute();
        }
    }
}
