using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMXModelsLib;

namespace MMXServiceInterfaceLib {
    public interface IRaceClassMemberService {
        void CreateRaceClass(RaceClassModel raceClassModel);
        void UpdateRaceClass(RaceClassModel raceClassModel);
        RaceClassModels GetAllRaceClasses();
        void DisableRaceClass(int raceClassKey);
        bool OpenEnrollmentForNextYear();
        bool CloseEnrollmentForCurrentYear();
        void ResetEnrollmentYear(short year, bool isOpen);
        short GetCurrentEnrollmentYear();
        bool CreateRaceClassMember(int raceClassKey, int contactKey, int memberMotorcycleKey, bool hasPaid, StringBuilder sb);
        bool IsNumberUsedInClass(int raceClassKey, string ridingNumber);
        void UpdateRaceClassMemberRide(int raceClassMemberKey, int newMotorcycleKey);
        void UpdateRaceClassMemberHasPaid(int raceClassMemberKey, bool hasPaid);
        string DeleteRaceClassMember(int raceClassMemberKey);        
        short GetRaceMemberAge(int memberContactKey);
        IEnumerable<RaceClassMemberModel> GetRaceClassMemberViews(int raceClassKey, short year);
        RaceClassMemberModel GetRaceClassMemberView(int raceClassMemberKey);
        RaceClassModels GetRaceClassesForMember(int memberContactKey);
        RaceClassModel GetRaceClass(int raceClassKey);
        IEnumerable<RaceClassDisplayModel> GetActiveRaceClasses();
        EnrollmentModel GetEnrollment();
        //bool CreateImportedRaceClassMember(string raceClassName, int memberKey, short year);

    }
}
