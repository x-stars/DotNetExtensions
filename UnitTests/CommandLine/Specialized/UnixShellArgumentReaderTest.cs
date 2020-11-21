using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.CommandLine.Specialized
{
    [TestClass]
    public class UnixShellArgumentReaderTest
    {
        // App.exe InputFile [-O OutputFile] [-d,--depth Depth] [-f,--force] [-r,--recursive] [-h,--help]

        private static readonly string[] paramNames = { "-O", "-d", "--depth" };
        private static readonly string[] switchNames = { "-f", "--force", "-r", "--recursive", "-h", "--help" };

        [TestMethod]
        public void GetParameterAndSwitch_CommonOrder_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-O", "file2.txt", "-f" };
            var reader = new UnixShellArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.IsNull(reader.GetParameter("-o"));
            Assert.AreEqual("file2.txt", reader.GetParameter("-O"));
            Assert.IsNull(reader.GetParameter("-d,--depth"));
            Assert.IsTrue(reader.GetSwitch("-f,--force"));
            Assert.IsFalse(reader.GetSwitch("-r,--recursive"));
            Assert.IsFalse(reader.GetSwitch("-h,--help"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MessOrder_WorksProperly()
        {
            string[] arguments = { "-O", "file2.txt", "--recursive", "file1.txt", "--depth", "1", "-f" };
            var reader = new UnixShellArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.AreEqual("file2.txt", reader.GetParameter("-O"));
            Assert.AreEqual("1", reader.GetParameter("-d,--depth"));
            Assert.IsTrue(reader.GetSwitch("-f,--force"));
            Assert.IsTrue(reader.GetSwitch("-r,--recursive"));
            Assert.IsFalse(reader.GetSwitch("-h,--help"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MultiParam_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-O", "file2.txt", "-f", "-O", "file3.txt", "-d", "0" };
            var reader = new UnixShellArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.IsTrue(reader.GetParameters("-O").SequenceEqual(new[] { "file2.txt", "file3.txt" }));
            Assert.AreEqual("0", reader.GetParameter("-d,--depth"));
            Assert.IsTrue(reader.GetSwitch("-f,--force"));
            Assert.IsFalse(reader.GetSwitch("-r,--recursive"));
            Assert.IsFalse(reader.GetSwitch("-h,--help"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MultiSwitch_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-rf", "--help" };
            var reader = new UnixShellArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.IsNull(reader.GetParameter("-O"));
            Assert.IsNull(reader.GetParameter("-d,--depth"));
            Assert.IsTrue(reader.GetSwitch("-f,--force"));
            Assert.IsTrue(reader.GetSwitch("-r,--recursive"));
            Assert.IsTrue(reader.GetSwitch("-h,--help"));
        }
    }
}
