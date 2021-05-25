using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.Objects;

namespace MMXDataLib {
    public class RaceClassDataController : BaseDataController {


        public RaceClassDataController(string testConn = "") :
            base(testConn) {
        }

        public void CreateRaceClass(string className, short minAge, string note) {
            RaceClass raceClass = new RaceClass() {
                ClassName = className,
                MinumumAge = minAge,
                Note = note,                
                Enabled = true
            };

            context.RaceClasses.AddObject(raceClass);
            context.SaveChanges();
        }

        //public void CreateNextYearsRaceClassses(short previousYear, short nextYear) {
        //    IEnumerable<RaceClass> raceClasses = context.RaceClasses.Where(r => r.Year == previousYear);
        //    foreach (RaceClass raceClass in raceClasses) {
        //        CreateRaceClass(raceClass);            
        //    }
        //}

        //private void CreateRaceClass(RaceClass newRaceClass) {
        //    RaceClass raceClass = new RaceClass() {
        //        ClassName = newRaceClass.ClassName,
        //        MinumumAge = newRaceClass.MinumumAge,
        //        Note = newRaceClass.Note,
        //        Year = newRaceClass.Year,
        //        Enabled = true
        //    };

        //    context.RaceClasses.AddObject(raceClass);
        //    context.SaveChanges();
        //}

        public void UpdateRaceClass(int raceClassKey, string className, short minAge, string note) {
            RaceClass raceClass = context.RaceClasses.Where(r => r.RaceClassKey == raceClassKey).Single();
            raceClass.ClassName = className;
            raceClass.MinumumAge = minAge;
            raceClass.Note = note;            
            context.SaveChanges();
        }

        public void DisableRaceClass(int raceClassKey) {
            RaceClass raceClass = context.RaceClasses.Where(r => r.RaceClassKey == raceClassKey).Single();
            raceClass.Enabled = false;
            context.SaveChanges();
        }


        public bool OpenEnrollmentForNextYear() {
            bool enrollmentOpened = false; // still open, close previous year first
            Enrollment enrollment =  context.Enrollments.Where(e => e.EnrollmentKey == 1).Single();
            if (enrollment.EnrollmentOpen == false) {
                short currentYear = enrollment.Year;
                short nextYear = ++currentYear;
                enrollment.Year = nextYear;               
                enrollment.EnrollmentOpen = true;
                enrollment.DropPoints = false;
                context.SaveChanges();
                enrollmentOpened = true;
            }

            return enrollmentOpened;
        }       


        public bool CloseEnrollmentForCurrentYear() {
            bool enrollmentClosed = false; //already closed can't close again
            Enrollment enrollment = context.Enrollments.Where(e => e.EnrollmentKey == 1).Single();
            if (enrollment.EnrollmentOpen == true) {                
                enrollment.EnrollmentOpen = false;
                context.SaveChanges();
                enrollmentClosed = true;
            }
            return enrollmentClosed;
        }

        public void ResetEnrollmentYear(short year, bool isOpen) {
            Enrollment enrollment = context.Enrollments.Where(e => e.EnrollmentKey == 1).Single();
            enrollment.Year = year;
            enrollment.EnrollmentOpen = isOpen;
            context.SaveChanges();
        }

        public void SetPointsDropForEnrollmentYear() {
            Enrollment enrollment = context.Enrollments.Where(e => e.EnrollmentKey == 1).Single();
            enrollment.DropPoints = true;
            context.SaveChanges();
        }      


        public short CurrentEnrollmentYear {
            get {
                Enrollment enrollment = context.Enrollments.Where(e => e.EnrollmentKey == 1).Single();
                return enrollment.Year;
            }
        }

        public Enrollment GetEnrollment() {
            Enrollment enrollment = context.Enrollments.Where(e => e.EnrollmentKey == 1).Single();
            return enrollment;
        }


