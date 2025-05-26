using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSoftCase.Dtos.Requests
{
    public record ProductRequest
    {
        public required String ProductName { get; set; }
        public required String ProductIcon { get; set; }
        public required Guid CategoryId { get; set; }
    }
}