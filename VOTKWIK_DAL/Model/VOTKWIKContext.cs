using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VOTKWIK_DAL.Entities;

namespace VOTKWIK_DAL.Model
{
    public class VOTKWIKContext : DbContext
    {
        public VOTKWIKContext() : base("TestContext")
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserDetail> UserDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserSystemDetail> UserSystemDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<VoterDetail> VoterDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<CandidateDetail> CandidateDetails { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DbSet<CandidateBallotDetail> CandidateBallotDetails { get; set; }
    }
}
