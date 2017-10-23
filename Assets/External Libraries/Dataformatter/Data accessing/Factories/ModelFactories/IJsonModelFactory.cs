using Dataformatter.Datamodels;
using Newtonsoft.Json.Linq;

namespace Dataformatter.Data_accessing.Factories.ModelFactories
{
    
    public interface IJsonModelFactory<out T> where T : IModel
    {
        T Create(JObject jObject); 
    }
}
