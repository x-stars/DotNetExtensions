﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.CommandLine.Specialized
{
    [TestClass]
    public class CmdArgumentReaderTest
    {
        // App.exe InputFile [/OUT:OutputFile] [/DEPTH:Depth] [/R] [/F]

        private static readonly string[] paramNames = { "/OUT", "/D" };
        private static readonly string[] switchNames = { "/R", "/F" };

        [TestMethod]
        public void GetParameterAndSwitch_CommonOrder_WorksProperly()
        {
            string[] arguments = { "file1.txt", "/out:file2.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.AreEqual("file2.txt", reader.GetArgument("/OUT"));
            Assert.IsNull(reader.GetArgument("/DEPTH"));
            Assert.IsFalse(reader.GetOption("/R"));
            Assert.IsTrue(reader.GetOption("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MessOrder_WorksProperly()
        {
            string[] arguments = { "/out:file2.txt", "file1.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.AreEqual("file2.txt", reader.GetArgument("/OUT"));
            Assert.IsNull(reader.GetArgument("/D"));
            Assert.IsFalse(reader.GetOption("/R"));
            Assert.IsTrue(reader.GetOption("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_MultiParam_WorksProperly()
        {
            string[] arguments = { "file1.txt", "/out:file2.txt", "/f", "/OUT:file3.txt", "/Depth:0" };
            var reader = new CmdArgumentReader(arguments, paramNames, switchNames);
            Assert.AreEqual("file1.txt", reader.GetArgument(0));
            Assert.IsTrue(reader.GetArguments("/OUT").SequenceEqual(new[] { "file2.txt", "file3.txt" }));
            Assert.AreEqual("0", reader.GetArgument("/DEPTH"));
            Assert.IsFalse(reader.GetOption("/R"));
            Assert.IsTrue(reader.GetOption("/F"));
        }

        [TestMethod]
        public void GetParameterAndSwitch_ExtraParam_Fails()
        {
            string[] arguments = { "/out:file2.txt", "/set", "file1.txt", "/f" };
            var reader = new CmdArgumentReader(arguments, paramNames, switchNames);
            Assert.AreNotEqual(reader.GetArgument(0), "file1.txt");
            Assert.AreEqual("file1.txt", reader.GetArgument(1));
            Assert.AreEqual("file2.txt", reader.GetArgument("/OUT"));
            Assert.IsNull(reader.GetArgument("/DEPTH"));
            Assert.IsFalse(reader.GetOption("/R"));
            Assert.IsTrue(reader.GetOption("/F"));
        }
    }
}
