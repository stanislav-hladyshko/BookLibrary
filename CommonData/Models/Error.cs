﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CommonData.Models
{
        public class Error
        { 
            /// <summary>
            /// Gets or Sets Code
            /// </summary>
            [Required]
            [DataMember(Name="code")]
            public int? Code { get; set; }

            /// <summary>
            /// Gets or Sets Message
            /// </summary>
            [Required]
            [DataMember(Name="message")]
            public string Message { get; set; }
        }
    
}