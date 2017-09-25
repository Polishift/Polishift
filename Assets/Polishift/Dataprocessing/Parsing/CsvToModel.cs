using Assets.Data.Factories;
using Mono.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Dataprocessing.Parsing
{
    public static class CsvToModel<T>
    {
        public static List<T> ParseAllCsvLinesToModels(string fileLocation, IModelFactory<T> modelFactory)
        {
            var allRowsAsModels = new List<T>();

            List<string> currentTextRow = new List<string>();
            using (var reader = new CsvFileReader(fileLocation))
            {
                while (reader.ReadRow(currentTextRow)) //out arguments, wow nice programming brah
                {
                    allRowsAsModels.Add(modelFactory.Create(currentTextRow));
                }
            }
            return allRowsAsModels;
        }
    }

}
