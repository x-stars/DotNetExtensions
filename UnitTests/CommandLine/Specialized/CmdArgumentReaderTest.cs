using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.CommandLine.Specialized
{
    [TestClass]
    public class CmdArgumentReaderTest
    {
        // App.exe InputFile [/OUT:OutputFile] [/DEPTH:Depth] [/R] [/F]

        private readonly string[] ArgumentNames = { "/OUT", "/DEPTH" };
        private readonly string[] OptionNames = { "/R", "/F" };

        [TestMethod]
        public void GetParameterAndSwitch_CommonOrder_WorksProperly()
        {
            var arguments = new[] { "file1.txt", "/out:file2.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.AreEqual("file2.txt", reader.GetArgument("/OUT"));
            Assert.IsNull(reader.GetArgument("/DEPTH"));
            Assert.IsFalse(reader.GetOption("/R"));
            Assert.IsTrue(reader.GetOption("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MessOrder_WorksProperly()
        {
            var arguments = new[] { "/out:file2.txt", "file1.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.AreEqual("file2.txt", reader.GetArgument("/OUT"));
            Assert.IsNull(reader.GetArgument("/D"));
            Assert.IsFalse(reader.GetOption("/R"));
            Assert.IsTrue(reader.GetOption("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MultiParam_WorksProperly()
        {
            var arguments = new[] { "file1.txt", "/out:file2.txt", "/f", "/OUT:file3.txt", "/Depth:0" };
            var reader = new CmdArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.IsTrue(reader.GetArguments("/OUT").SequenceEqual(new[] { "file2.txt", "file3.txt" }));
            Assert.AreEqual("0", reader.GetArgument("/DEPTH"));
            Assert.IsFalse(reader.GetOption("/R"));
            Assert.IsTrue(reader.GetOption("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_ExtraParam_Fails()
        {
            var arguments = new[] { "/out:file2.txt", "/set", "file1.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreNotEqual(reader.GetArgument(0), "file1.txt");
            Assert.AreEqual("file1.txt", reader.GetArgument(1));
            Assert.AreEqual("file2.txt", reader.GetArgument("/OUT"));
            Assert.IsNull(reader.GetArgument("/DEPTH"));
            Assert.IsFalse(reader.GetOption("/R"));
            Assert.IsTrue(reader.GetOption("/F"));
        }
    }
}
