using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using MMXDataLib;

namespace MMXDataService {
    public class RaceResultFileProcessor {

        private readonly MemberDataController _memberDataController;
        private readonly RaceClassDataController _raceClassDataController;
        private readonly RaceDataController _raceDataController;


        public RaceResultFileProcessor() {
            _memberDataController = new MemberDataController();
            _raceClassDataController = new RaceClassDataController();
            _raceDataController = new RaceDataController();
        }

        public void ProcessRaceResultFile(string filePath, int raceKey, string worksheetName, int year) {
            List<RaceResultRowData> savedResults = new List<RaceResultRowData>();           
            
            worksheetName += "$";
            // todo set excel version connection string
            string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", filePath);

            var adapter = new OleDbDataAdapter("SELECT * FROM [" + worksheetName + "]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "results");

            var data = ds.Tables["results"].AsEnumerable();

            IEnumerable<RaceResultRowData> r = data.Where(x => x.Field<string>("Class") != string.Empty && x.Field<string>("Rider") != "zzz").Select(x =>
                new RaceResultRowData {
                    ClassName = ConvertToString(x.Field<object>("Class")),
                    Member = ConvertToString(x.Field<object>("Rider")),
                    RidingNumber = ConvertToString(x.Field<object>("#")),
                    Brand = ConvertToString(x.Field<object>("Brand")),
                    Position = ConvertToShort(x.Field<object>("Pos")),
                    //TotalPoints = ConvertToInt(x.Field<object>("TotalPoints")),
                    MotoOne = ConvertToString(x.Field<object>("Moto 1")),
                    MotoTwo = ConvertToString(x.Field<object>("Moto 2")),
                    RacePoints = ConvertToInt(x.Field<object>("Points"))
                });

            //int count = 0;
            foreach (RaceResultRowData rcr in r) {
                
                string firstName = "";
                string lastName = "";
                string [] riderName = rcr.Member.Split(new char [] {','});
                if (riderName != null && riderName.Length > 1) {
                    firstName = riderName[1];
                    lastName = riderName[0];
                } else {
                    continue;
                }

                int memberKey = _memberDataController.CreateTemporaryMember(firstName, lastName, rcr.ClassName);
                if (memberKey > 0) {
                    
                    rcr.SetMemberKey(memberKey);
                    savedResults.Add(rcr);
                }
                
            }

            
            foreach (RaceResultRowData xxr in savedResults) {              
                
                int raceClassMemberKey = _raceClassDataController.CreateImportedRaceClassMember(xxr.ClassName, xxr.MemberKey, year);
                if (raceClassMemberKey > 0) {
                    xxr.SetRaceClassMemberKey(raceClassMemberKey);
                } 
            }

            foreach (RaceResultRowData ffr in savedResults) {
                short m1 = 0;
                short m2 = 0;
                string m1Status = "Finished";
                string m2Status = "Finished";
               

                if (short.TryParse(ffr.MotoOne, out m1) == false) {
                    m1Status = ffr.MotoOne;
                    
                }
                if (short.TryParse(ffr.MotoTwo, out m2) == false) {
                    m2Status = ffr.MotoTwo;
                }

                bool saved = _raceDataController.CreateRaceResult(ffr.RaceClassMemberKey, raceKey, m1, m1Status, m2, m2Status, ffr.Position, ffr.RidingNumber, ffr.Brand);
             
            }



        }


        private static int ConvertToInt(object value) {
            if (value == null) {
                return 0;
            } else {
                string rVal = value.ToString();
                return int.Parse(rVal);
            }

        }

        private static short ConvertToShort(object value) {
            if (value == null) {
                return 0;
            } else {
                string rVal = value.ToString();
                return short.Parse(rVal);
            }

        }

        private static string ConvertToString(object value) {
            if (value == null) {
                return "";
            } else {
                return value.ToString();
            }

        }
    }
}
