using Application.Dtos.ErrorsDtos;
using Application.Dtos.TransactionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence
{
    public interface ITestTransaction
    {

        public Task<ResultDto> ConfirmTransactionAsyncFalt1(int transactionId, string userId);
        public Task<ResultDto> ConfirmTransactionAsyncFalt2(int transactionId, string userId);
        public Task<ResultDto> ConfirmTransactionAsyncFalt3(int transactionId, string userId);
        public Task<ResultDto> ConfirmTransactionAsyncFalt4(int transactionId, string userId);
        public Task<ResultDto> ConfirmTransactionAsyncFalt5(int transactionId, string userId);
        public Task<ResultDto> ConfirmTransactionAsyncTrue(int transactionId, string userId);
        public Task<int> TransformCurrencyTest( string username);
    }
}
