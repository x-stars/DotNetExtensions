using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS
{
    [TestClass]
    public class ParamReaderTests
    {
        // App.exe InputFile [-o OutputFile] [-d Depth] [-r] [-f]

        private static readonly string[] paramNames = { "-o", "-d" };
        private static readonly string[] switchNames = { "-r", "-f" };

        [TestMethod]
        public void GetParamAndSwitch_CommonOrder_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-o", "file2.txt", "-f" };
            var reader = new ParamReader(arguments, false, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.AreEqual(reader.GetParam("-o"), "file2.txt");
            Assert.IsNull(reader.GetParam("-d"));
            Assert.IsFalse(reader.GetSwitch("-r"));
            Assert.IsTrue(reader.GetSwitch("-f"));
        }

        [TestMethod]
        public void GetParamAndSwitch_MessOrder_WorksProperly()
        {
            string[] arguments = { "-o", "file2.txt", "file1.txt", "-f" };
            var reader = new ParamReader(arguments, false, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.AreEqual(reader.GetParam("-o"), "file2.txt");
            Assert.IsNull(reader.GetParam("-d"));
            Assert.IsFalse(reader.GetSwitch("-r"));
            Assert.IsTrue(reader.GetSwitch("-f"));
        }

        [TestMethod]
        public void GetParamAndSwitch_ExtraParam_Fails()
        {
            string[] arguments = { "-o", "file2.txt", "-a", "file1.txt", "-f" };
            var reader = new ParamReader(arguments, false, paramNames, switchNames);
            Assert.AreNotEqual(reader.GetParam(0), "file1.txt");
            Assert.AreEqual(reader.GetParam(1), "file1.txt");
            Assert.AreEqual(reader.GetParam("-o"), "file2.txt");
            Assert.IsNull(reader.GetParam("-d"));
            Assert.IsFalse(reader.GetSwitch("-r"));
            Assert.IsTrue(reader.GetSwitch("-f"));
        }
    }

    [TestClass]
    public class CmdParamReaderTests
    {
        // App.exe InputFile [/OUT:OutputFile] [/DEPTH:Depth] [/R] [/F]

        private static readonly string[] paramNames = { "/OUT", "/D" };
        private static readonly string[] switchNames = { "/R", "/F" };

        [TestMethod]
        public void GetParamAndSwitch_CommonOrder_WorksProperly()
        {
            string[] arguments = { "file1.txt", "/out:file2.txt", "/f" };
            var reader = new CmdParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.AreEqual(reader.GetParam("/OUT"), "file2.txt");
            Assert.IsNull(reader.GetParam("/DEPTH"));
            Assert.IsFalse(reader.GetSwitch("/R"));
            Assert.IsTrue(reader.GetSwitch("/F"));
        }

        [TestMethod]
        public void GetParamAndSwitch_MessOrder_WorksProperly()
        {
            string[] arguments = { "/out:file2.txt", "file1.txt", "/f" };
            var reader = new CmdParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.AreEqual(reader.GetParam("/OUT"), "file2.txt");
            Assert.IsNull(reader.GetParam("/D"));
            Assert.IsFalse(reader.GetSwitch("/R"));
            Assert.IsTrue(reader.GetSwitch("/F"));
        }

        [TestMethod]
        public void GetParamAndSwitch_MultiParam_WorksProperly()
        {
            string[] arguments = { "file1.txt", "/out:file2.txt", "/f", "/OUT:file3.txt", "/Depth:0" };
            var reader = new CmdParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.IsTrue(reader.GetParams("/OUT").SequenceEqual(new[] { "file2.txt", "file3.txt" }));
            Assert.AreEqual(reader.GetParam("/DEPTH"), "0");
            Assert.IsFalse(reader.GetSwitch("/R"));
            Assert.IsTrue(reader.GetSwitch("/F"));
        }

        [TestMethod]
        public void GetParamAndSwitch_ExtraParam_Fails()
        {
            string[] arguments = { "/out:file2.txt", "/set", "file1.txt", "/f" };
            var reader = new CmdParamReader(arguments, paramNames, switchNames);
            Assert.AreNotEqual(reader.GetParam(0), "file1.txt");
            Assert.AreEqual(reader.GetParam(1), "file1.txt");
            Assert.AreEqual(reader.GetParam("/OUT"), "file2.txt");
            Assert.IsNull(reader.GetParam("/DEPTH"));
            Assert.IsFalse(reader.GetSwitch("/R"));
            Assert.IsTrue(reader.GetSwitch("/F"));
        }
    }

    [TestClass]
    public class PowerShellParamReaderTests
    {
        // App.exe -Path <String> [-OutFile <String>] [-Depth Int32] [-Recursive] [-Force]

        private static readonly string[] paramNames = { "-Path", "-OutFile", "-Depth" };
        private static readonly string[] switchNames = { "-Recursive", "-Force" };

        [TestMethod]
        public void GetParamAndSwitch_AllNamed_WorksProperly()
        {
            string[] arguments = { "-Path", "file1.txt", "-OutFile", "file2.txt", "-Force" };
            var reader = new PowerShellParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam("-Path"), "file1.txt");
            Assert.AreEqual(reader.GetParam("-OutFile"), "file2.txt");
            Assert.IsNull(reader.GetParam("-Depth"));
            Assert.IsFalse(reader.GetSwitch("-Recursive"));
            Assert.IsTrue(reader.GetSwitch("-Force"));
        }

        [TestMethod]
        public void GetParamAndSwitch_PartialNamed_WorksProperly()
        {
            string[] arguments = { "-OutFile", "file2.txt", "file1.txt", "1", "-Recursive" };
            var reader = new PowerShellParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam("-Path"), "file1.txt");
            Assert.AreEqual(reader.GetParam("-OutFile"), "file2.txt");
            Assert.AreEqual(reader.GetParam("-Depth"), "1");
            Assert.IsTrue(reader.GetSwitch("-Recursive"));
            Assert.IsFalse(reader.GetSwitch("-Force"));
        }

        [TestMethod]
        public void GetParamAndSwitch_ExtraParam_Fails()
        {
            string[] arguments = { "-OutFile", "file2.txt", "-Mode", "777", "file1.txt", "-Recursive" };
            var reader = new PowerShellParamReader(arguments, paramNames, switchNames);
            Assert.AreNotEqual(reader.GetParam("-Path"), "file1.txt");
            Assert.AreEqual(reader.GetParam("-Path"), "-Mode");
            Assert.AreEqual(reader.GetParam("-OutFile"), "file2.txt");
            Assert.AreEqual(reader.GetParam("-Depth"), "777");
            Assert.AreEqual(reader.GetParam("-Mode"), "777");
            Assert.IsTrue(reader.GetSwitch("-Recursive"));
            Assert.IsFalse(reader.GetSwitch("-Force"));
        }

        [TestMethod]
        public void GetParamAndSwitch_IndexedParam_ThrowsException()
        {
            string[] arguments = { "-OutFile", "file2.txt", "file1.txt", "1", "-Recursive" };
            var reader = new PowerShellParamReader(arguments, paramNames, switchNames);
            Assert.ThrowsException<NotSupportedException>(() => reader.GetParam(0));
            Assert.ThrowsException<NotSupportedException>(() => reader.GetParam(1));
        }
    }

    [TestClass]
    public class UnixShellParamReaderTests
    {
        // App.exe InputFile [-O OutputFile] [-d,--depth Depth] [-f,--force] [-r,--recursive] [-h,--help]

        private static readonly string[] paramNames = { "-O", "-d", "--depth" };
        private static readonly string[] switchNames = { "-f", "--force", "-r", "--recursive", "-h", "--help" };

        [TestMethod]
        public void GetParamAndSwitch_CommonOrder_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-O", "file2.txt", "-f" };
            var reader = new UnixShellParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.IsNull(reader.GetParam("-o"));
            Assert.AreEqual(reader.GetParam("-O"), "file2.txt");
            Assert.IsNull(reader.GetParam("-d,--depth"));
            Assert.IsTrue(reader.GetSwitch("-f,--force"));
            Assert.IsFalse(reader.GetSwitch("-r,--recursive"));
            Assert.IsFalse(reader.GetSwitch("-h,--help"));
        }

        [TestMethod]
        public void GetParamAndSwitch_MessOrder_WorksProperly()
        {
            string[] arguments = { "-O", "file2.txt", "--recursive", "file1.txt", "--depth", "1", "-f" };
            var reader = new UnixShellParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.AreEqual(reader.GetParam("-O"), "file2.txt");
            Assert.AreEqual(reader.GetParam("-d,--depth"), "1");
            Assert.IsTrue(reader.GetSwitch("-f,--force"));
            Assert.IsTrue(reader.GetSwitch("-r,--recursive"));
            Assert.IsFalse(reader.GetSwitch("-h,--help"));
        }

        [TestMethod]
        public void GetParamAndSwitch_MultiParam_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-O", "file2.txt", "-f", "-O", "file3.txt", "-d", "0" };
            var reader = new UnixShellParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.IsTrue(reader.GetParams("-O").SequenceEqual(new[] { "file2.txt", "file3.txt" }));
            Assert.AreEqual(reader.GetParam("-d,--depth"), "0");
            Assert.IsTrue(reader.GetSwitch("-f,--force"));
            Assert.IsFalse(reader.GetSwitch("-r,--recursive"));
            Assert.IsFalse(reader.GetSwitch("-h,--help"));
        }

        [TestMethod]
        public void GetParamAndSwitch_MultiSwitch_WorksProperly()
        {
            string[] arguments = { "file1.txt", "-rf", "--help" };
            var reader = new UnixShellParamReader(arguments, paramNames, switchNames);
            Assert.AreEqual(reader.GetParam(0), "file1.txt");
            Assert.IsNull(reader.GetParam("-O"));
            Assert.IsNull(reader.GetParam("-d,--depth"));
            Assert.IsTrue(reader.GetSwitch("-f,--force"));
            Assert.IsTrue(reader.GetSwitch("-r,--recursive"));
            Assert.IsTrue(reader.GetSwitch("-h,--help"));
        }
    }
}
