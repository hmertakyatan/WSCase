using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSoftCase.Dtos.Requests
{
    public class CategoryRequest
    {
        public required String CategoryName { get; set; }
        public required String CategoryIcon { get; set; }

    }
}