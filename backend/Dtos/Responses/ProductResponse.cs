using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSoftCase.Dtos.Responses
{
    public record ProductResponse
    {
        public Guid Id { get; set; }
        public required String ProductName { get; set; }
        public required String ProductIcon { get; set; }


    }
}