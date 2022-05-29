using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.CommandLine.Specialized
{
    [TestClass]
    public class UnixShellArgumentReaderTest
    {
        // App.exe InputFile [-O OutputFile] [-d,--depth Depth] [-f,--force] [-r,--recursive] [-h,--help]

        private readonly string[] ArgumentNames = { "-O", "-d", "--depth" };
        private readonly string[] OptionNames = { "-f", "--force", "-r", "--recursive", "-h", "--help" };

        [TestMethod]
        public void GetParameterAndSwitch_CommonOrder_WorksProperly()
        {
            var arguments = new[] { "file1.txt", "-O", "file2.txt", "-f" };
            var reader = new UnixShellArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.IsNull(reader.GetArgument("-o"));
            Assert.AreEqual("file2.txt", reader.GetArgument("-O"));
            Assert.IsNull(reader.GetArgument("-d,--depth"));
            Assert.IsTrue(reader.GetOption("-f,--force"));
            Assert.IsFalse(reader.GetOption("-r,--recursive"));
            Assert.IsFalse(reader.GetOption("-h,--help"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MessOrder_WorksProperly()
        {
            var arguments = new[] { "-O", "file2.txt", "--recursive", "file1.txt", "--depth", "1", "-f" };
            var reader = new UnixShellArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.AreEqual("file2.txt", reader.GetArgument("-O"));
            Assert.AreEqual("1", reader.GetArgument("-d,--depth"));
            Assert.IsTrue(reader.GetOption("-f,--force"));
            Assert.IsTrue(reader.GetOption("-r,--recursive"));
            Assert.IsFalse(reader.GetOption("-h,--help"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MultiParam_WorksProperly()
        {
            var arguments = new[] { "file1.txt", "-O", "file2.txt", "-f", "-O", "file3.txt", "-d", "0" };
            var reader = new UnixShellArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.IsTrue(reader.GetArguments("-O").SequenceEqual(new[] { "file2.txt", "file3.txt" }));
            Assert.AreEqual("0", reader.GetArgument("-d,--depth"));
            Assert.IsTrue(reader.GetOption("-f,--force"));
            Assert.IsFalse(reader.GetOption("-r,--recursive"));
            Assert.IsFalse(reader.GetOption("-h,--help"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MultiSwitch_WorksProperly()
        {
            var arguments = new[] { "file1.txt", "-rf", "--help" };
            var reader = new UnixShellArgumentReader(arguments, this.ArgumentNames, this.OptionNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.IsNull(reader.GetArgument("-O"));
            Assert.IsNull(reader.GetArgument("-d,--depth"));
            Assert.IsTrue(reader.GetOption("-f,--force"));
            Assert.IsTrue(reader.GetOption("-r,--recursive"));
            Assert.IsTrue(reader.GetOption("-h,--help"));
        }
    }
}
