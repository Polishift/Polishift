using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Data.Repositories
{
    interface IRepository
    {
        Dictionary<string, string[]> GetAll();
        Dictionary<string, string[]> GetByCountry(string CountryCode); //Make a Country class 
        Dictionary<string, string[]> GetByYear(int year);

        //problem: How/Where to populate? 
        //In their constructors, calling a static CsvParser method for their file (which has a const name associated
        //with it in the json file)
    }
} 
