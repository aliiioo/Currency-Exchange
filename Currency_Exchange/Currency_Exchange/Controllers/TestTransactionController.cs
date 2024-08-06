using Application.Contracts.Persistence;
using Application.Statics;
using Infrastructure.Repositories.Persistence;
using Infrastructure.Repositories.Persistence.NewFolder;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Exchange.Controllers
{
    public class TestTransactionController: Controller
    {

        private readonly ITestTransaction _test;

        public TestTransactionController(ITestTransaction test)
        {
            _test = test;
        }

        public async Task<IActionResult> TestError1()
        {
            var transactionId =await _test.TransformCurrencyTest(User.GetUserId());
            await _test.ConfirmTransactionAsyncFalt1(transactionId,User.GetUserId());
            return null;
        }
        public async Task<IActionResult> TestError2()
        {
            var transactionId = await _test.TransformCurrencyTest(User.GetUserId());
            await _test.ConfirmTransactionAsyncFalt2(transactionId, User.GetUserId());
            return null;
        }
        public async Task<IActionResult> TestError3()
        {
            var transactionId = await _test.TransformCurrencyTest(User.GetUserId());
            await _test.ConfirmTransactionAsyncFalt3(transactionId, User.GetUserId());
            return null;
        }
        public async Task<IActionResult> TestError4()
        {
            var transactionId = await _test.TransformCurrencyTest(User.GetUserId());
            await _test.ConfirmTransactionAsyncFalt4(transactionId, User.GetUserId());
            return null;
        }
        public async Task<IActionResult> TestError5()
        {
            var transactionId = await _test.TransformCurrencyTest(User.GetUserId());
            await _test.ConfirmTransactionAsyncFalt5(transactionId, User.GetUserId());
            return null;
        }

        public async Task<IActionResult> TestError6()
        {
            var transactionId = await _test.TransformCurrencyTest(User.GetUserId());
            await _test.ConfirmTransactionAsyncTrue(transactionId, User.GetUserId());
            return null;
        }

    }
}
