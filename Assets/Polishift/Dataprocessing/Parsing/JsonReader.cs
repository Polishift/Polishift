using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Assets.Dataprocessing.CsvParsing
{
    public static class JsonReader<T>
    {
        public static T[] ParseJsonToListOfObjects(string fileLocation)
        {
            JObject rootObject = JObject.Parse(File.ReadAllText(fileLocation));
            JArray rootArray = (JArray) rootObject[""];

            T[] objectList = rootArray.ToObject<T[]>();
            return objectList;
        }
    }
}
