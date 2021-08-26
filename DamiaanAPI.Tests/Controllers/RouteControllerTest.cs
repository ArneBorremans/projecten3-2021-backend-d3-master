using DamiaanAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using DamiaanAPI.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DamiaanAPI.Data.DTOs;
using DamiaanAPI.Tests.MockData;

namespace DamiaanAPI.Tests.Controllers
{
    public class RouteControllerTest
    {
        #region Properties
        private readonly RouteController _routeController;
        private readonly Mock<IRouteRepository> _mockRouteRepository;
        private readonly MockRouteRepositoryData mockRouteRepositoryData;
        #endregion

        #region Constructor
        public RouteControllerTest()
        {
            _mockRouteRepository = new Mock<IRouteRepository>();

            _routeController = new RouteController(_mockRouteRepository.Object, new Mock<IPuntRepository>().Object);

            mockRouteRepositoryData = new MockRouteRepositoryData();
        }
        #endregion

        #region GET All
        [Fact]
        public void GetAll_ReturnsNoRoute()
        {
            _mockRouteRepository
                .Setup(x => x.GetAll())
                .Returns(mockRouteRepositoryData.routesEmpty);

            IEnumerable<Route> routes = _routeController.GetAll();

            Assert.Equal(0, Count(routes));
        }

        [Fact]
        public void GetAll_ReturnsSingleRoute()
        {
            _mockRouteRepository
                .Setup(x => x.GetAll())
                .Returns(mockRouteRepositoryData.routesSingle);

            IEnumerable<Route> routes = _routeController.GetAll();

            Assert.Single(routes);
        }

        [Fact]
        public void GetAll_ReturnsMultipleRoute()
        {
            _mockRouteRepository
                .Setup(x => x.GetAll())
                .Returns(mockRouteRepositoryData.routesMultiple);

            IEnumerable<Route> routes = _routeController.GetAll();

            Assert.Equal(2, Count(routes));
        }

        [Fact]
        public void GetAll_CheckIfGeoJsonIsEmpty()
        {
            _mockRouteRepository
                .Setup(x => x.GetAll())
                .Returns(mockRouteRepositoryData.routesSingle);

            IEnumerable<Route> routes = _routeController.GetAll();

            Assert.Equal("", routes.FirstOrDefault().GeoJson);
        }
        #endregion

        #region GET PublicRoutes
        [Fact]
        public void GetPublicRoutes_ReturnsNoRoute()
        {
            _mockRouteRepository
                .Setup(x => x.GetPublicRoutes())
                .Returns(mockRouteRepositoryData.routesEmpty);

            IEnumerable<RouteDTO> routes = _routeController.GetPublicRoutes();

            Assert.Equal(0, Count(routes));
        }

        [Fact]
        public void GetPublicRoutes_ReturnsSingleRoute()
        {
            _mockRouteRepository
                .Setup(x => x.GetPublicRoutes())
                .Returns(mockRouteRepositoryData.routesSingle);

            IEnumerable<RouteDTO> routes = _routeController.GetPublicRoutes();

            Assert.Single(routes);
        }

        [Fact]
        public void GetPublicRoutes_ReturnsMultipleRoute()
        {
            _mockRouteRepository
                .Setup(x => x.GetPublicRoutes())
                .Returns(mockRouteRepositoryData.routesMultiple);

            IEnumerable<RouteDTO> routes = _routeController.GetPublicRoutes();

            Assert.Equal(2, Count(routes));
        }

        [Fact]
        public void GetPublicRoutes_CheckIfGeoJsonIsEmpty()
        {
            _mockRouteRepository
                .Setup(x => x.GetPublicRoutes())
                .Returns(mockRouteRepositoryData.routesSingle);

            IEnumerable<RouteDTO> routes = _routeController.GetPublicRoutes();

            Assert.Equal("", routes.FirstOrDefault().GeoJson);
        }
        #endregion

        #region GET ByIdGeoJson
        [Fact]
        public void GetByIdGeoJson_RouteIsNull_ReturnsNotFoundResult()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns((Route) null);

            var result = _routeController.GetByIdGeoJson(1);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetByIdGeoJson_RouteIsNotNull_ReturnsString()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault());

            var result = _routeController.GetByIdGeoJson(1);