        public int CreateImportedRaceClassMember(string raceClassName, int memberKey, int year) {
            int raceClassMemberKey = 0;

            int raceClassKey = GetRaceClassKey(raceClassName);

            if (raceClassKey == 0) {
                return 0;
            }

            RaceClassMember existingMember = context.RaceClassMembers.Where(rc => rc.MemberKey == memberKey && rc.RaceClassKey == raceClassKey && rc.Year == year).SingleOrDefault();
            if (existingMember != null) {
                raceClassMemberKey = existingMember.RaceClassMemberKey;
                return raceClassMemberKey;
            }            

            RaceClass raceClass = context.RaceClasses.Where(rc => rc.RaceClassKey == raceClassKey).Single();

            // todo reenable this check
            //int? minAge = raceClass.MinumumAge;           

            //short memberAge = GetRaceMemberAge(memberKey, true);

            //if ((memberAge >= minAge) == false) {
            //    // todo return exception..?
            //    return 0;
            //}

            using (TransactionScope scope = new TransactionScope()) {
                RaceClassMember classMember = new RaceClassMember() {
                    RaceClassKey = raceClassKey,
                    MemberKey = memberKey,
                    Year = (short) year,
                    HasPaid = true
                };

                context.RaceClassMembers.AddObject(classMember);
                context.SaveChanges();

                scope.Complete();
                raceClassMemberKey = classMember.RaceClassMemberKey;
            }

            return raceClassMemberKey;

        }
        
        public bool CreateRaceClassMember(int raceClassKey, int memberKey, int memberMotorcycleKey, bool hasPaid, StringBuilder sb) {
            bool success = false;           
            short year = 0;
            ErrorCollection.Clear();
            if (context.Enrollments.Any(e => e.EnrollmentOpen == true)) {
                Enrollment enrollment = context.Enrollments.Where(en => en.EnrollmentOpen).First();
                year = enrollment.Year;

                RaceClass raceClass = context.RaceClasses.Where(rc => rc.RaceClassKey == raceClassKey).Single();

                int? minAge = raceClass.MinumumAge;

                bool alreadyIsMember = context.RaceClassMembers.Any(rc => rc.MemberKey == memberKey && rc.RaceClassKey == raceClassKey && rc.Year == year);
                if (alreadyIsMember) {
                    ErrorCollection.Add(string.Format("You are currently a member of this race class for year {0}", year));
                    //sb.AppendLine(string.Format("You are currently a member of this race class for year {0}", year));
                    return success;
                }

                short memberAge = GetRaceMemberAge(memberKey, true);
                if ((memberAge >= minAge) == false) {
                    ErrorCollection.Add(string.Format("Member age is less than the minimum age for this class {0}", minAge));
                    return success;
                }

                //Motorcycle motorcycle = context.Motorcycles.Where(mm => mm.MotorcycleKey == memberMotorcycleKey).First();
                //string ridingNumber =  motorcycle.RidingNumber;
                //if (IsNumberUsedInClass(raceClassKey,ridingNumber)) {
                //    ErrorCollection.Add(string.Format("The riding number '{0}' is already used in the this class, please update your motorcycle riding number for this class", ridingNumber));
                //    return success;
                //}               

                using (TransactionScope scope = new TransactionScope()) {
                    RaceClassMember classMember = new RaceClassMember() {
                        RaceClassKey = raceClassKey,
                        MemberKey = memberKey,
                        Year = year,
                        HasPaid = hasPaid
                    };

                    context.RaceClassMembers.AddObject(classMember);
                    context.SaveChanges();

                    //int raceClassMemberKey = classMember.RaceClassMemberKey;
                    //MemberMotorcycle memberMotorcycle = context.MemberMotorcycles.Where(mm => mm.MemberMotorcycleKey == memberMotorcycleKey).Single();
                    //memberMotorcycle.RaceClassMemberKey = raceClassMemberKey;
                    //context.SaveChanges();
                    scope.Complete();
                    success = true;
                }

            } else {
                ErrorCollection.Add("Enrollment is not open");
            }
            return success;
        }

        public bool IsNumberUsedInClass(int raceClassKey, string ridingNumber) {
            // todo test these joins
            IEnumerable<RaceClassMember> raceClassMembers = from rc in context.RaceClassMembers
                                                            //join mm in context.MemberMotorcycles on
                                                            //rc.RaceClassMemberKey equals mm.RaceClassMemberKey
                                                            //join mc in context.Motorcycles on
                                                            //mm.MotorcycleKey equals mc.MotorcycleKey
                                                            //where rc.RaceClassKey == raceClassKey &&
                                                            //mc.RidingNumber.Equals(ridingNumber, StringComparison.InvariantCultureIgnoreCase)
                                                            // todo add riding number
                                                            select rc;
            return raceClassMembers != null ? raceClassMembers.Count() > 0 : false;
        }

