using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.CommandLine
{
    [TestClass]
    public class ArgumentReaderTest
    {
        // App.exe InputFile [-o OutputFile] [-d Depth] [-r] [-f]

        private readonly string[] ArgumentNames = { "-o", "-d" };
        private readonly string[] OptionNames = { "-r", "-f" };

        [TestMethod]
        public void GetParameterAndSwitch_CommonOrder_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-o", "file2.txt", "-f" };
            var reader = new ArgumentReader(arguments,
                ignoreCase: false, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.AreEqual("file2.txt", reader.GetArgument("-o"));
            Assert.IsNull(reader.GetArgument("-d"));
            Assert.IsFalse(reader.GetOption("-r"));
            Assert.IsTrue(reader.GetOption("-f"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MessOrder_WorksProperly()
        {
            string[] arguments = { "-o", "file2.txt", "file1.txt", "-f" };
            var reader = new ArgumentReader(arguments,
                ignoreCase: false, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.AreEqual("file2.txt", reader.GetArgument("-o"));
            Assert.IsNull(reader.GetArgument("-d"));
            Assert.IsFalse(reader.GetOption("-r"));
            Assert.IsTrue(reader.GetOption("-f"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_ExtraParam_Fails()
        {
            string[] arguments = { "-o", "file2.txt", "-a", "file1.txt", "-f" };
            var reader = new ArgumentReader(arguments,
                ignoreCase: false, this.ArgumentNames, this.OptionNames);
            Assert.AreNotEqual(reader.GetArgument(0), "file1.txt");
            Assert.AreEqual("file1.txt", reader.GetArgument(1));
            Assert.AreEqual("file2.txt", reader.GetArgument("-o"));
            Assert.IsNull(reader.GetArgument("-d"));
            Assert.IsFalse(reader.GetOption("-r"));
            Assert.IsTrue(reader.GetOption("-f"));
        }
    }
}
