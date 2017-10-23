using System;
using System.Collections.Generic;
using System.IO;
using Dataformatter.Datamodels;
using Dataformatter.Data_accessing.Factories.ModelFactories;
using Newtonsoft.Json.Linq;

namespace Dataformatter.Dataprocessing.Parsing
{
    public static class JsonToModel<T> where T : IModel
    {
        public static List<T> ParseJsonDirectoryToModels(string directoryPath, IJsonModelFactory<T> modelFactory,
            string pattern)
        {
            var filesInDirectory = Directory.GetFiles(directoryPath, pattern, SearchOption.AllDirectories);

            var resultList = new List<T>();
            for (var i = 0; i < filesInDirectory.Length; i++)
            {
                var currentFile = filesInDirectory[i];

                try
                {
                    var currentFileModels = ParseJsonFileToModel(currentFile, modelFactory);
                    resultList.AddRange(currentFileModels);

                }
                catch(Exception e)
                {
                    Console.WriteLine("Encountered unreadable file: " + currentFile);
                }
            }
            return resultList;
        }

        private static List<T> ParseJsonFileToModel(string fileLocation, IJsonModelFactory<T> modelFactory)
        {
            var result = new List<T>();

            var rootObject = JObject.Parse(File.ReadAllText(fileLocation));

            result.Add(modelFactory.Create(rootObject));


            return result;
        }
    }
}