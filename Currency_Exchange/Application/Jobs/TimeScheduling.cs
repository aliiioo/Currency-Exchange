using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Persistence;

namespace Application.Jobs
{
    [DisallowConcurrentExecution]
    public class TimeScheduling : IJob
    {
        private readonly IProviderServices _providerServices;

        public TimeScheduling(IProviderServices providerServices)
        {
            _providerServices = providerServices;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var transactions = await _providerServices.CanceledPendingTransactionsByTimePass(10);
        }
    }
}
