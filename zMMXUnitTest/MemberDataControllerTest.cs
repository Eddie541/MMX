using MMXDataLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace zMMXUnitTest
{
    
    
    /// <summary>
    ///This is a test class for MemberDataControllerTest and is intended
    ///to contain all MemberDataControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MemberDataControllerTest {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext) {
            
        //    TestDataStore.CurrentDataStore = new TestDataStore();

        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CreateMotorcycle
        ///</summary>
        [TestMethod()]       
        public void CreateMotorcycleTest() {
            MemberDataController target = new MemberDataController(TestDataStore.Connection);
            int memberContactKey = 3; 
            string brand = "Penton"; 
            string model = "CR"; 
            short year = 1973; 
            string displacement = "125"; 
            string ridingNumber = "19"; 
            target.CreateMotorcycle(memberContactKey, brand, model, year, displacement, ridingNumber);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateAddress
        ///</summary>
        [TestMethod()]
        public void CreateAddressTest() {

            MemberDataController target = new MemberDataController(TestDataStore.Connection);
            string streetAddress1 = "1765 Harvest Dr."; 
            string streetAddress2 = "";
            string city = "Frederick"; 
            string state = "MD"; 
            string zip = "21702";
            Decimal latitude = new Decimal(0); //,
            Decimal longitude = new Decimal(0); 
            int expected = 4; 
            int actual;
            actual = target.CreateAddress(streetAddress1, streetAddress2, city, state, zip, latitude, longitude);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CreateMemberContact
        ///</summary>
        [TestMethod()]
        public void CreateMemberContactTest() {

            MemberDataController target = new MemberDataController(TestDataStore.Connection); 
            int memberKey = 7; 
            string firstName = "Fred"; 
            string middleName = "G"; 
            string lastName = "Smith"; 
            string phone = "111-222-5555"; 
            string amaNumber = "0011223378"; 
            DateTime dob = new DateTime(1930, 3,9); 
            int addressKey = 4; 
            //target.CreateMemberContact(memberKey, firstName, middleName, lastName, phone, amaNumber, dob, addressKey);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

       

        /// <summary>
        ///A test for DeleteMotorcycle
        ///</summary>
        [TestMethod()]
        public void DeleteMotorcycleTest() {
            
            MemberDataController target = new MemberDataController(TestDataStore.Connection); 
            int memberContactKey = 1; 
            int motorcycleKey = 2; 
            bool expected = true; 
            bool actual;
            actual = target.DeleteMotorcycle(memberContactKey, motorcycleKey);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UpdateAddress
        ///</summary>
        [TestMethod()]
        public void UpdateAddressTest() {

            MemberDataController target = new MemberDataController(TestDataStore.Connection); 
            int addressKey = 2; 
            string streetAddress1 = "112 Main Street"; 
            string streetAddress2 = "Apt 12"; 
            string city = "Washington"; 
            string state = "MO"; 
            string zip = "65789";
            Decimal latitude = new Decimal(45.446786);
            Decimal longitude = new Decimal(93.40756); 
            bool expected = true; 
            bool actual;
            actual = target.UpdateAddress(addressKey, streetAddress1, streetAddress2, city, state, zip, latitude, longitude);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UpdateMemberContact
        ///</summary>
        [TestMethod()]
        public void UpdateMemberContactTest() {

            MemberDataController target = new MemberDataController(TestDataStore.Connection); 
            int memberContactKey = 2; 
            string firstName = "Irving"; 
            string middleName = "J"; 
            string lastName = "Berlin"; 
            string phone = "XXX-XXX-XXXX"; 
            string amaNumber = "1245678"; 
            DateTime dob = new DateTime(1910, 1, 30); 
            //target.UpdateMemberContact(memberContactKey, firstName, middleName, lastName, phone, amaNumber, dob);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateMotorcycle
        ///</summary>
        [TestMethod()]
        public void UpdateMotorcycleTest() {

            MemberDataController target = new MemberDataController(TestDataStore.Connection); 
            int motorcycleKey = 5;
            string brand = "Husqvarna";
            string model = "CR125"; 
            short year = 2012; 
            string displacement = "125"; 
            string ridingNumber = "111"; 
            target.UpdateMotorcycle(motorcycleKey, brand, model, year, displacement, ridingNumber);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
