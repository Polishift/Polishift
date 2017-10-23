using System;
using System.Collections.Generic;
using Dataformatter.Datamodels;
using Dataformatter.Data_accessing.Factories.ModelFactories;

namespace Dataformatter.Dataprocessing.Parsing
{
    public static class CsvToModel<T> where T : IModel
    {
        public static List<T> ParseAllCsvLinesToModels(string fileLocation, ICsvModelFactory<T> modelFactory)
        {
            var allRowsAsModels = new List<T>();

            var counter = 0; //le header skip
            var currentTextRow = new List<string>();
            Console.WriteLine("Now parsing: " + fileLocation);
            
            using (var reader = new CsvFileReader(fileLocation))
            {
                while (reader.ReadRow(currentTextRow)) //out arguments, wow nice programming brah
                {
                    counter++;

                    if (counter > 1)
                        allRowsAsModels.Add(modelFactory.Create(currentTextRow));
                }
            }
            return allRowsAsModels;
        }
    }
}