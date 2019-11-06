using EaseFlight.Common.Constants;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.Web.WebUtilities
{
    public class PaypalUtility
    {
        //Variables for storing the clientID and clientSecret key
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        private static Payment payment;

        //Constructor
        static PaypalUtility()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            // getting accesstocken from paypal               
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();

            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken())
            {
                Config = GetConfig()
            };

            return apiContext;
        }

        public static Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment()
            {
                id = paymentId
            };

            return payment.Execute(apiContext, paymentExecution);
        }

        public static Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var itemList = new ItemList() { items = new List<Item>() };
            var booking = SessionUtility.GetBookingSession();
            var depart = "(" + booking.Departure.Name.Split('-')[0].Trim() + ") " + booking.Departure.City;
            var arrival = "(" + booking.Arrival.Name.Split('-')[0].Trim() + ") " + booking.Arrival.City;
            var percent = booking.PassengerType.Where(type => type.Name.Equals(Constant.CONST_DB_NAME_INFANT)).Select(type => type.Discount.Value).FirstOrDefault();
            var priceInfant = Math.Round(booking.Price - (booking.Price / 100) * percent, 2);
            var percent2 = booking.PassengerType.Where(type => type.Name.Equals(Constant.CONST_DB_NAME_CHILD)).Select(type => type.Discount.Value).FirstOrDefault();
            var priceChild = Math.Round(booking.Price - (booking.Price / 100) * percent2, 2);
            var totalPrice = (booking.Price * booking.Adult) + (booking.Child > 0 ? priceChild * booking.Child : 0) + (booking.Infant > 0 ? priceInfant * booking.Infant : 0);

            itemList.items.Add(new Item()
            {
                name = "Flight: " + depart + " to " + arrival + (booking.ReturnFlight.Count > 0? " (Round trip)":""),
                currency = "USD",
                price = totalPrice.ToString(),
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = totalPrice.ToString()
            };

            var transactionList = new List<Transaction>
            {
                new Transaction()
                {
                    description = "Booking Flight from EaseFlight",
                    invoice_number = Convert.ToString((new Random()).Next(100000)),
                    amount = amount,
                    item_list = itemList
                }
            };

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);
        }

        public static bool RefundPayment(APIContext apiContext, string paymentId, double price)
        {
            try
            {
                var payment = Payment.Get(apiContext, paymentId);
                var sale = payment.transactions[0].related_resources[0].sale;

                var refund = new Refund()  //create new refund to be sent to paypal
                {
                    amount = new Amount()
                    {
                        currency = "USD",
                        total = string.Format("{0:0.00}", price)
                    }
                };

                apiContext.ResetRequestId();

                #pragma warning disable CS0618
                var response = sale.Refund(apiContext, refund);
                #pragma warning restore CS0618

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}