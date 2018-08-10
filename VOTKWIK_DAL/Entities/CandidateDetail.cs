using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VOTKWIK_DAL.Entities
{
    public class CandidateDetail
    {
        [Key]
        public long CandidateDetailId { get; set; }

        public long UserId { get; set; }

        public long CandidateID { get; set; }

        public string ImagePath { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
