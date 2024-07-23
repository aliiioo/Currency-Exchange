﻿using Application.Contracts.Persistence;
using Quartz;

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
