using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.CommandLine.Specialized
{
    [TestClass]
    public class PowerShellArgumentReaderTest
    {
        // App.exe -Path <String> [-OutFile <String>] [-Depth Int32] [-Recursive] [-Force]

        private readonly string[] ArgumentNames = { "-Path", "-OutFile", "-Depth" };
        private readonly string[] OptionNames = { "-Recursive", "-Force" };

        [TestMethod]
        public void GetParameterAndSwitch_AllNamed_WorksProperly()
        {
            var arguments = new[] { "-Path", "file1.txt", "-OutFile", "file2.txt", "-Force" };
            var reader = new PowerShellArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument("-Path"));
            Assert.AreEqual("file2.txt", reader.GetArgument("-OutFile"));
            Assert.IsNull(reader.GetArgument("-Depth"));
            Assert.IsFalse(reader.GetOption("-Recursive"));
            Assert.IsTrue(reader.GetOption("-Force"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_PartialNamed_WorksProperly()
        {
            var arguments = new[] { "-OutFile", "file2.txt", "file1.txt", "1", "-Recursive" };
            var reader = new PowerShellArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument("-Path"));
            Assert.AreEqual("file2.txt", reader.GetArgument("-OutFile"));
            Assert.AreEqual("1", reader.GetArgument("-Depth"));
            Assert.IsTrue(reader.GetOption("-Recursive"));
            Assert.IsFalse(reader.GetOption("-Force"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_ExtraParam_Fails()
        {
            var arguments = new[] { "-OutFile", "file2.txt", "-Mode", "777", "file1.txt", "-Recursive" };
            var reader = new PowerShellArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreNotEqual(reader.GetArgument("-Path"), "file1.txt");
            Assert.AreEqual("-Mode", reader.GetArgument("-Path"));
            Assert.AreEqual("file2.txt", reader.GetArgument("-OutFile"));
            Assert.AreEqual("777", reader.GetArgument("-Depth"));
            Assert.AreEqual("777", reader.GetArgument("-Mode"));
            Assert.IsTrue(reader.GetOption("-Recursive"));
            Assert.IsFalse(reader.GetOption("-Force"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_IndexedParam_ThrowsException()
        {
            var arguments = new[] { "-OutFile", "file2.txt", "file1.txt", "1", "-Recursive" };
            var reader = new PowerShellArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.ThrowsException<NotSupportedException>(() => reader.GetArgument(0));
            Assert.ThrowsException<NotSupportedException>(() => reader.GetArgument(1));
        }
    }
}
