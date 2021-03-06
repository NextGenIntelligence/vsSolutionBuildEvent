﻿using System;
using System.Linq;
using EnvDTE80;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using net.r_eg.vsSBE.MSBuild;

namespace net.r_eg.vsSBE.Test.MSBuild
{
    /// <summary>
    ///This is a test class for MSBuildParserTest and is intended
    ///to contain all MSBuildParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MSBuildParserTest
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
        ///A test for getProperty
        ///</summary>
        [TestMethod()]
        public void getPropertyTest()
        {
            var mockDte2                    = new Mock<EnvDTE80.DTE2>();
            var mockSolution                = new Mock<EnvDTE.Solution>();
            var mockSolutionBuild           = new Mock<EnvDTE.SolutionBuild>();
            var mockSolutionConfiguration2  = new Mock<EnvDTE80.SolutionConfiguration2>();

            mockSolutionConfiguration2.SetupGet(p => p.Name).Returns("Release");
            mockSolutionConfiguration2.SetupGet(p => p.PlatformName).Returns("x86");

            mockSolutionBuild.SetupGet(p => p.ActiveConfiguration).Returns(mockSolutionConfiguration2.Object);
            mockSolution.SetupGet(p => p.SolutionBuild).Returns(mockSolutionBuild.Object);
            mockDte2.SetupGet(p => p.Solution).Returns(mockSolution.Object);

            vsSBE.MSBuild.Parser target = new Parser(new net.r_eg.vsSBE.Environment(mockDte2.Object));
            Assert.IsNotNull(target.getProperty("Configuration"));
            Assert.IsNotNull(target.getProperty("Platform"));
        }

