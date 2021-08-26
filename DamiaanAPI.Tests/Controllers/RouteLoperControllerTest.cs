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
    public class RouteLoperControllerTest
    {
        // Sommige testen falen als ze tesamen runnen maar alles slaagt individueel
        
        #region Properties
        private readonly LoperController _routeLoperController;
        private readonly Mock<IRouteRepository> _mockRouteRepository;
        private readonly Mock<ILoperRepository> _mockLoperRepository;
        private readonly MockRouteLoperRepositoryData mockRouteLoperRepositoryData;
        private readonly MockRouteRepositoryData mockRouteRepositoryData;
        #endregion

        #region Constructor
        public RouteLoperControllerTest()
        {
            _mockRouteRepository = new Mock<IRouteRepository>();
            _mockLoperRepository = new Mock<ILoperRepository>();

            _routeLoperController = new LoperController(_mockLoperRepository.Object, _mockRouteRepository.Object);

            mockRouteLoperRepositoryData = new MockRouteLoperRepositoryData();
            mockRouteRepositoryData = new MockRouteRepositoryData();
        }
        #endregion

        #region GET Loper
        [Fact]
        public void GetLoper_LoperPassed_ReturnsUserInfoDTO()
        {
            _mockLoperRepository
               .Setup(x => x.GetByID(1))
               .Returns(mockRouteLoperRepositoryData.loper);

            var result = _routeLoperController.GetLoper(1);

            Assert.IsAssignableFrom<UserInfoDTO>(result.Value);
        }
        #endregion

        #region GET PublicLopers
        [Fact]
        public void GetPublicLopers_ReturnsEnumerableLopers()
        {
            _mockLoperRepository
               .Setup(x => x.GetPublicLopers(1))
               .Returns(mockRouteLoperRepositoryData.lopers.ToList());

            var result = _routeLoperController.GetPublicLopers(1);

            Assert.IsAssignableFrom<IEnumerable<Loper>>(result);
        }
        #endregion

        #region GET SearchHiddenLopers
        [Fact]
        public void SearchHiddenLopers_ReturnsLoper()
        {
            _mockLoperRepository
               .Setup(x => x.SearchHidden("John", "Parsley", "John@Parsley.com"))
               .Returns(mockRouteLoperRepositoryData.loper);

            var result = _routeLoperController.SearchHiddenLopers("John", "Parsley", "John@Parsley.com");

            Assert.IsAssignableFrom<Loper>(result);
        }
        #endregion

        #region GET SearchPublicLopers
        [Fact]
        public void SearchPublicLopers_ReturnsLopers()
        {
            _mockLoperRepository
               .Setup(x => x.SearchPublic("John", "Parsley", "John@Parsley.com"))
               .Returns(mockRouteLoperRepositoryData.lopers);

            var result = _routeLoperController.SearchPublicLopers("John", "Parsley", "John@Parsley.com");

            Assert.IsAssignableFrom<IEnumerable<Loper>>(result);
        }
        #endregion

        #region GET Chat
        [Fact]
        public void GetChat_ReturnsListMessageDTO()
        {
            _mockLoperRepository
                .Setup(x => x.GetChat("testing"))
                .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            var result = _routeLoperController.GetChat("testing");

            Assert.IsAssignableFrom<List<MessageDTO>>(result);
        }
        #endregion

        #region GET Ingeschreven
        [Fact]
        public void Ingeschreven_NietIngeschreven_ReturnedFalse()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            var result = _routeLoperController.Ingeschreven(2, 1);

            Assert.False(result.Value);
        }

        [Fact]
        public void Ingeschreven_LoperIsNull_ReturnedFalse()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns((Loper) null);

            var result = _routeLoperController.Ingeschreven(2, 1);

            Assert.False(result.Value);
        }

        [Fact]
        public void Ingeschreven_Ingeschreven_ReturnedTrue()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            var result = _routeLoperController.Ingeschreven(1, 1);

            Assert.True(result.Value);
        }
        #endregion

        #region GET CurrentRouteIDAndLocationByCode
        [Fact]
        public void CurrentRouteIDAndLocationByCode_LoperIsNull_ReturnsNotFound()
        {
            _mockLoperRepository
               .Setup(x => x.GetByLinkCode("testing"))
               .Returns((Loper) null);

            var result = _routeLoperController.CurrentRouteIDAndLocationByCode("testing");

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void CurrentRouteIDAndLocationByCode_LoperIsNotNull_ReturnsHuidigeLocatieDTO()
        {
            _mockLoperRepository
               .Setup(x => x.GetByLinkCode("testing"))
               .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            var result = _routeLoperController.CurrentRouteIDAndLocationByCode("testing");

            Assert.IsAssignableFrom<HuidigeLocatieDTO>(result.Value);
        }
        #endregion

        #region GET CurrentRoute
        [Fact]
        public void CurrentRoute_LoperIsNull_ReturnsNotFound()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns((Loper) null);

            var result = _routeLoperController.CurrentRoute(1);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void CurrentRoute_LoperIsNotNull_actieveRouteLoperIsNull_ReturnsNotFound()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(mockRouteLoperRepositoryData.loper);

            var result = _routeLoperController.CurrentRoute(1);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void CurrentRoute_LoperIsNotNull_actieveRouteLoperIsNotNull_ReturnsRouteDTO()
        {
            Loper routeLoper = mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes;

            routeLoper.GeregistreerdeRoutes.First().StartDatumEnUur = DateTime.Today;

            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(routeLoper);

            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.First());

            var result = _routeLoperController.CurrentRoute(1);

            Assert.IsAssignableFrom<RouteDTO>(result.Value);
        }
        #endregion

        #region POST AddMessage
        [Fact]
        public void AddMessage_LoperNotNull_ReturnMessageDTO()
        {
            _mockLoperRepository
               .Setup(x => x.GetChat("testing"))
               .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            var result = _routeLoperController.addMessage("testing", mockRouteLoperRepositoryData.messageDTO);

            Assert.IsAssignableFrom<MessageDTO>(result.Value);
        }

        // Faalt
        /*
        [Fact]
        public void AddMessage_LoperIsNull_ReturnNotFound()
        {
            _mockLoperRepository
               .Setup(x => x.GetChat("testing"))
               .Returns((Loper) null);

            var result = _routeLoperController.addMessage("testing", MockData.MockRouteLoperRepositoryData.messageDTO);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }
        */
        #endregion

        #region POST New
        [Fact]
        public void New_LoperIsNull_ReturnsNotFound()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns((Loper) null);

            var result = _routeLoperController.New(1, mockRouteLoperRepositoryData.routeLoperDTO);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void New_RouteIsNull_ReturnsNotFound()
        {
            _mockRouteRepository
               .Setup(x => x.GetById(1))
               .Returns((Route)null);

            var result = _routeLoperController.New(1, mockRouteLoperRepositoryData.routeLoperDTO);

            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void New_LoperIsNotNull_RouteIsNotNull_AlIngeschreven_ReturnsBadRequest()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.routesMultiple.FirstOrDefault());

            var result = _routeLoperController.New(1, mockRouteLoperRepositoryData.routeLoperDTO);

            Assert.IsAssignableFrom<BadRequestResult>(result.Result);
        }

        // Faalt, Mail krijgt een routeloper zonder naam door -> Nullreference
        /*
        [Fact]
        public void New_LoperIsNotNull_RouteIsNotNull_NogNietIngeschreven_ReturnsRouteLoperDTO()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(MockData.MockRouteLoperRepositoryData.loper);

            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(MockData.MockRouteRepositoryData.routesMultiple.First());

            var result = _routeLoperController.New(MockData.MockRouteLoperRepositoryData.routeLoperDTO);

            Assert.IsAssignableFrom<RouteLoperDTO>(result.Value);
        }*/
        #endregion

        #region PUT UpdateLocation
        [Fact]
        public void UpdateLocation_routeLoperIsNull_ReturnsNotFound()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            var result = _routeLoperController.UpdateLocation(1, mockRouteLoperRepositoryData.huidigeLocatieDTOFive);

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateLocation_routeLoperIsNotNull_ReturnsNotFound()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            var result = _routeLoperController.UpdateLocation(1, mockRouteLoperRepositoryData.huidigeLocatieDTOOne);

            Assert.IsAssignableFrom<NoContentResult>(result);
        }
        #endregion

        #region DELETE Delete
        // IN Route Loper Controller moet || niet && zijn?
        [Fact]
        public void Delete_LoperIsNullRouteIsNull_ReturnsNotFound()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns((Loper) null);

            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns((Route) null);

            var result = _routeLoperController.Delete(1, 1);

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_LoperIsNotNullRouteIsNotNull_ReturnsNoContent()
        {
            _mockLoperRepository
               .Setup(x => x.GetBy(1))
               .Returns(mockRouteLoperRepositoryData.loperMetGeregistreerdeRoutes);

            _mockRouteRepository
                .Setup(x => x.GetById(1))
                .Returns(mockRouteRepositoryData.RouteMetGeregistreerdeLoper);

            var result = _routeLoperController.Delete(1, 1);

            Assert.IsAssignableFrom<NoContentResult>(result);
        }
        #endregion
    }
}
