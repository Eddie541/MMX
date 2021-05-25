using MMXDataLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace zMMXUnitTest
{
    
    
    /// <summary>
    ///This is a test class for RaceDataControllerTest and is intended
    ///to contain all RaceDataControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RaceDataControllerTest {


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
        ///A test for CloseTrack
        ///</summary>
        [TestMethod()]
        public void CloseTrackTest() {
            string testConn = TestDataStore.Connection;
            RaceDataController target = new RaceDataController(testConn); 
            int trackKey = 1; 
            target.CloseTrack(trackKey);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateRace
        ///</summary>
        [TestMethod()]
        public void CreateRaceTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            string raceName = "MMX Race #1"; 
            int trackKey = 1; 
            DateTime raceDate = new DateTime(2013, 4, 6); 
            target.CreateRace(raceName, trackKey, raceDate);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateRaceResult
        ///</summary>
        [TestMethod()]
        public void CreateRaceResultTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            int raceClassMemberKey = 2; 
            int raceKey = 1; 
            short motoOnePosition = 5; 
            short motoTwoPosition = 6; 
            short overall = 5;
            string ridingNumber = "19x"; 
            string brand = "Penton"; 
            string note = "Test Result 2";
            string motoOneStatus = "Finished";
            string motoTwoStatus = "Finished";
            bool expected = true;
            bool actual = target.CreateRaceResult(raceClassMemberKey, raceKey, motoOnePosition, motoOneStatus, motoTwoPosition, motoTwoStatus, overall, ridingNumber, brand, note);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CreateTrack
        ///</summary>
        [TestMethod()]
        public void CreateTrackTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            string name = "Budds Creek"; 
            string contactName = "Jonathon Beasley";
            string phone = "301-475-2000";
            string emailAddress = "jonathan@buddscreek.com"; 
            string webAddress = "www.buddscreek.com"; 
            int addressKey = 3; 
            int expected = 1; 
            int actual;
            actual = target.CreateTrack(name, contactName, phone, emailAddress, webAddress, addressKey);
            Assert.AreEqual(expected, actual);
        }

      

        /// <summary>
        ///A test for GetFinishedStatus
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MMXDataLib.dll")]
        public void GetFinishedStatusTest() {
            short position = 0; 
            string expected = string.Empty; 
            string actual;
            actual = RaceDataController_Accessor.GetFinishedStatus(position);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
     

        /// <summary>
        ///A test for GetPositionPoints
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MMXDataLib.dll")]
        public void GetPositionPointsTest() {
            short position = 2; 
            short expected = 22; 
            short actual;
            actual = RaceDataController_Accessor.GetPositionPoints(position);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetRace
        ///</summary>
        [TestMethod()]
        public void GetRaceTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            DateTime raceDate = new DateTime(2013,4,6); // todo set race date
            int expected = 1;
            int actual = -1;
            Race race;
            race = target.GetRace(raceDate);
            actual = race.RaceKey;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetRace
        ///</summary>
        [TestMethod()]
        public void GetRaceTest1() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            int raceKey = 0; 
            Race expected = null; 
            Race actual;
            actual = target.GetRace(raceKey);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetTrackRaces
        ///</summary>
        [TestMethod()]
        public void GetTrackRacesTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            int trackKey = 1;
            IEnumerable<Race> expected = null; 
            IEnumerable<Race> actual;
            actual = target.GetTrackRaces(trackKey);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetYearsRaces
        ///</summary>
        [TestMethod()]
        public void GetYearsRacesTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            int year = 2013; 
            //IEnumerable<Race> expected = null; 
            int expected = 1;
            int actualCount = 0;
            IEnumerable<Race> actual;
            actual = target.GetYearsRaces(year);
            foreach (Race r in actual) {
                actualCount++;
            }
            Assert.AreEqual(expected, actualCount);
        }

        /// <summary>
        ///A test for UpdateRace
        ///</summary>
        [TestMethod()]
        public void UpdateRaceTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn);
            string raceName = "";
            int raceKey = 0; 
            int trackKey = 0; 
            DateTime raceDate = new DateTime(); 
            target.UpdateRace(raceKey, raceName, trackKey, raceDate);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateRaceResultDetails
        ///</summary>
        [TestMethod()]
        public void UpdateRaceResultDetailsTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            int raceResultKey = 0; 
            string ridingNumber = string.Empty; 
            string brand = string.Empty; 
            string note = string.Empty; 
            target.UpdateRaceResultDetails(raceResultKey, ridingNumber, brand, note);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateRaceResultPosition
        ///</summary>
        [TestMethod()]
        public void UpdateRaceResultPositionTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            int raceResultKey = 1; 
            short motoOnePosition = 2; 
            short motoTwoPosition = 20; 
            short overall = 18;
            string motoOneStatus = "Finished";
            string motoTwoStatus = "Finished";
            target.UpdateRaceResultPosition(raceResultKey, motoOnePosition, motoOneStatus, motoTwoPosition, motoTwoStatus, overall);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateTrack
        ///</summary>
        [TestMethod()]
        public void UpdateTrackTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            int trackKey = 0; 
            string name = string.Empty; 
            string contactName = string.Empty; 
            string phone = string.Empty; 
            string emailAddress = string.Empty; 
            string webAddress = string.Empty; 
            bool isOpen = false; 
            target.UpdateTrack(trackKey, name, contactName, phone, emailAddress, webAddress, isOpen);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetMemberRaceResults
        ///</summary>
        [TestMethod()]
        public void GetMemberRaceResultsTest() {
            string testConn = TestDataStore.Connection;  
            RaceDataController target = new RaceDataController(testConn); 
            int memberContactKey = 2; 
            short year = 2012; 
            //IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> expected = null; 
            //IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> actual;

            IEnumerable<IGrouping<int, RaceClassMemberRaceResult>> actual = target.GetMemberRaceResults(memberContactKey, year);
            foreach (IGrouping<int, RaceClassMemberRaceResult> group in actual) {
                Race race = target.GetRace(group.Key);
                string raceName = race.RaceName;
                foreach (RaceClassMemberRaceResult r in group) {
                    string className = r.ClassName;
                    //string brand = r.RaceBrand;
                }

            }
            //Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetClassRaceResults
        ///</summary>
        [TestMethod()]
        public void GetClassRaceResultTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            int raceKey = 1; 
            int raceClassKey = 1; 
            IEnumerable<RaceClassMemberRaceResult> expected = null; 
            IEnumerable<RaceClassMemberRaceResult> actual;
            actual = target.GetClassRaceResult(raceKey, raceClassKey);
            foreach(RaceClassMemberRaceResult rr in actual) {
                string name = rr.LastName;
            }
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetAllRaceResultsForYear
        ///</summary>
        [TestMethod()]
        public void GetAllRaceResultsForYearTest() {
            string testConn = TestDataStore.Connection; 
            RaceDataController target = new RaceDataController(testConn); 
            short year = 0; 
            IEnumerable<IGrouping<int, MemberRaceResultsView>> expected = null;
            IEnumerable<IGrouping<int, MemberRaceResultsView>> actual;
            actual = target.GetAllRaceResultsForYear(year);

            foreach (IGrouping<int, MemberRaceResultsView> outerGroup in actual) {
                Race race = target.GetRace(outerGroup.Key);
                string raceName = race.RaceName;

                IEnumerable<IGrouping<int, MemberRaceResultsView>> innerGroups = outerGroup.GroupBy(og => og.RaceClassKey);
                foreach (IGrouping<int, MemberRaceResultsView> innerGroup in innerGroups) {
                    int raceClassKey = innerGroup.Key;
                    foreach (MemberRaceResultsView r in innerGroup) {
                        string className = r.ClassName;
                        //string brand = r.RaceBrand;
                    }
                }

            }

        }
    }
}
