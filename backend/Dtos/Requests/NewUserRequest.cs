using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSoftCase.Dtos.Requests
{
    public class UserRequest
    {
        public required String UserName { get; set; }
        public required String Password { get; set; }
        public required String FirstName { get; set; }
        public required String LastName { get; set; }

    }
}