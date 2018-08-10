
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VOTKWIK_DAL.Entities
{
    public class VoterDetail
    {
        [Key]
        public long VoterDetailId { get; set; }

        public long UserId { get; set; }

        public long UserSystemDetailID { get; set; }

        public bool IsActive { get; set; }


        [ForeignKey("UserSystemDetailID")]
        public virtual UserSystemDetail UserSystemDetails { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
