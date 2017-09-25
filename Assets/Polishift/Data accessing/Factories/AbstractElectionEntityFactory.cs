using Assets.Data.Repositories.Models;
using Assets.Dataprocessing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Data_accessing.Factories
{
    abstract class AbstractElectionEntityFactory
    {
        public abstract ElectionEntity Create(ConstituencyElectionModel rawModel);
    }
}
