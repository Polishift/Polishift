namespace Dataformatter.Dataprocessing.Entities
{
    public class PartyClassificationEntity : IEntity
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; }
    }
}