            Assert.IsAssignableFrom<String>(result.Value);
        }
        #endregion

        #region GET ById
        [Fact]
        public void GetById_RouteIsNull_ReturnsNotFoundResult()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns((Route)null);

            var result = _routeController.GetById(1);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetById_RouteIsNotNull_ReturnsRouteAndEmptyGeoJsonString()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault());

            var result = _routeController.GetById(1);
            // Is of type Route
            Assert.IsAssignableFrom<RouteDTO>(result.Value);
            // GeoJson is emptied
            Assert.Equal("", result.Value.GeoJson);
        }
        #endregion

        #region GET PointsById
        [Fact]
        public void GetPointsById_RouteIsNull_ReturnsNotFoundResult()
        {
            _mockRouteRepository
               .Setup(x => x.GetById(1))
               .Returns((Route) null);

            var result = _routeController.GetPointsById(1);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetPointsById_RouteIsNotNull_ReturnsListOfPuntDTO()
        {
            _mockRouteRepository
               .Setup(x => x.GetById(1))
               .Returns(mockRouteRepositoryData.routesMultiple.First);

            var result = _routeController.GetPointsById(1);

            Assert.IsAssignableFrom<List<PuntDTO>>(result.Value);
        }
        #endregion

        #region POST New
        [Fact]
        public void New_RouteDTOPassed_PuntenIsNull_ReturnsNewRoute_PuntenIsEmpty()
        {
            var route = _routeController.New(mockRouteRepositoryData.routeDTOPuntenIsNull);
            // Is of type Route
            Assert.IsAssignableFrom<Route>(route.Value);
            // Punten List is empty
            Assert.Empty(route.Value.Punten);
        }

        [Fact]
        public void New_RouteDTOPassed_PuntenIsNotNull_ReturnsNewRoute_PuntenIsNotEmpty()
        {
            var route = _routeController.New(mockRouteRepositoryData.routeDTOPuntenNotNull);
            // Is of type Route
            Assert.IsAssignableFrom<Route>(route.Value);
            // Punten List is empty
            Assert.NotEmpty(route.Value.Punten);
        }
        #endregion

        #region PUT Update
        [Fact]
        public void Update_RouteNotNull_ReturnsRouteDTO()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault());

            var result = _routeController.Update(1, mockRouteRepositoryData.routeDTOPuntenNotNull);

            Assert.IsAssignableFrom<RouteDTO>(result.Value);
        }

        [Fact]
        public void Update_RouteNull_ReturnsNotFound()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns((Route) null);

            var result = _routeController.Update(1, mockRouteRepositoryData.routeDTOPuntenNotNull);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }
        #endregion

        #region POST NewPunt
        [Fact]
        public void New_PuntDTOPassed_RouteIsNull_ReturnsNotFound()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns((Route)null);

            var result = _routeController.NewPunt(1, mockRouteRepositoryData.puntenDTO[0]);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void New_PuntDTOPassed_RouteNotNull_ReturnsNoContent()
        {
            _mockRouteRepository
                 .Setup(x => x.GetById(1))
                 .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault);

            var result = _routeController.NewPunt(1, mockRouteRepositoryData.puntenDTO[0]);

            Assert.IsAssignableFrom<Punt>(result.Value);
        }

        [Fact]
        public void NewPunt_RouteNotNull_IllegalFaciliteitString_ThrowsArgumentException()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault);

            Assert.Throws<ArgumentException>(() => _routeController.NewPunt(1, new PuntDTO(new Punt
                                                                        {
                                                                            Faciliteiten = new List<string> { "|" }
                                                                            ,
                                                                            Lat = 50.313654M,
                                                                            Lon = 5.148726M
                                                                        })));
        }
        #endregion

        #region PUT UpdatePoint
        [Fact]
        public void Update_PuntNotNull_RouteNotNull_ReturnsPuntDTO()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault);

            int ID = mockRouteRepositoryData.routesMultiple.FirstOrDefault().Punten.FirstOrDefault().ID;

            var result = _routeController.UpdatePoints(1, ID, mockRouteRepositoryData.puntenUpdateDTO[0]);

            Assert.IsAssignableFrom<Punt>(result.Value);
            Assert.Equal(mockRouteRepositoryData.puntenUpdate[0].Naam, result.Value.Naam);
        }

        [Fact]
        public void Update_PuntNull_RouteNotNull_ReturnsNotFound()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault);

            int ID = mockRouteRepositoryData.routesMultiple.FirstOrDefault().Punten.FirstOrDefault().ID;

            var result = _routeController.UpdatePoints(1, ID, null);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }
        #endregion

        #region DELETE DeletePoint
        [Fact]
        public void Delete_PuntNotNull_ReturnsNoContent()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault);

            var result = _routeController.DeletePoint(1, mockRouteRepositoryData.punten[1].ID);

            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public void Delete_PuntNull_ReturnsNotFound()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault);

            var result = _routeController.DeletePoint(1, 3);

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }
        #endregion

        #region DELETE Delete
        [Fact]
        public void Delete_RouteNotNull_ReturnsNoContent()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault());

            var result = _routeController.Delete(1);

            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public void Delete_RouteNull_ReturnsNotFound()
        {
            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns((Route)null);

            var result = _routeController.Delete(1);

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }
        #endregion

        #region HelpMethods
        private int Count<T>(IEnumerable<T> collection)
        {
            int counter = 0;

            foreach(var element in collection)
            {
                counter++;
            }

            return counter;
        }
        #endregion
    }
}
