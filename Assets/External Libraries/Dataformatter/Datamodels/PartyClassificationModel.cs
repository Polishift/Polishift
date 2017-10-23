namespace Dataformatter.Datamodels
{
    public class PartyClassificationModel : IModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Classification { get; set; }
        public string CountryName { get; set; }
    }
}