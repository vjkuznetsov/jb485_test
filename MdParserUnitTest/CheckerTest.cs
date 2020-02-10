using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using MdParser;
using MdParser.interfaces;
using MdParser.models;

namespace MdParserUnitTest
{
    [TestClass]
    public class CheckerTest
    {
        private ServiceProvider _serviceProvider;
        private IMarkdownChecker _checker;

        [TestInitialize]
        public void Init()
        {
            _serviceProvider = ContainerBuilder.CreateContainer();
            _checker = _serviceProvider.GetService<IMarkdownChecker>();
        }

        [TestMethod]
        public void SimpleImageWithDescription()
        {
            var lines = new List<string> {
                "![photo1](link)",
                "Рисунок 1"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(1, result.ImageCount);
            Assert.AreEqual(0, result.TableCount);
            
            // TODO: fix unstable equals
            Assert.IsTrue(result.Messages.ToList().Count == 0);
        }

        [TestMethod]
        public void SimpleImageWithDescriptionAndBlankLines()
        {
            var lines = new List<string> {
                "![photo1](link)",
                "",
                "Рисунок 1"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(1, result.ImageCount);
            Assert.AreEqual(0, result.TableCount);

            // TODO: fix unstable equals
            Assert.IsTrue(result.Messages.ToList().Count == 0);
        }

        [TestMethod]
        public void SimpleImageWithoutDescription()
        {
            var lines = new List<string> {
                "![photo1](link)",
                "lores ipsum"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(1, result.ImageCount);
            Assert.AreEqual(0, result.TableCount);

            var errorMessage = Utility.GetMessageImageDescriptionMissing(file, 2);

            Assert.IsTrue(result.Messages.ToList().Contains(errorMessage));
        }

        [TestMethod]
        public void SimpleImageWithoutDescriptionAndBlankLine()
        {
            var lines = new List<string> {
                "![photo1](link)",
                "",
                "lores ipsum"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(1, result.ImageCount);
            Assert.AreEqual(0, result.TableCount);

            var errorMessage = Utility.GetMessageImageDescriptionMissing(file, 3);

            Assert.IsTrue(result.Messages.ToList().Contains(errorMessage));
        }

        [TestMethod]
        public void SimpleImageEofBeforeDescription()
        {
            var lines = new List<string> {
                "![photo1](link)"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(1, result.ImageCount);
            Assert.AreEqual(0, result.TableCount);

            var errorMessage = Utility.GetMessageImageDescriptionMissingEof(file);

            Assert.IsTrue(result.Messages.ToList().Contains(errorMessage));
        }

        [TestMethod]
        public void SimpleTableWithDescription()
        {
            var lines = new List<string> {
                "| Tables | Are | Cool |",
                "| ------------- |:-------------:| -----:|",
                "| col 3 is      | right - aligned | $1600 |",
                "| col 2 is      | centered |   $12 |",
                "| zebra stripes | are neat |    $1 |",
                "Таблица 1"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(0, result.ImageCount);
            Assert.AreEqual(1, result.TableCount);

            // TODO: fix unstable equals
            Assert.IsTrue(result.Messages.ToList().Count == 0);
        }

        [TestMethod]
        public void SimpleTableWithDescriptionAndBlankLine()
        {
            var lines = new List<string> {
                "| Tables | Are | Cool |",
                "| ------------- |:-------------:| -----:|",
                "| col 3 is      | right - aligned | $1600 |",
                "| col 2 is      | centered |   $12 |",
                "| zebra stripes | are neat |    $1 |",
                " ",
                "Таблица 1"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(0, result.ImageCount);
            Assert.AreEqual(1, result.TableCount);

            // TODO: fix unstable equals
            Assert.IsTrue(result.Messages.ToList().Count == 0);
        }

        [TestMethod]
        public void SimpleTableWithoutDescription()
        {
            var lines = new List<string> {
                "| Tables | Are | Cool |",
                "| ------------- |:-------------:| -----:|",
                "| col 3 is      | right - aligned | $1600 |",
                "| col 2 is      | centered |   $12 |",
                "| zebra stripes | are neat |    $1 |",
                "lores ipsum"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(0, result.ImageCount);
            Assert.AreEqual(1, result.TableCount);

            var errorMessage = Utility.GetMessageTableDescriptionMissing(file, 6);

            Assert.IsTrue(result.Messages.ToList().Contains(errorMessage));
        }

        [TestMethod]
        public void SimpleTableWithoutDescriptionAndBlankLine()
        {
            var lines = new List<string> {
                "| Tables | Are | Cool |",
                "| ------------- |:-------------:| -----:|",
                "| col 3 is      | right - aligned | $1600 |",
                "| col 2 is      | centered |   $12 |",
                "| zebra stripes | are neat |    $1 |",
                " ",
                "lores ipsum"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(0, result.ImageCount);
            Assert.AreEqual(1, result.TableCount);

            var errorMessage = Utility.GetMessageTableDescriptionMissing(file, 7);

            Assert.IsTrue(result.Messages.ToList().Contains(errorMessage));
        }

        [TestMethod]
        public void SimpleTableEofAfterBlankLine()
        {
            var lines = new List<string> {
                "| Tables | Are | Cool |",
                "| ------------- |:-------------:| -----:|",
                "| col 3 is      | right - aligned | $1600 |",
                "| col 2 is      | centered |   $12 |",
                "| zebra stripes | are neat |    $1 |",
                ""
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(0, result.ImageCount);
            Assert.AreEqual(1, result.TableCount);

            var errorMessage = Utility.GetMessageTableDescriptionMissingEof(file);

            Assert.IsTrue(result.Messages.ToList().Contains(errorMessage));
        }

        [TestMethod]
        public void SimpleTableEofWithoutDescription()
        {
            var lines = new List<string> {
                "| Tables | Are | Cool |",
                "| ------------- |:-------------:| -----:|",
                "| col 3 is      | right - aligned | $1600 |",
                "| col 2 is      | centered |   $12 |",
                "| zebra stripes | are neat |    $1 |",
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(0, result.ImageCount);
            Assert.AreEqual(1, result.TableCount);

            var errorMessage = Utility.GetMessageTableDescriptionMissingEof(file);

            Assert.IsTrue(result.Messages.ToList().Contains(errorMessage));
        }

        [TestMethod]
        public void TableWithBadFormat()
        {
            var lines = new List<string> {
                "Markdown | Less | Pretty",
                "-- - | --- | ---",
                "*Still * | `renders` | **nicely * *",
                "1 | 2 | 3"
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(0, result.ImageCount);
            Assert.AreEqual(1, result.TableCount);
        }

        [TestMethod]
        public void TableWithBadFormatOnlyHeader()
        {
            var lines = new List<string> {
                "Markdown | Less | Pretty",
                "-- - | --- | ---",
            };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(0, result.ImageCount);
            Assert.AreEqual(1, result.TableCount);
        }

        [TestMethod]
        public void TwoImageTwoTablesFromOfficialGuide()
        {
            var lines = new List<string> {
                "test",
                "![photo1](link)",
                "![photo2](link)",
                "",
                "| Tables | Are | Cool |",
                "| ------------- |:-------------:| -----:|",
                "| col 3 is      | right - aligned | $1600 |",
                "| col 2 is      | centered |   $12 |",
                "| zebra stripes | are neat |    $1 |",
                "",
                "",
                "Markdown | Less | Pretty",
                "-- - | --- | ---",
                "*Still * | `renders` | **nicely * *",
                "1 | 2 | 3" };

            var file = new MarkdownFile()
            {
                FilePath = "testFile",
                Lines = lines
            };

            var result = _checker.Check(file);

            Assert.AreEqual(2, result.ImageCount);
            Assert.AreEqual(2, result.TableCount);

        }
    }
}
