using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.CommandLine
{
    [TestClass]
    public class ArgumentReaderTest
    {
        // App.exe InputFile [-o OutputFile] [-d Depth] [-r] [-f]

        private static readonly string[] paramNames = { "-o", "-d" };
        private static readonly string[] switchNames = { "-r", "-f" };

        [TestMethod]
        public void GetParameterAndSwitch_CommonOrder_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-o", "file2.txt", "-f" };
            var reader = new ArgumentReader(arguments, false, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.AreEqual("file2.txt", reader.GetParameter("-o"));
            Assert.IsNull(reader.GetParameter("-d"));
            Assert.IsFalse(reader.GetSwitch("-r"));
            Assert.IsTrue(reader.GetSwitch("-f"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MessOrder_WorksProperly()
        {
            string[] arguments = { "-o", "file2.txt", "file1.txt", "-f" };
            var reader = new ArgumentReader(arguments, false, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.AreEqual("file2.txt", reader.GetParameter("-o"));
            Assert.IsNull(reader.GetParameter("-d"));
            Assert.IsFalse(reader.GetSwitch("-r"));
            Assert.IsTrue(reader.GetSwitch("-f"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_ExtraParam_Fails()
        {
            string[] arguments = { "-o", "file2.txt", "-a", "file1.txt", "-f" };
            var reader = new ArgumentReader(arguments, false, paramNames, switchNames);
            Assert.AreNotEqual(reader.GetParameter(0), "file1.txt");
            Assert.AreEqual("file1.txt", reader.GetParameter(1));
            Assert.AreEqual("file2.txt", reader.GetParameter("-o"));
            Assert.IsNull(reader.GetParameter("-d"));
            Assert.IsFalse(reader.GetSwitch("-r"));
            Assert.IsTrue(reader.GetSwitch("-f"));
        }
    }
}
