using Assets.Data.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Data.Factories
{
    public interface IModelFactory<T>
    {
        T Create(List<string> csvRow); 
    }
}
