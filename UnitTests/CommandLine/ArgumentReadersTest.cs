using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.CommandLine
{
    [TestClass]
    public class ArgumentReaderTests
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

    [TestClass]
    public class CmdArgumentReaderTests
    {
        // App.exe InputFile [/OUT:OutputFile] [/DEPTH:Depth] [/R] [/F]

        private static readonly string[] paramNames = { "/OUT", "/D" };
        private static readonly string[] switchNames = { "/R", "/F" };

        [TestMethod]
        public void GetParameterAndSwitch_CommonOrder_WorksProperly()
        {
            string[] arguments = { "file1.txt", "/out:file2.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.AreEqual("file2.txt", reader.GetParameter("/OUT"));
            Assert.IsNull(reader.GetParameter("/DEPTH"));
            Assert.IsFalse(reader.GetSwitch("/R"));
            Assert.IsTrue(reader.GetSwitch("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MessOrder_WorksProperly()
        {
            string[] arguments = { "/out:file2.txt", "file1.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.AreEqual("file2.txt", reader.GetParameter("/OUT"));
            Assert.IsNull(reader.GetParameter("/D"));
            Assert.IsFalse(reader.GetSwitch("/R"));
            Assert.IsTrue(reader.GetSwitch("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MultiParam_WorksProperly()
        {
            string[] arguments = { "file1.txt", "/out:file2.txt", "/f", "/OUT:file3.txt", "/Depth:0" };
            var reader = new CmdArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetParameter(0));
            Assert.IsTrue(reader.GetParameters("/OUT").SequenceEqual(new[] { "file2.txt", "file3.txt" }));
            Assert.AreEqual("0", reader.GetParameter("/DEPTH"));
            Assert.IsFalse(reader.GetSwitch("/R"));
            Assert.IsTrue(reader.GetSwitch("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_ExtraParam_Fails()
        {
            string[] arguments = { "/out:file2.txt", "/set", "file1.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, paramNames, switchNames);
            Assert.AreNotEqual(reader.GetParameter(0), "file1.txt");
            Assert.AreEqual("file1.txt", reader.GetParameter(1));
            Assert.AreEqual("file2.txt", reader.GetParameter("/OUT"));
            Assert.IsNull(reader.GetParameter("/DEPTH"));
            Assert.IsFalse(reader.GetSwitch("/R"));
            Assert.IsTrue(reader.GetSwitch("/F"));
        }
    }

    [TestClass]
    public class PowerShellArgumentReaderTests
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

    [TestClass]
    public class UnixShellArgumentReaderTests
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