        /// <summary>
        ///A test for parseVariablesSBE
        ///</summary>
        [TestMethod()]
        public void parseVariablesSBETest()
        {
            vsSBE.MSBuild.Parser target = new Parser(new net.r_eg.vsSBE.Environment((DTE2)null));

            string expected = "$(name)";
            string actual   = target.parseVariablesSBE("$(name)", "subname", "value");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for parseVariablesSBE
        ///</summary>
        [TestMethod()]
        public void parseVariablesSBETest2()
        {
            vsSBE.MSBuild.Parser target = new Parser(new net.r_eg.vsSBE.Environment((DTE2)null));

            string expected = "value";
            string actual   = target.parseVariablesSBE("$(name)", "name", "value");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for parseVariablesSBE
        ///</summary>
        [TestMethod()]
        public void parseVariablesSBETest3()
        {
            vsSBE.MSBuild.Parser target = new Parser(new net.r_eg.vsSBE.Environment((DTE2)null));

            string expected = "$$(name)";
            string actual   = target.parseVariablesSBE("$$(name)", "name", "value");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for parseVariablesSBE
        ///</summary>
        [TestMethod()]
        public void parseVariablesSBETest4()
        {
            vsSBE.MSBuild.Parser target = new Parser(new net.r_eg.vsSBE.Environment((DTE2)null));

            string expected = String.Empty;
            string actual   = target.parseVariablesSBE("$(name)", "name", null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest1()
        {
            vsSBE.MSBuild.Parser target = new Parser(new net.r_eg.vsSBE.Environment((DTE2)null));

            string actual   = target.parse("FooBar");
            string expected = "FooBar";

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest2()
        {
            string data     = "$(Path)";
            string actual   = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[P~Path~]", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest3()
        {
            string data     = "$$(Path)";
            string actual   = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("$(Path)", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest4()
        {
            string data     = "$$$(Path)";
            string actual   = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("$$(Path)", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest5()
        {
            string data     = "(Path)";
            string actual   = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("(Path)", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest6()
        {
            string data     = "$(Path:project)";
            string actual   = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[P~Path~project]", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest7()
        {
            string data = "$$(Path:project)";
            string actual = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("$(Path:project)", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest8()
        {
            string data     = "$([System.DateTime]::UtcNow.Ticks)";
            string actual   = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[E~[System.DateTime]::UtcNow.Ticks~]", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest9()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(Path.Replace('\\\\', '/'))";
            Assert.AreEqual("[E~Path.Replace('\\\\', '/')~]", target.parse(data));
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest10()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(Path.Replace('\\', '/'))";
            Assert.AreEqual("[E~Path.Replace('\\', '/')~]", target.parse(data));
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest11()
        {
            string data = "$(ProjectDir.Replace('\\', '/'):client)";
            string actual = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[E~ProjectDir.Replace('\\', '/')~client]", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest12()
        {
            vsSBE.MSBuild.Parser target = new Parser(new net.r_eg.vsSBE.Environment((DTE2)null));

            string actual = target.parse("$$(Path.Replace('\', '/'):project)");
            string expected = "$(Path.Replace('\', '/'):project)";

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest13()
        {
            string data = "$(ProjectDir.Replace(\"str1\", \"str2\"))";
            string actual = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[E~ProjectDir.Replace(\"str1\", \"str2\")~]", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest14()
        {
            string data = "$(ProjectDir.Replace('str1', 'str2'))";
            string actual = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[E~ProjectDir.Replace('str1', 'str2')~]", actual);
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest15()
        {
            string data = "$(var.Method('~str~', $(OS:$($(data):project2)), \"~str2~\"):project)";
            string actual = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[E~var.Method('~str~', [P~OS~[E~[P~data~]~project2]], \"~str2~\")~project]", actual);
        }

        /// <summary>
        ///A test for parse - Wrapping
        ///</summary>
        [TestMethod()]
        public void parseWrappingTest1()
        {
            string data = "$($(ProjectDir.Replace('\\', '/'):client))";
            string actual = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[E~[E~ProjectDir.Replace('\\', '/')~client]~]", actual);
        }

        /// <summary>
        ///A test for parse - Wrapping
        ///</summary>
        [TestMethod()]
        public void parseWrappingTest2()
        {
            string data = "$($(ProjectDir.Replace('\\', '/')))";
            string actual = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[E~[E~ProjectDir.Replace('\\', '/')~]~]", actual);
        }

        /// <summary>
        ///A test for parse - Wrapping
        ///</summary>
        [TestMethod()]
        public void parseWrappingTest3()
        {
            string data = "$($(var.Method('str', $(OS))):$(var.Method('str2', $(SO))))";
            string actual = (new MSBuildParserAccessor.ToParse()).parse(data);
            Assert.AreEqual("[E~[E~var.Method('str', [P~OS~])~]:[E~var.Method('str2', [P~SO~])~]~]", actual);
        }

        /// <summary>
        ///A test for parse - Wrapping
        ///</summary>
        [TestMethod()]
        public void parseWrappingTest4()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$($(Path:project))";
            string actual = target.parse(data);
            Assert.IsTrue("[E~[P~Path~project]~]" == actual || "[P~[P~Path~project]~]" == actual);
        }

        /// <summary>
        ///A test for parse - Variable & data
        ///</summary>
        [TestMethod()]
        public void parseVarAndDataTest1()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var=$(Path.Replace('\\', '/')):project)";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual(null, target.uvariable.get("var"));
            Assert.AreEqual("[E~Path.Replace('\\', '/')~]", target.uvariable.get("var", "project"));
        }

        /// <summary>
        ///A test for parse - Variable & data
        ///</summary>
        [TestMethod()]
        public void parseVarAndDataTest2()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = $$(Path:project))";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("$(Path:project)", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - Variable & data
        ///</summary>
        [TestMethod()]
        public void parseVarAndDataTest3()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$$(var = $$(Path:project))";
            Assert.AreEqual("$(var = $$(Path:project))", target.parse(data));
        }

        /// <summary>
        ///A test for parse - Variable & data
        ///</summary>
        [TestMethod()]
        public void parseVarAndDataTest4()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$$(var = $(Path:project))";
            Assert.AreEqual("$(var = $(Path:project))", target.parse(data));
        }

        /// <summary>
        ///A test for parse - Variable & data
        ///</summary>
        [TestMethod()]
        public void parseVarAndDataTest5()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = $(Path:project))";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("[P~Path~project]", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - Variable & data
        ///</summary>
        [TestMethod()]
        public void parseVarAndDataTest6()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var=$(Path:project2):project)";

            Assert.IsTrue(target.uvariable.Variables.Count() < 1);
            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("[P~Path~project2]", target.uvariable.get("var", "project"));
            Assert.IsTrue(target.uvariable.Variables.Count() == 1);
        }

        /// <summary>
        ///A test for parse - Variable & data
        ///</summary>
        [TestMethod()]
        public void parseVarAndDataTest7()
        {
            MSBuildParserAccessor.ToUserVariables target = new MSBuildParserAccessor.ToUserVariables();

            target.uvariable.set("var", "project", "is a Windows_NT"); //"$(var.Replace('%OS%', $(OS)):project)";

            string actual = target.parse("$(var:project)");
            Assert.AreEqual("is a Windows_NT", actual);
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest1()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            /*
                $(p1 = $(Platform))
                $(p2 = $(p1))
                $(p2)
             */
            string data = "$(p1 = $(Platform))$(p2 = $(p1))$(p2)";
            Assert.AreEqual("[P~Platform~]", target.parse(data));
            Assert.AreEqual("[P~Platform~]", target.uvariable.get("p1"));
            Assert.AreEqual("[P~Platform~]", target.uvariable.get("p2"));
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest2()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            /*
                $(p1 = $(Platform))
                $(p2 = $$(p1))
                $(p2)
             */
            string data = "$(p1 = $(Platform))$(p2 = $$(p1))$(p2)";
            Assert.AreEqual("[P~Platform~]", target.parse(data));
            Assert.AreEqual("[P~Platform~]", target.uvariable.get("p1"));
            Assert.AreEqual("$(p1)", target.uvariable.get("p2"));
        }


        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest3()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            /*
                $(p1 = $(Platform))
                $(p2 = $$$(p1))
                $(p2)
             */
            string data = "$(p1 = $(Platform))$(p2 = $$$(p1))$(p2)";
            Assert.AreEqual("$(p1)", target.parse(data));
            Assert.AreEqual("[P~Platform~]", target.uvariable.get("p1"));
            Assert.AreEqual("$$(p1)", target.uvariable.get("p2"));
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest4()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            /*
                $(p1 = $(Platform))
                $(p2 = $$$$(p1))
                $(p2)
             */
            string data = "$(p1 = $(Platform))$(p2 = $$$$(p1))$(p2)";
            Assert.AreEqual("$$(p1)", target.parse(data));
            Assert.AreEqual("[P~Platform~]", target.uvariable.get("p1"));
            Assert.AreEqual("$$$(p1)", target.uvariable.get("p2"));
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest5()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(p2 = $(Platform))";
            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("[P~Platform~]", target.uvariable.get("p2"));
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest6()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(p2 = $$(Platform))";
            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("$(Platform)", target.uvariable.get("p2"));
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest7()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(p2 = $$$(Platform))";
            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("$$(Platform)", target.uvariable.get("p2"));
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest8()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(p2 = $$$$(Platform))";
            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("$$$(Platform)", target.uvariable.get("p2"));
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest9()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            /*
                $(p1 = $(Platform))
                $(p1)
             */
            string data = "$(p1 = $(Platform))$(p1)";
            Assert.AreEqual("[P~Platform~]", target.parse(data));
            Assert.AreEqual("[P~Platform~]", target.uvariable.get("p1"));
        }

        /// <summary>
        ///A test for parse - Escape & Variable
        ///</summary>
        [TestMethod()]
        public void parseEscapeAndVarTest10()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            /*
                $(p1 = $(Platform))
                $$(p1)
             */
            string data = "$(p1 = $(Platform))$$(p1)";
            Assert.AreEqual("$(p1)", target.parse(data));
            Assert.AreEqual("[P~Platform~]", target.uvariable.get("p1"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest1()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = \"$(Path:project)\")";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("[P~Path~project]", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest2()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = \" mixed $(Path:project) \" )";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual(" mixed [P~Path~project] ", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest3()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = \" $$(Path:project) \")";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual(" $(Path:project) ", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest4()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = ' $(Path:project) ')";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual(" $(Path:project) ", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest5()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = @"$(var = '$(Path.Replace(\'1\', \'2\'))')";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("$(Path.Replace('1', '2'))", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest6()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = '$(Path.Replace(\\\"1\\\", \\\"2\\\"))')";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("$(Path.Replace(\\\"1\\\", \\\"2\\\"))", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest7()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = \"$(Path.Replace(\\'1\\', \\'2\\'))\")";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("[E~Path.Replace(\\'1\\', \\'2\\')~]", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest8()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = \"$(Path.Replace(\\\"1\\\", \\\"2\\\"))\")";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("[E~Path.Replace(\"1\", \"2\")~]", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest9()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = @"$(var = $(Path.Replace(\'1\', '2')))";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual(@"[E~Path.Replace(\'1\', '2')~]", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest10()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var = $(Path.Replace(\\\"1\\\", \"2\")))";

            Assert.AreEqual("", target.parse(data));
            Assert.AreEqual("[E~Path.Replace(\\\"1\\\", \"2\")~]", target.uvariable.get("var"));
        }

        /// <summary>
        ///A test for parse - string
        ///</summary>
        [TestMethod()]
        public void parseStringTest11()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();
            
            Assert.AreEqual("", target.parse("$(name = ' test 12345  -_*~!@#$%^&= :) ')"));
            Assert.AreEqual(" test 12345  -_*~!@#$%^&= :) ", target.uvariable.get("name"));

            Assert.AreEqual("", target.parse("$(name = ' $( -_*~!@#$%^&= :) ')"));
            Assert.AreEqual(" $( -_*~!@#$%^&= :) ", target.uvariable.get("name"));
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest1()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = @"$(var.Method('str1', $(OS:$($(data.Replace('\', '/')):project2)), 'str2'):project)";
            Assert.AreEqual(@"[E~var.Method('str1', [E~OS:[E~[E~data.Replace('\', '/')~]~project2]~], 'str2')~project]", target.parse(data));
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest2()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = @"$(var.Method('str1', $(OS:$($(data.Method2):project2)), 'str2'):project)";
            Assert.AreEqual(@"[E~var.Method('str1', [P~OS~[E~[E~data.Method2~]~project2]], 'str2')~project]", target.parse(data));
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest3()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var.Method('str1', $(OS:$($([System.DateTime]::Parse(\"27.03.2015\").ToBinary()):project2)), 'str2'):project)";
            Assert.AreEqual("[E~var.Method('str1', [E~OS:[E~[E~[System.DateTime]::Parse(\"27.03.2015\").ToBinary()~]~project2]~], 'str2')~project]", target.parse(data));
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest4()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$(var.Method('str1', $(OS:$($(test.Ticks):project2)), \\\"str2\\\"):project)";
            Assert.AreEqual("[E~var.Method('str1', [P~OS~[E~[E~test.Ticks~]~project2]], \\\"str2\\\")~project]", target.parse(data));
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest5()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data     = "$(var.Method('str1', $(OS:$($(data):project2)), \\\"str2\\\"):project)";
            string actual   = target.parse(data);
            Assert.IsTrue("[E~var.Method('str1', [P~OS~[E~[P~data~]~project2]], \\\"str2\\\")~project]" == actual
                            || "[E~var.Method('str1', [P~OS~[P~[P~data~]~project2]], \\\"str2\\\")~project]" == actual);
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest6()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$([System.DateTime]::Parse(\"01.01.2000\").ToBinary())";
            Assert.AreEqual("[E~[System.DateTime]::Parse(\"01.01.2000\").ToBinary()~]", target.parse(data));
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest7()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$([System.DateTime]::Parse(\" left $([System.DateTime]::UtcNow.Ticks) right\").ToBinary())";
            Assert.AreEqual("[E~[System.DateTime]::Parse(\" left [E~[System.DateTime]::UtcNow.Ticks~] right\").ToBinary()~]", target.parse(data));
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest8()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$([System.DateTime]::Parse(' left $([System.DateTime]::UtcNow.Ticks) right').ToBinary())";
            Assert.AreEqual("[E~[System.DateTime]::Parse(' left $([System.DateTime]::UtcNow.Ticks) right').ToBinary()~]", target.parse(data));
        }

        /// <summary>
        ///A test for parse - nested
        ///</summary>
        [TestMethod()]
        public void parseNestedTest9()
        {
            MSBuildParserAccessor.ToParse target = new MSBuildParserAccessor.ToParse();

            string data = "$([System.TimeSpan]::FromTicks($([MSBuild]::Subtract($([System.DateTime]::UtcNow.Ticks), $([System.DateTime]::Parse('27.03.2015').ToBinary())))).TotalMinutes.ToString(\"0\"))";
            Assert.AreEqual("[E~[System.TimeSpan]::FromTicks([E~[MSBuild]::Subtract([E~[System.DateTime]::UtcNow.Ticks~], [E~[System.DateTime]::Parse('27.03.2015').ToBinary()~])~]).TotalMinutes.ToString(\"0\")~]", target.parse(data));
        }


        private class MSBuildParserAccessor
        {
            public class Accessor: vsSBE.MSBuild.Parser
            {
                public Accessor(): base(new net.r_eg.vsSBE.Environment((DTE2)null)) {}
                public Accessor(net.r_eg.vsSBE.Environment env): base(env) { }
            }

            public class StubEvaluatingProperty: Accessor
            {
                public override string evaluate(string unevaluated, string project)
                {
                    return String.Format("[E~{0}~{1}]", unevaluated, project);
                }

                public override string getProperty(string name, string project)
                {
                    if(uvariable.isExist(name, project)) {
                        return getUVariableValue(name, project);
                    }
                    return String.Format("[P~{0}~{1}]", name, project);
                }
            }

            public class ToUserVariables: StubEvaluatingProperty
            {
                public new Scripts.IUserVariable uvariable
                {
                    get { return base.uvariable; }
                    set { base.uvariable = value; }
                }
            }

            public class ToParse: ToUserVariables
            {

            }

            public class ToPrepareVariables: StubEvaluatingProperty
            {
                public new PreparedData prepare(string raw)
                {
                    return base.prepare(raw);
                }
            }

            public class ToEvaluateVariable: ToUserVariables
            {
                public new string evaluate(PreparedData prepared)
                {
                    return base.evaluate(prepared);
                }
            }
        }
    }
}