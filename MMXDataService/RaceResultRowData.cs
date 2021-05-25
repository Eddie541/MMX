using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMXDataService {
    public class RaceResultRowData {
        public string ClassName { get; set; }

        public string Member { get; set; }

        public string RidingNumber { get; set; }

        public string Brand { get; set; }

        public short Position { get; set; }

        public int TotalPoints { get; set; }

        public string MotoOne { get; set; }

        public string MotoTwo { get; set; }

        public int RacePoints { get; set; }

        public int RaceClassMemberKey {get; private set;}

        public int MemberKey { get; private set; }

        public void SetRaceClassMemberKey(int key) {
            RaceClassMemberKey = key;
        }

        public void SetMemberKey(int memberKey) {
            MemberKey = memberKey;
        }

        public override string ToString() {
            return ClassName + " " +
                Member + " " +
                RidingNumber + " " +
                Brand + " " +
                Position + " " +
                TotalPoints + " " +
                MotoOne + " " +
                MotoTwo + " " +
                RacePoints;
        }
    }
}
