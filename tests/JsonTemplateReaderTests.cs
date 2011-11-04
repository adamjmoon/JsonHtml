using JsonDataTable;
using JsonHtmlTable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlTemplates.Tests
{
    /// <summary>
    /// Summary description for SqlReportTests
    /// </summary>
    [TestClass]
    public class JsonTemplateReaderTests
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
        public void Can_Read_Json_Template()
        {
            var jsonSqlReportConfig = JsonConfigReader<SqlReportConfig>.GetConfig("C:\\Projects\\amoon\\SqlTemplates\\json\\SqlReportConfig.json");

            Assert.AreEqual(typeof(SqlReportConfig), jsonSqlReportConfig.GetType());
            Assert.AreEqual(jsonSqlReportConfig.ConnectionString, "Data Source=VMQADB1;Initial Catalog=CreditFulfillment;Persist Security Info=True;UID=CFWebUser;PWD=H4ppyFunB4ll");
            
        }
    }
}