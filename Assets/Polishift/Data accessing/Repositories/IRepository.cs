using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Data.Repositories
{
    interface IRepository<T>
    {
        T[] GetAll();
        T[] GetByYear(int year);
        T[] GetByCountry(string CountryCode); //Make a Country class 
    }
} 
