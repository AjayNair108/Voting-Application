using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VOTKWIK_API.POCO
{
    public class CandidateDetailPOCO
    {
        public long UserId { get; set; }

        public string CandidateName { get; set; }

        public string ImagePath { get; set; }

        public bool IsActive { get; set; }

    }
}