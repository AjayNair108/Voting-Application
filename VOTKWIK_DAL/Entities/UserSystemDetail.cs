using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VOTKWIK_DAL.Entities
{
    public class UserSystemDetail
    {
        [Key]
        public long UserSystemDetailID { get; set; }

        public long UserId { get; set; }

        public string VotingBallotName { get; set; }

        public string VotingBallotInfo { get; set; }

        public string TokenNoBroadCast { get; set; }

        public bool IsLocalAdmin { get; set; }


        public DateTime StarteDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
