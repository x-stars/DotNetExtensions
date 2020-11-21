using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.CommandLine.Specialized
{
    [TestClass]
    public class PowerShellArgumentReaderTest
    {
        // App.exe -Path <String> [-OutFile <String>] [-Depth Int32] [-Recursive] [-Force]

        private static readonly string[] paramNames = { "-Path", "-OutFile", "-Depth" };
        private static readonly string[] switchNames = { "-Recursive", "-Force" };

        [TestMethod]
        public void GetParameterAndSwitch_AllNamed_WorksProperly()
        {
            string[] arguments = { "-Path", "file1.txt", "-OutFile", "file2.txt", "-Force" };
            var reader = new PowerShellArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter("-Path"));
            Assert.AreEqual("file2.txt", reader.GetParameter("-OutFile"));
            Assert.IsNull(reader.GetParameter("-Depth"));
            Assert.IsFalse(reader.GetSwitch("-Recursive"));
            Assert.IsTrue(reader.GetSwitch("-Force"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_PartialNamed_WorksProperly()
        {
            string[] arguments = { "-OutFile", "file2.txt", "file1.txt", "1", "-Recursive" };
            var reader = new PowerShellArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter("-Path"));
            Assert.AreEqual("file2.txt", reader.GetParameter("-OutFile"));
            Assert.AreEqual("1", reader.GetParameter("-Depth"));
            Assert.IsTrue(reader.GetSwitch("-Recursive"));
            Assert.IsFalse(reader.GetSwitch("-Force"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_ExtraParam_Fails()
        {
            string[] arguments = { "-OutFile", "file2.txt", "-Mode", "777", "file1.txt", "-Recursive" };
            var reader = new PowerShellArgumentReader(arguments, paramNames, switchNames);
            Assert.AreNotEqual(reader.GetParameter("-Path"), "file1.txt");
            Assert.AreEqual("-Mode", reader.GetParameter("-Path"));
            Assert.AreEqual("file2.txt", reader.GetParameter("-OutFile"));
            Assert.AreEqual("777", reader.GetParameter("-Depth"));
            Assert.AreEqual("777", reader.GetParameter("-Mode"));
            Assert.IsTrue(reader.GetSwitch("-Recursive"));
            Assert.IsFalse(reader.GetSwitch("-Force"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_IndexedParam_ThrowsException()
        {
            string[] arguments = { "-OutFile", "file2.txt", "file1.txt", "1", "-Recursive" };
            var reader = new PowerShellArgumentReader(arguments, paramNames, switchNames);
            Assert.ThrowsException<NotSupportedException>(() => reader.GetParameter(0));
            Assert.ThrowsException<NotSupportedException>(() => reader.GetParameter(1));
        }
    }
}
