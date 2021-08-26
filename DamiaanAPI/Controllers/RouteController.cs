using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamiaanAPI.Data.DTOs;
using DamiaanAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DamiaanAPI.Controllers
{

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class RouteController : ControllerBase
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IPuntRepository _puntRepository;
        private static readonly string IMAGE_DIRECTORY = "Images";

        public RouteController(IRouteRepository routeRepository, IPuntRepository puntRepository)
        {
            _routeRepository = routeRepository;
            _puntRepository = puntRepository;
        }

        /// <summary>
        /// Get all routes
        /// </summary>
        /// <returns>array of routes</returns>
        [HttpGet("")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<Route> GetAll()
        {
            //Het geoJson bestand heb je niet nodig voor het overzicht van de routes.
            //Haal het bestand op door de aparte methode.
            return _routeRepository.GetAll().Select(r => { r.GeoJson = ""; return r; });
        }

        /// <summary>
        /// Get all public routes
        /// </summary>
        /// <returns>array of public routes</returns>
        [AllowAnonymous]
        [HttpGet("public")]
        public IEnumerable<RouteDTO> GetPublicRoutes()
        {
            //Het geoJson bestand heb je niet nodig voor het overzicht van de routes.
            //Haal het bestand op door de aparte methode.
            return _routeRepository.GetPublicRoutes().Select(r => { r.GeoJson = ""; return new RouteDTO(r); });
        }

        /// <summary>
        /// Get geoJson of route with given id
        /// </summary>
        /// <param name="id">the id of route</param>
        /// <returns>the geoJson of the route</returns>
        [AllowAnonymous]
        [HttpGet("{id}/geojson")]
        public ActionResult<String> GetByIdGeoJson(int id)
        {
            var route = _routeRepository.GetById(id);
            if (route == null)
            {
                return NotFound();
            }
            return route.GeoJson;
        }

        /// <summary>
        /// Get route with given id
        /// </summary>
        /// <param name="id">the id of the route</param>
        /// <returns>the route</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<RouteDTO> GetById(int id)
        {
            var route = _routeRepository.GetById(id);
            if (route == null)
            {
                return NotFound();
            }
            route.GeoJson = "";
            return new RouteDTO(route);
        }

        /// <summary>
        /// Get points of route with given id
        /// </summary>
        /// <param name="id">the id of the route</param>
        /// <returns>array of points of the route</returns>
        [AllowAnonymous]
        [HttpGet("{id}/points")]
        public ActionResult<List<PuntDTO>> GetPointsById(int id)
        {
            var route = _routeRepository.GetById(id);
            if (route == null)
            {
                return NotFound();
            }
            return route.Punten.Select(p => new PuntDTO(p)).ToList();
        }

        /// <summary>
        /// Adds a new route
        /// </summary>
        /// <param name="route">the new route</param>
        /// <returns>the new route</returns>
        [HttpPost("")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Route> New(RouteDTO route)
        {
            try
            {
                Route r = new Route(route);
                _routeRepository.NewRoute(r);
                return r;
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Modifies a route
        /// </summary>
        /// <param name="id">the id of the route</param>
        /// <param name="route">the modified route</param>
        /// <returns>the modified route</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult<RouteDTO> Update(int id, RouteDTO route)
        {
            try
            {
                Route r = _routeRepository.GetById(id);
                if (r != null)
                {
                    r.SetFromDto(route);
                    _routeRepository.Update(r);
                    return route;
                }
                return NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes a route
        /// </summary>
        /// <param name="id">the id of the route</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Route r = _routeRepository.GetById(id);
            if (r != null)
            {
                _routeRepository.Delete(r);
                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), IMAGE_DIRECTORY, "route" + id);
                if (Directory.Exists(imageDirectory))
                {
                    Directory.Delete(imageDirectory, true);
                }
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Adds a new point to a route
        /// </summary>
        /// <param name="routeId">the id of the route</param>
        /// <param name="punt">the new point</param>
        /// <returns>the new point</returns>
        [HttpPost("{routeId}/punten")]
        public ActionResult<Punt> NewPunt(int routeId, PuntDTO punt)
        {
            Route route = _routeRepository.GetById(routeId);
            if (route == null || punt == null)
            {
                return NotFound();
            }
            try
            {

                Punt nieuw = new Punt(punt);
                route.Punten.Add(nieuw);
                _routeRepository.Update(route);
                _routeRepository.SaveChanges();

                return nieuw;
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }

        }

        /// <summary>
        /// Modifies a point of a route
        /// </summary>
        /// <param name="routeId">the id of the route</param>
        /// <param name="puntId">the id of the point</param>
        /// <param name="punt">the modified point</param>
        /// <returns>the modified point</returns>
        [HttpPut("{routeId}/punten/{puntId}")]
        public ActionResult<Punt> UpdatePoints(int routeId, int puntId, PuntDTO punt)
        {
            Route route = _routeRepository.GetById(routeId);
            if (route == null || punt == null)
            {
                return NotFound();
            }
            if (puntId != punt.Id)
                return BadRequest();
            try
            {
                Punt oudPunt = route.Punten.SingleOrDefault(p => p.ID == puntId);

                // UPDATE
                if (oudPunt != null)
                {
                    // oudPunt updaten
                    oudPunt.SetFromDto(punt);
                }
                else
                {
                    return NotFound();
                }

                _routeRepository.Update(route);
                _routeRepository.SaveChanges();

                return oudPunt;
            }
            catch (ArgumentException e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes a point from a route
        /// </summary>
        /// <param name="routeId">the id of the route</param>
        /// <param name="pointId">the id of the point</param>
        /// <returns>no content</returns>
        [HttpDelete("{routeId}/punten/{pointId}")]
        public ActionResult DeletePoint(int routeId, int pointId)
        {
            Route r = _routeRepository.GetById(routeId);
            if (r != null)
            {
                Punt punt = r.Punten.SingleOrDefault(p => p.ID == pointId);
                if (punt != null)
                {
                    r.Punten.Remove(punt);

                    _routeRepository.Update(r);

                    _puntRepository.Delete(punt);
                    return NoContent();
                }
            }
            return NotFound();
        }

        [HttpPost("{id}/afbeeldingen")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UploadImageAsync(int id, IFormFileCollection files)
        {
            var route = _routeRepository.GetById(id);
            if (route == null)
                return NotFound();
            var directory = Path.Combine(Directory.GetCurrentDirectory(), IMAGE_DIRECTORY, "route" + id);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            foreach (var file in files)
            {
                try
                {
                    if (file.ContentType.Contains("image/"))
                    {
                        var path = Path.Combine(directory, file.FileName);
                        using (var writeStream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(writeStream);
                        }
                        route.Afbeeldingen.Add(file.FileName);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("File creation failed: " + e.Message);
                    Console.Error.WriteLine(e.StackTrace);
                }
            }
            _routeRepository.Update(route);
            return NoContent();
        }

        [HttpDelete("{id}/afbeeldingen/{name}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteImage(int id, string name)
        {
            try
            {
                var route = _routeRepository.GetById(id);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), IMAGE_DIRECTORY, "route" + id);
                if (route == null || !Directory.Exists(directory))
                    return NotFound();
                var path = Path.Combine(directory, name);
                System.IO.File.Delete(path);
                route.Afbeeldingen.Remove(name);
                _routeRepository.Update(route);
                return NoContent();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            } catch(Exception e)
            {
                Console.Error.WriteLine("File deletion failed: " + e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return BadRequest();
            }
        }

        [HttpGet("{id}/afbeeldingen/{name}")]
        [AllowAnonymous]
        public ActionResult DownloadImage(int id, string name)
        {

            try
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), IMAGE_DIRECTORY, "route" + id);
                if (!Directory.Exists(directory))
                    return NotFound();
                var path = Path.Combine(directory, name);
                var ext = Path.GetExtension(path).Trim('.');
                var stream = new FileStream(path, FileMode.Open);
                return new FileStreamResult(stream, "image/" + ext);
            }
            catch (FileNotFoundException )
            {
                return NotFound();
            } catch(Exception e)
            {
                Console.Error.WriteLine("File Reading failed: " + e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return BadRequest();
            }
        }
    }
}
