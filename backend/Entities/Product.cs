using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Entities.Common;

namespace WorkSoftCase.Entities
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        public required String ProductName { get; set; }
        public required String ProductIcon { get; set; }
        public required Guid CategoryId { get; set; }

    }
}