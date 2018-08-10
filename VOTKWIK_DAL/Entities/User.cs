using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VOTKWIK_DAL.Entities
{
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        public User()
        {
            UserDetails = new List<UserDetail>();
            UserSystemDetails = new List<UserSystemDetail>();
            CandidateDetails = new List<CandidateDetail>();
            VoterDetails = new List<VoterDetail>(); 
        }
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AdharNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCandidate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<UserDetail> UserDetails { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<UserSystemDetail> UserSystemDetails { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<CandidateDetail> CandidateDetails { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<VoterDetail> VoterDetails { get; set; }
    }
}
