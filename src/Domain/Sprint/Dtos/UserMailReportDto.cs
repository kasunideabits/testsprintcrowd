using System;
using System.ComponentModel;

namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    /// <summary>
    /// UserMailReportDto Model.
    /// </summary>
    public class UserMailReportDto
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>full name.</value>
        [DisplayName("Full Name")]
        public string Name { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>country name</value>
        [DisplayName("Country Name")]
        public string Country { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>country code</value>
        [DisplayName("Country Code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>email</value>
        [DisplayName("Email")]
        public string Email { get; set; }

    }
}