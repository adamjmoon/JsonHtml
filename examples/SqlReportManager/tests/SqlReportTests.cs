using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlReports.Tests
{
    /// <summary>
    /// Summary description for SqlReportTests
    /// </summary>
    [TestClass]
    public class SqlReportTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void Can_Generate_Json_Table()
        {
            var config = JsonConfigReader<SqlReportConfig>.GetConfig(@"C:\Projects\amoon\SiteAdmin\OneTechnologies.SiteAdministration\OneTechnologies.SiteAdmin.Portal\json\sqlReportConfig.json");
            SqlReport report = new SqlReport(config);
            var jsonReport = report.GetJsonTable();
            Assert.IsTrue(jsonReport.Length>0);

        }
    }
}