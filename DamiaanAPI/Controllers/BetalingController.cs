using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DamiaanAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using DamiaanAPI.Data.DTOs;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DamiaanAPI.Controllers
{

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class BetalingController : Controller
    {
        private readonly ILoperRepository _loperRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly UserManager<IdentityUser> _userManager;


        public BetalingController(ILoperRepository loperRepository, IRouteRepository routeRepository, UserManager<IdentityUser> userManager)
        {
            _loperRepository = loperRepository;
            _routeRepository = routeRepository;
            _userManager = userManager;
        }


        [HttpPost]
        public ActionResult<BetaalDTO> InschrijvenEnGetBetaalLink(RouteLoperDTO routeLoperDTO)
        {

            var userName = _userManager.GetUserName(User);

            if(userName != null)
            {
                var loper = _loperRepository.GetByEmail(userName);
                Route route = _routeRepository.GetById(routeLoperDTO.RouteID);
                if (loper != null && route != null)
                {
                    var inschrijving = loper.GeregistreerdeRoutes.SingleOrDefault(r => r.RouteID == route.ID);
                    //checken of loper al geregistreerd is voor de route
                    if (inschrijving == null)
                    {
                        string linkCode = GetMd5Hash();
                        string orderId = GetSha1Hash(GetMd5Hash());
                        RouteLoper routeLoper = new RouteLoper
                        {
                            RouteID = route.ID,
                            LoperID = loper.ID,
                            GeregistreerdOp = DateTime.Now,
                            TshirtMaat = routeLoperDTO.TshirtMaat.Trim().Length == 1 ? routeLoperDTO.TshirtMaat.Trim() : "GEEN",
                            Zichtbaarheid = (Zichtbaarheid)routeLoperDTO.Zichtbaarheid,
                            LinkCode = linkCode,
                            Messages = new List<Message>(),
                            OrderId = orderId,
                            OrderStatus = "IN PROGRESS"
                        };

                        loper.GeregistreerdeRoutes.Add(routeLoper);
                        route.Lopers.Add(routeLoper);
                        _loperRepository.SaveChanges();
                        _routeRepository.SaveChanges();
                        //Nog meer onderscheid maken in de mail zelf: andere tekst
                        linkCode = routeLoper.Zichtbaarheid == Zichtbaarheid.Niet ? "" : linkCode;
                        return new BetaalDTO
                        {
                            Link = "https://damiaan.azurewebsites.net/api/betaling/" + orderId

                        };
                    }
                    if(inschrijving.OrderStatus.Trim() != "9")
                    {
                        //Loper al ingeschreven -> geef direct link naar betalen door
                        return new BetaalDTO
                        {
                            Link = "https://damiaan.azurewebsites.net/api/betaling/" + inschrijving.OrderId

                        };
                    }
                }
                
            }

            return NotFound();
        }

        [HttpGet]
        [AllowAnonymous]
        public void VerifieerBetalingResultaat([FromQuery] string orderID = "", [FromQuery] string currency = "", [FromQuery] int amount = 0, [FromQuery] int STATUS = 0, [FromQuery] string PAYID = "", [FromQuery] string NCERROR = "", [FromQuery] string SHASIGN = "")
        {
            var shaout = "f7ek+DrabrefrEd6";
            var concatString = "AMOUNT=" + amount + shaout +
                   "CURRENCY=" + currency + shaout +
                   "NCERROR=" + NCERROR + shaout +
                   "ORDERID=" + orderID + shaout +
                   "PAYID=" + PAYID + shaout +
                   "STATUS=" + STATUS + shaout;
            var hash = GetSha1Hash(concatString);

            if (hash.Equals(SHASIGN))
            {
                var loper = _loperRepository.GetByOrderId(orderID);
                var inschrijving = loper.GeregistreerdeRoutes.Where(rl => rl.OrderId == orderID).Single();
                if (loper != null && inschrijving != null)
                {
                    inschrijving.OrderStatus = STATUS.ToString();
                    _loperRepository.Update(loper);

                    if(STATUS == 9)
                    {
                        Mail.IngeschrevenRouteMail(inschrijving);
                    }

                }

            }

            HttpResponse response = HttpContext.Response;
            response.Clear();
            var responseContent = "<html>" +
                "<body onload='document.forms[\"form\"].submit()'>" +
                "<p>Redirecting...</p>" +
                $"<form name='form' action='https://angularfrontendd3static.z22.web.core.windows.net/betalingResult' method='get'>" +
                $"<input type='hidden' name='status' value='{STATUS}'>" +
                $"</form></body></html>";


            response.WriteAsync(responseContent);
        }

        [HttpGet("{orderId}")]
        [AllowAnonymous]
        public ActionResult<string> GaNaarBetaal(string orderId, [FromQuery] string taal = "nl_BE")
        {
            var loper = _loperRepository.GetByOrderId(orderId);
            var inschrijving = loper.GeregistreerdeRoutes.Where(rl => rl.OrderId == orderId).Single();
            var route = _routeRepository.GetById(inschrijving.RouteID);

            if(loper != null && inschrijving != null && route != null)
            {
                var PSPID = "damiaanactie";
                var ORDERID = orderId;
                //Prijs nog uit db halen
                var AMOUNT = route.Prijs * 100;
                AMOUNT += inschrijving.TshirtMaat != "GEEN" ? 15 * 100 : 0;
                var CURRENCY = "EUR";
                //Nog doen: taal van website
                var LANGUAGE = taal;
                var SHAIN = "s*aW2dr86U++ZaKU";
                var ACCEPTURL = "https://damiaan.azurewebsites.net/api/betaling";

                var concatString = "ACCEPTURL=" + ACCEPTURL + SHAIN +
                    "AMOUNT=" + AMOUNT.ToString() + SHAIN +
                    "CANCELURL=" + ACCEPTURL + SHAIN +
                    "CURRENCY=" + CURRENCY + SHAIN +
                    "DECLINEURL=" + ACCEPTURL + SHAIN +
                    "EXCEPTIONURL=" + ACCEPTURL + SHAIN +
                    "LANGUAGE=" + LANGUAGE + SHAIN +
                    "ORDERID=" + ORDERID + SHAIN +
                    "PSPID=" + PSPID + SHAIN;

                var hash = GetSha1Hash(concatString);


                HttpResponse response = HttpContext.Response;
                response.Clear();


                var s = "<html>" +
                "<body onload='document.forms[\"form\"].submit()'>" +
                "<p>Redirecting...</p>" +
                "<form name='form' action='https://ogone.test.v-psp.com/ncol/test/orderstandard_utf8.asp' method='post'>" +
                $"<input type='hidden' name='PSPID' value='{PSPID}'>" +
                $"<input type='hidden' name='ORDERID' value='{ORDERID}'>" +
                $"<input type='hidden' name='AMOUNT' value='{AMOUNT}'>" +
                $"<input type='hidden' name='CURRENCY' value='{CURRENCY}'>" +
                $"<input type='hidden' name='LANGUAGE' value='{LANGUAGE}'>" +
                $"<input type='hidden' name='SHASIGN' value='{hash}'>" +
                $"<input type='hidden' name='ACCEPTURL' value='{ACCEPTURL}'>" +
                $"<input type='hidden' name='CANCELURL' value='{ACCEPTURL}'>" +
                $"<input type='hidden' name='DECLINEURL' value='{ACCEPTURL}'>" +
                $"<input type='hidden' name='EXCEPTIONURL' value='{ACCEPTURL}'>" +
                $"</form></body></html>";
                response.WriteAsync(s);


                return "Redirecting...";
            }

            return NotFound();
            
        }

        

        private string GetMd5Hash()
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString("ddMMyyyymmHHssFF")));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private string GetSha1Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
