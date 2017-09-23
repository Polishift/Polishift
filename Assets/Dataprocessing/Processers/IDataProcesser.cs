using Assets.Data.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Dataprocessing.Processers
{
    interface IDataProcesser<T>
    {
        void SerializeDataToJSON(List<T> rawModels);
    }
}
