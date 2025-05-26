using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Entities;
using WorkSoftCase.Repository.Interfaces.IRepository;

namespace WorkSoftCase.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        //TODO : Kategoriye ait productlar istenirse method yazılabilir
    }
}