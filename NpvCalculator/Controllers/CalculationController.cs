using Microsoft.AspNetCore.Mvc;
using NpvCalculator.Models;

namespace NpvCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(NpvRequestModel request)
        {
            try
            {
                if (!Validate(request))
                {
                    return BadRequest();
                }
                List<Task<NpvModel>> npvTasks = new();

                for(decimal rate = request.LowerBoundRate; rate < (request.UpperBoundRate + request.Increment); rate += request.Increment)
                {
                    if(rate > request.UpperBoundRate) rate = request.UpperBoundRate;

                    // add each async method calls to Task list first to avoid blocking the execution flow
                    npvTasks.Add(CalculateNpv(request.CashFlows, rate, request.InitialAmount));
                }

                // await the list of async calculation method calls to get the list of npvs
                return Ok(await Task.WhenAll(npvTasks));
            }
            catch(Exception ex)
            {
                // Create logger for exceptions
                throw;
            }
        }

        /// <summary>
        /// Calculate the NPV using a single doscount rate
        /// </summary>
        /// <param name="cashFlows">Array of cashflow amount.</param>
        /// <param name="rate">Current active rate.</param>
        /// <param name="initialAmt">Initial cash flow amount.</param>
        /// <returns>Awaitable decimal value for the calculted npv.</returns>
        private async Task<NpvModel> CalculateNpv(decimal[] cashFlows, decimal rate, decimal initialAmt)
        {
            NpvModel model = new();
            decimal val = initialAmt * -1;

            // This part was created as Task to show asynchronous programming,
            // but in normal circumstances, this type of operation most likely will not require async processing
            await Task.Run(() => {
                for (int i = 0; i < cashFlows.Length; i++)
                    val += (cashFlows[i] / (decimal)(Math.Pow(1 + decimal.ToDouble(rate/100), i + 1)));
            });

            model.Npv = val;
            model.DiscountRate = rate;
            model.IsLoss = val < 0;

            return model;
        }

        /// <summary>
        /// Validate request for bad request
        /// </summary>
        /// <param name="request">Request sent to the api method.</param>
        /// <returns>Bool value indicating id the request is valid or not.</returns>
        private bool Validate(NpvRequestModel request)
        {
            bool isValid = request != null && request.CashFlows != null && request.CashFlows.Length > 0 
                && request.Increment > 0 && request.LowerBoundRate < request.UpperBoundRate;
            return isValid;
        }
    }
}
