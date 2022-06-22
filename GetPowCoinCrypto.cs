using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Alterra.PowCoin.Api.PaymentToken
{
    public static class PowCrypto
    {
        [FunctionName("PowCrypto")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Generating blockchain snowflakes.");

            string quantity = req.Query["quantity"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            quantity = quantity ?? data?.quantity;

            string responseMessage = "Welcome to the lastest cryptocurrency for shredders. Try adding 'quantity' in the query string to get ur coin.";

            if (string.IsNullOrEmpty(quantity))
            {
                return new OkObjectResult(responseMessage);
            }


            int quantityInt = 0;

            try
            {
                quantityInt = Int32.Parse(quantity);
                Console.WriteLine(quantityInt);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse the quantity of PowCoins '{quantity}'");
                log.LogInformation($"Unable to parse the quantity of PowCoins '{quantity}'");
            }
            

            string coinEmojis = "";

            if (quantityInt > 0)
            {
                for (int i = 0; i < quantityInt; i++) 
                {
                    coinEmojis += "\n💰❄";
                }
            }

            responseMessage = $"Kapow! Here's {quantity} PowCoins for you: {coinEmojis}";

            return new OkObjectResult(responseMessage);
        }
    }
}