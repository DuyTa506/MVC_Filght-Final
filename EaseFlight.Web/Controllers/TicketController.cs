using EaseFlight.Web.WebUtilities;
using PayPal.Api;
using System;
using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    public class TicketController : Controller
    {
        #region Properties
        
        #endregion

        #region Constructors
        public TicketController()
        {
            
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SavePassenger()
        {
            return new JsonResult { ContentType = "text" };
        }

        [HttpGet]
        public ActionResult PaymentPaypal()
        {
            if (SessionUtility.GetBookingSession() == null) //or passengerSesion null
                return RedirectToAction("Index", "Home");

            APIContext apiContext = PaypalUtility.GetAPIContext();

            try
            {
                var payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    var baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Ticket/PaymentPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = PaypalUtility.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //Get id payment for refund
                    var idPayment = createdPayment.id;

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                            paypalRedirectUrl = lnk.href;
                    }

                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = PaypalUtility.ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                        return View("Failed");
                }
            }
            catch
            {
                return View("Failed");
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}