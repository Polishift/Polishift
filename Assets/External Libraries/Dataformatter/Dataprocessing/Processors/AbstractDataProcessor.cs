using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;


namespace Dataformatter.Dataprocessing.Processors
{
    public enum EntityNames
    {
        PartyClassification,
        Election,
        Turnout,
        CountryBorders
    }

    public abstract class AbstractDataProcessor<I, O> where I : IModel
                                                      where O : IEntity
    {
        private const string Directory = "ProcessedData/";

        public abstract void SerializeDataToJson(List<I> rawModels);

        protected void WriteEntitiesToJson(EntityNames entityName, List<O> entities)
        {
            var orderedByCoutry = SortByCountry(entities);
            foreach (var countryPair in orderedByCoutry)
            {
                CreateDirectoryIfNotExists();
                var resultFileName = CreateCurrentCountryFileName(entityName, countryPair.Key);

                SerializeFile(resultFileName, countryPair.Value);
            }
        }

        private static Dictionary<string, List<O>> SortByCountry(List<O> allEntities)
        {
            var result = new Dictionary<string, List<O>>();
            for (var i = 0; i < allEntities.Count; i++)
            {
                var entity = allEntities[i];

                if (!result.ContainsKey(entity.CountryCode))
                    result.Add(entity.CountryCode, new List<O>());

                result[entity.CountryCode].Add(entity);
            }
            return result;
        }


        private static void CreateDirectoryIfNotExists()
        {
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);
        }

        private static string CreateCurrentCountryFileName(EntityNames entityName, string countryName)
        {
            return Directory + Enum.GetName(typeof(EntityNames), entityName) + "_" + countryName 
                             + ".json";
        }

        private static void SerializeFile(string fileName, List<O> entityList)
        {
            if (!File.Exists(fileName))
                File.Create(fileName).Dispose();

            using (var file = File.CreateText(fileName))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, entityList);
            }
        }
    }
}