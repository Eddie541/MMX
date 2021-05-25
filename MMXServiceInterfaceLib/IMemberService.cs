using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMXModelsLib;


namespace MMXServiceInterfaceLib {
    public interface IMemberService {
        MemberModel GetMemberSummary(int memberKey);
        bool IsAdministrator(int memberKey);
        bool IsSuperUser(int memberKey);
        bool IsValidUser(int memberKey);
        RoleModel GetMemberRole(int memberKey);
        List<MemberModel> GetMemberSummaries();
        int CreateAddress(AddressModel addressModel);
        bool UpdateAddress(AddressModel addressModel);
        AddressModel GetAddress(int addressKey);
        void CreateMember(MemberModel member);
        void UpdateMember(MemberModel member);
        MemberModel GetMember(int memberKey);
        void CreateMotorcycle(MotorcycleModel motorcycleModel);
        void UpdateMotorcycle(MotorcycleModel motorcycleModel);
        bool DeleteMotorcycle(int memberContactKey, int motorcycleKey);
        IEnumerable<MotorcycleModel> GetMemberMotorcycles(int memberContactKey);
        MotorcycleModel GetMemberMotorcycle(int motorcycleKey, int memberContactKey);
        List<MemberDisplayModel> GetMembers(short year);



    }
}
