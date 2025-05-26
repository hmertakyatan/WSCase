using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Entities.Common;

namespace WorkSoftCase.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        
        public required String UserName { get; set; }
        public required String FirstName { get; set; }
        public required String LastName { get; set; }
        public required String UserPassword { get; set; }
        public DateTime? PasswordModifyDate { get; set; }
    }
}