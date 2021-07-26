using System;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    /// <summary>
    /// User info data transfer object
    /// </summary>
    public class RolesDto
    {
       
        public RolesDto()
        {

        }
        public RolesDto( int roleId , string rolesName)
        {
            this.RoleId = roleId;
            this.RoleName = rolesName;
            
        }

        /// <summary>
        /// Gets role id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets role name
        /// </summary>
        public string RoleName { get; set; }
    }
}