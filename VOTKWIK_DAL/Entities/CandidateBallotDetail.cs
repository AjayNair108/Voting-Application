using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VOTKWIK_DAL.Entities
{
    public class CandidateBallotDetail
    {
        [Key]
        public long CandidateBallotDetailId { get; set; }

        public long UserSystemDetailID { get; set; }


        public long CandidateDetailId { get; set; } 

        public int VoteCount { get; set; }


        public bool IsActive { get; set; }


        [ForeignKey("UserSystemDetailID")]
        public virtual UserSystemDetail UserSystemDetails { get; set; }



        [ForeignKey("CandidateDetailId")]
        public virtual CandidateDetail CandidateDetails { get; set; }
    }
}