        // todo check if bike exists and belongs to member
        //public void UpdateRaceClassMemberRide(int raceClassMemberKey, int newMotorcycleKey) {
        //    MemberMotorcycle memberMotorcycle = context.MemberMotorcycles.Where(mm => mm.RaceClassMemberKey == raceClassMemberKey).Single();
        //    memberMotorcycle.MotorcycleKey = newMotorcycleKey;
        //    context.SaveChanges();
        //}      

        public void UpdateRaceClassMemberHasPaid(int raceClassMemberKey, bool hasPaid) {
            RaceClassMember classMember = context.RaceClassMembers.Where(r => r.RaceClassMemberKey == raceClassMemberKey).Single();
            classMember.HasPaid = hasPaid;
            context.SaveChanges();
        }

        public string DeleteRaceClassMember(int raceClassMemberKey) {            
            string message = "Member Deleted";            
            IEnumerable<RaceResult> memberResults = context.RaceResults.Where(rr => rr.RaceClassMemberKey == raceClassMemberKey);
            RaceClassMember classMember = context.RaceClassMembers.Where(r => r.RaceClassMemberKey == raceClassMemberKey).Single();
            if ((memberResults == null || memberResults.Count() < 1) && classMember.HasPaid == false) {                
                context.DeleteObject(classMember);
                context.SaveChanges();
            } else {
                message = "Member cannot be deleted, payment received or race results entered";
            }
            return message;
        }


        public IEnumerable<RaceClassMember> GetRaceClassMembers(int memberKey) {
            return context.RaceClassMembers.Where(rc => rc.MemberKey == memberKey).Select(s => s);
        }

        public short GetRaceMemberAge(int memberKey, bool yearAge = false) {  
            short age = 0;
            ObjectParameter parameter = new ObjectParameter("age", 0);
            int result = context.GetMemberContactAge(memberKey, yearAge, parameter);
            Int16.TryParse(parameter.Value.ToString(), out age);
            return age;

        }
        

        public IEnumerable<RaceClassMemberView> GetRaceClassMemberViews(int raceClassKey, short year) {            
            IEnumerable<RaceClassMemberView> members = context.RaceClassMemberViews.Where(r => r.RaceClassKey == raceClassKey && r.Year == year).OrderByDescending(r => r.AdjustedPointTotal).ThenBy(r => r.LastName);
            return members;
        }

        public RaceClassMemberView GetRaceClassMemberView(int raceClassMemberKey) {
           RaceClassMemberView member = context.RaceClassMemberViews.Where(r => r.RaceClassMemberKey == raceClassMemberKey).FirstOrDefault();
           return member;
        }

        public IEnumerable<RaceClass> GetMemberRaceClasses(int memberKey) {
            IEnumerable<RaceClass> raceClasses = from rc in context.RaceClasses 
                                                 join rcm in context.RaceClassMembers 
                                                 on rc.RaceClassKey equals rcm.RaceClassKey 
                                                 where rcm.MemberKey == memberKey select rc;

            return raceClasses;

        }

        private int GetRaceClassKey(string raceClassName) {
            RaceClass rc = context.RaceClasses.Where(r => r.ClassName.Equals(raceClassName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
            return rc == null ? 0 : rc.RaceClassKey;
        }

        public RaceClass GetRaceClass(int raceClassKey) {
            return context.RaceClasses.Where(rc => rc.RaceClassKey == raceClassKey).Single();
        }

        public IEnumerable<RaceClass> GetAllActiveRaceClasses() {
            return context.RaceClasses.Where(rc => rc.Enabled == true).OrderBy(s => s.ClassName);
        }

        public IEnumerable<RaceClass> GetAllRaceClasses() {
            return context.RaceClasses.Select(s => s).OrderBy(s => s.ClassName);
        }

        public IEnumerable<RaceClass> GetCurrentRaceClassesForMember(int memberKey, short year) {
            IEnumerable<RaceClass> raceClasses = from rc in context.RaceClasses
                                                 join rcm in context.RaceClassMembers on rc.RaceClassKey equals rcm.RaceClassKey
                                                 where rcm.MemberKey == memberKey && rcm.Year == year
                                                 select rc;
            return raceClasses;


        }

    }
}
