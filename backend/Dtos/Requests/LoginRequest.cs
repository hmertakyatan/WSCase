using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSoftCase.Dtos.Requests
{
    public record LoginRequest
    {
        public String Username { get; set; }
        public String Password { get; set; }
    }
}