using MMXDataLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace zMMXUnitTest
{
    
    
    /// <summary>
    ///This is a test class for RaceClassDataControllerTest and is intended
    ///to contain all RaceClassDataControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RaceClassDataControllerTest {


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
        //public static void MyClassInitialize(TestContext testContext)
        //{
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
        ///A test for CloseEnrollmentForCurrentYear
        ///</summary>
        [TestMethod()]
        public void CloseEnrollmentForCurrentYearTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            bool expected = false; 
            bool actual;
            actual = target.CloseEnrollmentForCurrentYear();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateRaceClass
        ///</summary>
        [TestMethod()]
        public void CreateRaceClassTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            string className = "70+ B"; 
            short minAge = 70; 
            string note = "Need a split gate, too many entrants"; 
            target.CreateRaceClass(className, minAge, note);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateRaceClassMember
        ///</summary>
        [TestMethod()]
        public void CreateRaceClassMemberTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            int raceClassKey = 11; 
            int contactKey = 3; 
            int memberMotorcycleKey = 6; 
            bool hasPaid = false;            
            bool expected = true; 
            bool actual;
            actual = target.CreateRaceClassMember(raceClassKey, contactKey, memberMotorcycleKey, hasPaid, new StringBuilder());
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DeleteRaceClassMember
        ///</summary>
        [TestMethod()]
        public void DeleteRaceClassMemberTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            int raceClassMemberKey = 0; 
            string expected = string.Empty; 
            string actual;
            actual = target.DeleteRaceClassMember(raceClassMemberKey);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DisableRaceClass
        ///</summary>
        [TestMethod()]
        public void DisableRaceClassTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            int raceClassKey = 0; 
            target.DisableRaceClass(raceClassKey);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetRaceClassMember
        ///</summary>
        //[TestMethod()]
        //public void GetRaceClassMemberTest() {
        //    string testConn = TestDataStore.Connection; 
        //    RaceClassDataController target = new RaceClassDataController(testConn); 
        //    int raceClassMemberKey = 0; 
        //    RaceClassMember expected = null; 
        //    RaceClassMember actual;
        //    actual = target.GetRaceClassMember(raceClassMemberKey);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for GetRaceMemberAge
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MMXDataLib.dll")]
        public void GetRaceMemberAgeTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            int memberContactKey = 2; 
            short expected = 102; 
            short actual;
            actual = target.GetRaceMemberAge(memberContactKey);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsNumberUsedInClass
        ///</summary>
        [TestMethod()]
        public void IsNumberUsedInClassTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            int raceClassKey = 1; 
            string ridingNumber = "11"; 
            bool expected = false; 
            bool actual;
            actual = target.IsNumberUsedInClass(raceClassKey, ridingNumber);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OpenEnrollmentForNextYear
        ///</summary>
        [TestMethod()]
        public void OpenEnrollmentForNextYearTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            bool expected = false; 
            bool actual;
            actual = target.OpenEnrollmentForNextYear();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ResetEnrollmentYear
        ///</summary>
        [TestMethod()]
        public void ResetEnrollmentYearTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            short year = 0; 
            bool isOpen = false; 
            target.ResetEnrollmentYear(year, isOpen);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateRaceClass
        ///</summary>
        [TestMethod()]
        public void UpdateRaceClassTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            int raceClassKey = 0; 
            string className = string.Empty; 
            short minAge = 0; 
            string note = string.Empty; 
            target.UpdateRaceClass(raceClassKey, className, minAge, note);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateRaceClassMemberHasPaid
        ///</summary>
        [TestMethod()]
        public void UpdateRaceClassMemberHasPaidTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            int raceClassMemberKey = 0; 
            bool hasPaid = false; 
            target.UpdateRaceClassMemberHasPaid(raceClassMemberKey, hasPaid);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateRaceClassMemberRide
        ///</summary>
        [TestMethod()]
        public void UpdateRaceClassMemberRideTest() {
            string testConn = TestDataStore.Connection; 
            RaceClassDataController target = new RaceClassDataController(testConn); 
            int raceClassMemberKey = 0; 
            int newMotorcycleKey = 0; 
            //target.UpdateRaceClassMemberRide(raceClassMemberKey, newMotorcycleKey);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }




    }
}
