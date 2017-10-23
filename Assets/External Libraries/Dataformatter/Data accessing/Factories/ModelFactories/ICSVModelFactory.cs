using System.Collections.Generic;
using Dataformatter.Datamodels;

namespace Dataformatter.Data_accessing.Factories.ModelFactories
{
    public interface ICsvModelFactory<T> where T : IModel
    {
        T Create(List<string> csvRow); 
    }
}