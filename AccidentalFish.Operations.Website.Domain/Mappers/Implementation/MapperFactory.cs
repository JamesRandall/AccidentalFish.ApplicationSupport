using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Alerts.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.Operations.Website.Domain.Mappers.Implementation
{
    internal class MapperFactory : IMapperFactory
    {
        public IMapper<AlertSubscriber, ViewModel.AlertSubscriber> GetAlertSubscriberMapper()
        {
            throw new NotImplementedException();
        }
    }
}
