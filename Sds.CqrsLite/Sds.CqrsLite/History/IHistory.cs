using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sds.CqrsLite.History
{
    public interface IHistory
    {
        Task<IEnumerable<IEvent>> GetHistory(string streamName);
    }
}
