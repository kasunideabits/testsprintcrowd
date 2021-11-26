using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.ScrowdUser.Dtos
{
    public class ViewUserProfileDto
    {
        /// <summary>
        /// gets or set IsViewMap value.
        /// </summary>
        public bool IsViewMap { get; set; }
        /// <summary>
        /// gets or set UserShareType value.
        /// </summary>
        public int UserShareType { get; set; }

    }
}
