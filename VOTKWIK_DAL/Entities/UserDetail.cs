using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VOTKWIK_DAL.Entities
{
    public class UserDetail
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public long UserDetailId { get; set; }

        public long UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

            

        public bool IsActive { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ContactNumber { get; set; }


        
    }
}
