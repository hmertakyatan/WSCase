using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Entities.Common;

namespace WorkSoftCase.Entities
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        public required String CategoryName { get; set; }
        public required String CategoryIcon { get; set; }
       
    }
}