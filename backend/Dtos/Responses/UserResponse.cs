using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSoftCase.Dtos.Responses
{
    public record UserResponse
    {
        public Guid Id { get; set; }
        public required String FirstName { get; set; }
        public required String LastName { get; set; }
        
    }
}