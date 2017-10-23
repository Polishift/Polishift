using System.Collections.Generic;
using System.Linq;
using Dataformatter.Dataprocessing.Parsing;
using Newtonsoft.Json;

namespace Dataformatter.Data_accessing.Repositories
{
    public static class Iso3166Repository 
    {
        private const string FileName = "countries.json";

        private static readonly Iso3166Country[] Collection =
            JsonReader<Iso3166Country>.ParseJsonToListOfObjects(FileName);

        /// <summary>
        /// Obtain ISO3166-1 Country based on its alpha3 code.
        /// </summary>
        /// <param name="alpha3"></param>
        /// <returns></returns>
        public static Iso3166Country FromAlpha3(string alpha3)
        {
            return Collection.FirstOrDefault(p => p.Alpha3 == alpha3);
        }

        /// <summary>
        /// Obtain ISO3166-1 Country based on its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Iso3166Country FromName(string name)
        {
            return Collection.FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// Obtain ISO3166-1 Country based on its alpha2 code.
        /// </summary>
        /// <param name="alpha2"></param>
        /// <returns></returns>
        public static Iso3166Country FromAlpha2(string alpha2)
        {
            return Collection.FirstOrDefault(p => p.Alpha2 == alpha2);
        }

        /// <summary>
        /// Obtain ISO3166-1 Country based on alternative names for the country.
        /// </summary>
        /// <param name="alternative"></param>
        /// <returns></returns>
        public static Iso3166Country FromAlternativeName(string alternative)
        {
            for (var i = 0; i < Collection.Length; i++)
            {
                var item = Collection.ElementAt(i);
                if (item.AlternativeNames == null) continue;
                if (item.AlternativeNames.Contains(alternative))
                    return item;
            }
            return null;
        }
    }

    /// <summary>
    /// Representation of an ISO3166-1 Country
    /// </summary>
    public class Iso3166Country
    {
        public Iso3166Country(string name, string alpha2, string alpha3)
        {
            Name = name.ToLower();
            Alpha2 = alpha2;
            Alpha3 = alpha3;
        }

        //needed for parsing countries.json
        [JsonConstructor]
        public Iso3166Country(string name, string alpha2, string alpha3,
            List<string> alternativeNames)
        {
            Name = name.ToLower();
            Alpha2 = alpha2;
            Alpha3 = alpha3;
            AlternativeNames = alternativeNames;
        }

        public string Name { get; }

        public string Alpha2 { get; }

        public string Alpha3 { get; }

        public List<string> AlternativeNames { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}