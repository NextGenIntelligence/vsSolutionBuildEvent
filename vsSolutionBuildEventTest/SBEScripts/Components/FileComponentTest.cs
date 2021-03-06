﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.vsSBE.Exceptions;
using net.r_eg.vsSBE.SBEScripts;
using net.r_eg.vsSBE.SBEScripts.Components;
using net.r_eg.vsSBE.SBEScripts.Exceptions;

namespace net.r_eg.vsSBE.Test.SBEScripts.Components
{
    /// <summary>
    ///This is a test class for FileComponentTest and is intended
    ///to contain all FileComponentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileComponentTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void parseTest()
        {
            FileComponent target = new FileComponent();
            target.parse("#[File get(\"file\")]");
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void parseTest2()
        {
            FileComponent target = new FileComponent();
            target.parse("File get(\"file\")");
        }

        /// <summary>
        ///A test for parse - stGet
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stGetParseTest1()
        {
            FileComponent target = new FileComponent();
            target.parse("[File get(file)]");
        }

        /// <summary>
        ///A test for parse - stGet
        ///</summary>
        [TestMethod()]
        public void stGetParseTest2()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            Assert.AreEqual("content from file", target.parse("[File get(\"file\")]"));
        }

        /// <summary>
        ///A test for parse - stGet
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ScriptException))]
        public void stGetParseTest3()
        {
            FileComponentAccessor target = new FileComponentAccessor(true);
            target.parse("[File get(\"file\")]");
        }

        /// <summary>
        ///A test for parse - stCall
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stCallParseTest1()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File call(file)]");
        }

        /// <summary>
        ///A test for parse - stCall
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stCallParseTest2()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File out(file)]");
        }

        /// <summary>
        ///A test for parse - stCall
        ///</summary>
        [TestMethod()]
        public void stCallParseTest3()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            Assert.AreEqual("stdout", target.parse("[File call(\"file\")]"));
            Assert.AreEqual("stdout", target.parse("[File call(\"file\", \"args\")]"));
            Assert.AreEqual("silent stdout", target.parse("[File scall(\"file\")]"));
            Assert.AreEqual("silent stdout", target.parse("[File scall(\"file\", \"args\")]"));
            Assert.AreEqual("silent stdout", target.parse("[File sout(\"file\")]"));
            Assert.AreEqual("silent stdout", target.parse("[File sout(\"file\", \"args\")]"));
        }

        /// <summary>
        ///A test for parse - stCall
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ScriptException))]
        public void stCallParseTest4()
        {
            FileComponentAccessor target = new FileComponentAccessor(true);
            target.parse("[File call(\"file\")]");
        }

        /// <summary>
        ///A test for parse - stCall
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stCallParseTest5()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File call(\"file\", \"args\", \"10\")]");
        }

        /// <summary>
        ///A test for parse - stCall
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stCallParseTest6()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File call(\"file\", 10)]");
        }

        /// <summary>
        ///A test for parse - stCall
        ///</summary>
        [TestMethod()]
        public void stCallParseTest7()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            Assert.AreEqual("stdout", target.parse("[File call(\"file\", \"args\", 0)]"));
            Assert.AreEqual("silent stdout", target.parse("[File scall(\"file\", \"args\", 15)]"));
            Assert.AreEqual("silent stdout", target.parse("[File sout(\"file\", \"args\", 10)]"));
        }

        /// <summary>
        ///A test for parse - stCall
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stCallParseTest8()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File call(\"file\", \"args\", )]");
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stWriteParseTest1()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File write(\"file\")]");
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        public void stWriteParseTest2()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File write(\"file\"):data]");
            Assert.AreEqual("data", target.parse("[File get(\"file\")]"));

            // append, writeLine, appendLine is identical for current accessor
            // however, also need checking the entry point as for the write() above:

            target.parse("[File append(\"file\"):data]");
            Assert.AreEqual("data", target.parse("[File get(\"file\")]"));
            target.parse("[File writeLine(\"file\"):data]");
            Assert.AreEqual("data", target.parse("[File get(\"file\")]"));
            target.parse("[File appendLine(\"file\"):data]");
            Assert.AreEqual("data", target.parse("[File get(\"file\")]"));
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        public void stWriteParseTest3()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File write(\"file\"): multi\nline\" \n 'data'.]");
            Assert.AreEqual(" multi\nline\" \n 'data'.", target.parse("[File get(\"file\")]"));
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ScriptException))]
        public void stWriteParseTest4()
        {
            FileComponentAccessor target = new FileComponentAccessor(true);
            target.parse("[File write(\"file\"):data]");
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stWriteParseTest5()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File write(\"file\", true):data]");
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stWriteParseTest6()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File write(\"file\", true, true):data]");
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stWriteParseTest7()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File write(\"file\", \"true\", \"true\", \"utf-8\"):data]");
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stWriteParseTest8()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File append(\"file\", true, true, \"utf-8\"):data]");
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stWriteParseTest9()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File appendLine(\"file\", true, true, \"utf-8\"):data]");
        }

        /// <summary>
        ///A test for parse - stWrite
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stWriteParseTest10()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File writeLine(\"file\", true, true, \"utf-8\"):data]");
        }

        /// <summary>
        ///A test for parse - stReplace
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stReplaceParseTest1()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File replace(file, pattern, replacement)]");
        }

        /// <summary>
        ///A test for parse - stReplace
        ///</summary>
        [TestMethod()]
        public void stReplaceParseTest2()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File replace(\"file\", \"from\", \"to\")]");
            Assert.AreEqual("content to file", target.parse("[File get(\"file\")]"));
        }

        /// <summary>
        ///A test for parse - stReplace
        ///</summary>
        [TestMethod()]
        public void stReplaceParseTest3()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File replace.Regexp(\"file\", \"t\\s*from\", \"t to\")]");
            Assert.AreEqual("content to file", target.parse("[File get(\"file\")]"));
        }

        /// <summary>
        ///A test for parse - stReplace
        ///</summary>
        [TestMethod()]
        public void stReplaceParseTest4()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File replace.Wildcards(\"file\", \"con*from \", \"\")]");
            Assert.AreEqual("file", target.parse("[File get(\"file\")]"));
        }

        /// <summary>
        ///A test for parse - stReplace
        ///</summary>
        [TestMethod()]
        public void stReplaceParseTest5()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File replace.Regex(\"file\", \"t\\s*from\", \"t to\")]");
            Assert.AreEqual("content to file", target.parse("[File get(\"file\")]"));
        }

        /// <summary>
        ///A test for parse - stExists
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void stExistsParseTest1()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            target.parse("[File exists.stub(\"path\")]");
        }

        /// <summary>
        ///A test for parse - stExists
        ///</summary>
        [TestMethod()]
        public void stExistsParseTest2()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            string realDir = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);

            Assert.AreEqual(Value.VFALSE, target.parse("[File exists.directory(\"c:\\stubdir\\notexist\")]"));
            Assert.AreEqual(Value.VTRUE, target.parse("[File exists.directory(\"" + realDir + "\")]"));
        }

        /// <summary>
        ///A test for parse - stExists
        ///</summary>
        [TestMethod()]
        public void stExistsParseTest3()
        {
            FileComponentAccessor target = new FileComponentAccessor();

            Assert.AreEqual(Value.VFALSE, target.parse("[File exists.directory(\"System32\", false)]"));
            //Assert.AreEqual(Value.VFALSE, target.parse("[File exists.directory(\"" + realDir + "\", true)]"));
            Assert.AreEqual(Value.VTRUE, target.parse("[File exists.directory(\"System32\", true)]"));
        }

        /// <summary>
        ///A test for parse - stExists
        ///</summary>
        [TestMethod()]
        public void stExistsParseTest4()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            string realFile = Assembly.GetAssembly(GetType()).Location;

            Assert.AreEqual(Value.VFALSE, target.parse("[File exists.file(\"file\")]"));
            Assert.AreEqual(Value.VTRUE, target.parse("[File exists.file(\"" + realFile + "\")]"));
        }

        /// <summary>
        ///A test for parse - stExists
        ///</summary>
        [TestMethod()]
        public void stExistsParseTest5()
        {
            FileComponentAccessor target = new FileComponentAccessor();
            string realFile = Path.GetFileName(Assembly.GetAssembly(GetType()).Location);

            Assert.AreEqual(Value.VFALSE, target.parse("[File exists.file(\"cmd.exe\", false)]"));
            //Assert.AreEqual(Value.VFALSE, target.parse("[File exists.file(\"" + realFile + "\", true)]"));
            Assert.AreEqual(Value.VTRUE, target.parse("[File exists.file(\"cmd.exe\", true)]"));
        }

        private class FileComponentAccessor: FileComponent
        {
            public bool throwError = false;
            protected string content = "content from file";

            public FileComponentAccessor(bool throwError = false)
            {
                Settings.setWorkingPath("/");
                this.throwError = throwError;
            }

            protected override string readToEnd(string file, Encoding enc, bool detectEncoding)
            {
                if(throwError) {
                    throw new System.IO.FileNotFoundException(String.Format("Some error for '{0}'", file));
                }
                return content;
            }

            protected override Encoding detectEncodingFromFile(string file)
            {
                return Encoding.UTF8;
            }

            protected override void writeToFile(string file, string data, bool append, bool writeLine, Encoding enc)
            {
                if(throwError) {
                    throw new System.IO.IOException(String.Format("Some error for '{0}'", file));
                }
                content = data;
            }

            protected override string findFile(string file)
            {
                return file;
            }

            protected override string run(string file, string args, bool silent, bool stdOut, int timeout = 0)
            {
                if(throwError) {
                    throw new ComponentException(String.Format("Some error for '{0} {1}'", file, args));
                }
                return String.Format("{0}stdout", silent? "silent ": String.Empty);
            }
        }
    }
}
