using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DamiaanAPI.Data.DTOs;
using DamiaanAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//TODO: Authorizatie: gebruiker kan enkel zijn eigen routes bekijken
namespace DamiaanAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class LoperController : ControllerBase
    {
        private readonly ILoperRepository _loperRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public LoperController(ILoperRepository loperRepository, IRouteRepository routeRepository, UserManager<IdentityUser> userManager)
        {
            _loperRepository = loperRepository;
            _routeRepository = routeRepository;
            _userManager = userManager;
        }

        // GET: api/RouteLoper/5
        /// <summary>
        /// Get loperinfo met loperID
        /// </summary>
        /// <returns>UserInfoDTO</returns>
        [HttpGet]
        [Authorize]
        public ActionResult<UserInfoDTO> GetLoper()
        {
            var userName = _userManager.GetUserName(User);
            var loper = _loperRepository.GetByEmail(userName);

            UserInfoDTO userInfoDTO = new UserInfoDTO()
            {
                Email = loper.Email,
                FirstName = loper.FirstName,
                LastName = loper.LastName,
                Gemeente = loper.Gemeente
            };

            List<RouteLoperDTO> Inschrijvingen = new List<RouteLoperDTO>();
            _loperRepository.GetByID(loper.ID).GeregistreerdeRoutes.ToList().ForEach(inschrijving =>
            {
                RouteLoperDTO rl = new RouteLoperDTO()
                {
                    EindDatumEnUur = inschrijving.EindDatumEnUur,
                    GeregistreerdOp = inschrijving.GeregistreerdOp,
                    StartDatumEnUur = inschrijving.StartDatumEnUur,
                    Zichtbaarheid = (int)inschrijving.Zichtbaarheid,
                    RouteNaam = inschrijving.Route.Naam,
                    RouteID = inschrijving.Route.ID,
                    LoperID = loper.ID
                };
                Inschrijvingen.Add(rl);
            });

            userInfoDTO.Inschrijvingen = Inschrijvingen;

            return userInfoDTO;
        }

        // GET: api/RouteLoper/Public/5
        /// <summary>
        /// Get publieke lopers met page aantal
        /// </summary>
        /// <param name="page">Mandatory page</param>
        /// <returns>Lijst van Lopers</returns>
        [AllowAnonymous]
        [HttpGet("public")]
        public IEnumerable<Loper> GetPublicLopers(int page = 1)
        {
            return _loperRepository.GetPublicLopers(page);
        }


        
        // GET: api/RouteLoper/Gent/Hoffman/Thibo
        /// <summary>
        /// Get hidden loper met gemeente, naam en voornaam
        /// </summary>
        /// <param name="gemeente">Mandatory gemeente</param>
        /// <param name="naam">Mandatory naam</param>
        /// <param name="voornaam">Mandatory voornaam</param>
        /// <returns>Loper</returns>
        [AllowAnonymous]
        [HttpGet("searchHidden")]
        public Loper SearchHiddenLopers(string voornaam, string naam, string gemeente)
        {
            return _loperRepository.SearchHidden(voornaam, naam, gemeente);
        }

        // GET: api/RouteLoper/Chat/123ezfeko&gerko234
        /// <summary>
        /// Get chat met code
        /// </summary>
        /// <param name="code">Mandatory code die vasthangt aan user en waarmee tracking gedeeld kan worden</param>
        /// <returns>Lijst van MessageDTOs</returns>
        [AllowAnonymous]
        [HttpGet("{code}/chat")]
        public List<MessageDTO> GetChat(string code)
        {
            List<MessageDTO> messageDTOs = new List<MessageDTO>();
            /*
            MessageDTO message1 = new MessageDTO
            {
                Text = "Hey",
                Date = DateTime.Now,
                Zender = "Maarten"
            };
            MessageDTO message2 = new MessageDTO
            {
                Text = "Suc6",
                Date = DateTime.Now,
                Zender = "Natan"
            };
            
            messageDTOs.Add(message1);
            messageDTOs.Add(message2);
            */
            Loper l = _loperRepository.GetChat(code);
            RouteLoper rl = l.GeregistreerdeRoutes.Where(rl => rl.LinkCode == code).Single();

            rl.Messages.ForEach(m =>
            {
                MessageDTO mes = new MessageDTO(m);
                messageDTOs.Add(mes);
            });
            
            return messageDTOs;
        }

        // POST: api/RouteLoper/Chat/123ezfeko&gerko234/Message
        /// <summary>
        /// Voeg bericht toe
        /// </summary>
        /// <param name="code">Mandatory code van loper</param>
        /// <param name="message">Mandatory bericht die gestuurd wordt</param>
        /// <returns>ActionResult van MessageDTO</returns>
        [HttpPost("{code}/chat")]
        [AllowAnonymous]
        public ActionResult<MessageDTO> AddMessage(string code, MessageDTO m)
        {
            Loper l = _loperRepository.GetChat(code);
            RouteLoper rl = l.GeregistreerdeRoutes.Where(rl => rl.LinkCode == code).Single();
            Message message = new Message
            {
                Text = m.Text,
                Date = m.Date,
                Zender = m.Zender
            };
            if (l != null) {
                rl.Messages.Add(message);
                _loperRepository.SaveChanges();
                return m;
            }

            return NotFound();

        }


        // GET: api/RouteLoper/Public/
        /// <summary>
        /// Voeg bericht toe
        /// </summary>
        /// <param name="code">Mandatory code van loper</param>
        /// <param name="message">Mandatory bericht die gestuurd wordt</param>
        /// <returns>ActionResult van MessageDTO</returns>
        [AllowAnonymous]
        [HttpGet("searchPublic")]
        public IEnumerable<Loper> SearchPublicLopers(string voornaam, string naam, string gemeente)
        {
            return _loperRepository.SearchPublic(voornaam, naam, gemeente);
        }



        // GET: api/RouteLoper/Ingeschreven/2/124
        /// <summary>
        /// Check ofdat loper is ingeschreven of niet
        /// </summary>
        /// <param name="loperID">Mandatory loperID</param>
        /// <returns>ActionResult van Boolean</returns>
        [Authorize]
        [HttpGet("routes/{routeID}")]
        public ActionResult<Boolean> Ingeschreven(int routeID)
        {
            var userName = _userManager.GetUserName(User);
            var loper = _loperRepository.GetByEmail(userName);
            if (loper != null && loper.GeregistreerdeRoutes.SingleOrDefault(rl => rl.RouteID == routeID && rl.OrderStatus == "9") != null)
                return true;
            return false;
        }


        // GET: api/RouteLoper/code/123rgekgoe53rjog432
        /// <summary>
        /// Get huidige locatie van een loper met de code
        /// </summary>
        /// <param name="code">Mandatory code van loper</param>
        /// <returns>ActionResult van HuidigeLocatieDTO</returns>
        [AllowAnonymous]
        [HttpGet("{code}/locatie")]
        public ActionResult<HuidigeLocatieDTO> CurrentRouteIDAndLocationByCode(string code)
        {
            Loper l = _loperRepository.GetByLinkCode(code);
            if (l != null)
            {
                RouteLoper rl = l.GeregistreerdeRoutes.Where(rl => rl.LinkCode == code).Single();
                Route route = rl.Route;
                IList<decimal> huidigeLocatie = null;
                //TODO: calculate distance
                if (rl.LaatsteLocatieLon.HasValue && rl.LaatsteLocatieLat.HasValue)
                {
                    var coords = JsonConvert.DeserializeObject<GeoJson>(route.GeoJson).Geometry.Coordinates;
                    var afstanden = coords.Select(coord => BerekenAfstand((double)rl.LaatsteLocatieLat, (double)coord[1], (double)rl.LaatsteLocatieLon, (double)coord[0])).ToList();
                    huidigeLocatie = coords[afstanden.IndexOf(afstanden.Min())]; 
                }
                HuidigeLocatieDTO h = new HuidigeLocatieDTO
                {
                    RouteId = route.ID,
                    Voornaam = l.FirstName,
                    Achternaam = l.LastName,
                    Lat = huidigeLocatie?[1],
                    Lon = huidigeLocatie?[0]
                };
                return h;
            }
            return NotFound();
        }


        // GET: api/RouteLoper/124/huidig
        /// <summary>
        /// Get huidige route van loper (route die momenteel gewandeld wordt)
        /// </summary>
        /// <param name="loperID">Mandatory ID van loper</param>
        /// <returns>ActionResult van RouteDTO</returns>
        [HttpGet("{loperID}/huidig")]
        [AllowAnonymous]
        public ActionResult<RouteDTO> CurrentRoute(int loperID)
        {
            var routeId = GetCurrentRoute(loperID)?.ID;
            if (routeId == null)
                return NotFound();
            else
                return new RouteDTO(_routeRepository.GetById(routeId ?? 0));
        }

        // PUT: api/RouteLoper/124/routes
        /// <summary>
        /// Update de zichtbaarheid voor een inschrijving
        /// </summary>
        /// <param name="loperID">Mandatory ID van loper</param>
        /// <param name="routeLoperDTO">Mandatory RouteLoperDTO (= inschrijving)</param>
        /// <returns>ActionResult van RouteLoperDTO</returns>
        [Authorize]
        [HttpPut("routes")]
        public ActionResult UpdateInschrijving(RouteLoperDTO routeLoperDTO)
        {
            var userName = _userManager.GetUserName(User);
            var loper = _loperRepository.GetByEmail(userName);
            Route route = _routeRepository.GetById(routeLoperDTO.RouteID);
            if (loper != null && route != null)
            {
                var inschrijving = loper.GeregistreerdeRoutes.SingleOrDefault(r => r.RouteID == route.ID);
                //checken of loper geregistreerd is voor de route
                if (inschrijving != null)
                {
                    var zichtbaarheid = (Zichtbaarheid)routeLoperDTO.Zichtbaarheid;
                    loper.GeregistreerdeRoutes.SingleOrDefault(r => r.RouteID == route.ID).Zichtbaarheid = zichtbaarheid;
                    _loperRepository.SaveChanges();
                    return NoContent();
                }
                return BadRequest();
            }
            return NotFound();
        }

        // PUT: api/RouteLoper/124/location
        /// <summary>
        /// Update de locatie van een loper
        /// </summary>
        /// <param name="loperId">Mandatory ID van loper</param>
        /// <param name="huidigeLocatie">Mandatory de huidige locatie van de loper</param>
        /// <returns>ActionResult</returns>
        [HttpPut("location")]
        [Authorize]
        public ActionResult UpdateLocation(HuidigeLocatieDTO huidigeLocatie)
        {
            var userName = _userManager.GetUserName(User);
            var loper = _loperRepository.GetByEmail(userName);
            var routeLoper = loper.GeregistreerdeRoutes.Where(rl => rl.RouteID == huidigeLocatie.RouteId).FirstOrDefault();
            if(routeLoper == null || loper == null)
            {
                return NotFound();
            }
            routeLoper.LaatsteLocatieLat = huidigeLocatie.Lat;
            routeLoper.LaatsteLocatieLon = huidigeLocatie.Lon;
            _loperRepository.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deletes a routeLoper
        /// </summary>
        /// <param name="routeID">the id of the route</param>
        /// <param name="loperID">the id of the loper</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("routes/{routeID}")]
        public ActionResult Delete(int routeID)
        {
            var userName = _userManager.GetUserName(User);
            var loper = _loperRepository.GetByEmail(userName);
            Route route = _routeRepository.GetById(routeID);
            if (loper != null || route != null || loper != null)
            {
                route.RemoveLoper(loper);
                _loperRepository.SaveChanges();
                _routeRepository.SaveChanges();
                return NoContent();

            }
            return NotFound();

        }


        private Route GetCurrentRoute(int loperID)
        {
            Loper loper = _loperRepository.GetByID(loperID);
            if (loper == null)
                return null;
            var actieveRoute = loper.GeregistreerdeRoutes.Where(gr => gr.StartDatumEnUur != null && gr.EindDatumEnUur == null).FirstOrDefault();
            if(actieveRoute == null)
            {
                var routes = loper.GeregistreerdeRoutes.Select(r => r.Route);
                //Start met zoeken laatst gestarte route
                actieveRoute = loper.GeregistreerdeRoutes.Where(r => r.Route.Start.Date == DateTime.Now.Date).FirstOrDefault();

            }
            return actieveRoute?.Route; 
        }

        private double BerekenAfstand(double latA, double latB, double lonA, double lonB)
        {
            double r = Math.PI / 180;
            double R = 6371;
            double deltaLat = (latB - latA) * r;
            double deltaLon = (lonB - lonA) * r;
            var a =
                Math.Pow(Math.Sin(deltaLat / 2), 2) +
                Math.Cos(Math.Cos(latB * r) * latA * r) * Math.Pow(Math.Sin(deltaLon / 2), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d;
        }
    }
}







