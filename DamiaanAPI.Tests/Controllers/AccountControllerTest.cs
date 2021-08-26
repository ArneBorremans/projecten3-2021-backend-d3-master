using DamiaanAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DamiaanAPI.Models;
using Microsoft.Extensions.Configuration;
using DamiaanAPI.Tests.MockData;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DamiaanAPI.Tests.Controllers
{
    public class AccountControllerTest
    {
        #region Properties
        private readonly AccountController _accountController;
        private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<ILoperRepository> _mockLoperRepository;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly Mock<IUserClaimsPrincipalFactory<IdentityUser>> _userPrincipalFactory;
        Mock<IHttpContextAccessor> _contextAccessor;
        Mock<IRoleStore<IdentityRole>> _mockRoleStore;

        private readonly MockLoginRegisterData mockLoginRegisterData;
        private readonly MockRouteLoperRepositoryData mockRouteLoperRepositoryData;
        #endregion

        #region Constructor
        public AccountControllerTest()
        {
            #region Mock User Manager
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();

            _mockUserManager = new Mock<UserManager<IdentityUser>>(userStoreMock.Object,
                null, null, null, null, null, null, null, null);
            #endregion

            #region Mock SignIn Manager
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

            _mockSignInManager = new Mock<SignInManager<IdentityUser>>(_mockUserManager.Object,
                _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null, null);
            #endregion

            #region Mock LoperRepo en Config
            _mockLoperRepository = new Mock<ILoperRepository>();
            _mockConfig = new Mock<IConfiguration>();
            #endregion

            #region Mock Role Manager
            _mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                _mockRoleStore.Object, null, null, null, null);
            #endregion

            #region Create Account Controller with Mocked Objects
            _accountController = new AccountController(
                _mockSignInManager.Object, _mockUserManager.Object, _mockLoperRepository.Object,
                _mockConfig.Object, _mockRoleManager.Object);
            #endregion

            #region Initialize Mock Data
            mockLoginRegisterData = new MockLoginRegisterData();
            mockRouteLoperRepositoryData = new MockRouteLoperRepositoryData();
            #endregion
        }
        #endregion

        #region POST CreateToken
        [Fact]
        public void CreateToken_UserIsNull_ReturnsBadRequest()
        {
            _mockUserManager
                .Setup(x => x.FindByNameAsync("test@test.be"))
                .Returns(Task.FromResult<IdentityUser>(null));
            _mockLoperRepository
                .Setup(x => x.GetByEmail("test@test.be"))
                .Returns(mockRouteLoperRepositoryData.loper);

            var result = _accountController.CreateToken(mockLoginRegisterData.loginDTO);

            Assert.IsAssignableFrom<BadRequestResult>(result.Result.Result);
        }

        [Fact]
        public void CreateToken_UserIsNotNull_FailedSignInResult_ReturnsBadRequest()
        {
            _mockUserManager
                .Setup(x => x.FindByNameAsync("test@test.be"))
                .Returns(Task.FromResult(mockLoginRegisterData.user));
            _mockLoperRepository
                .Setup(x => x.GetByEmail("test@test.be"))
                .Returns(mockRouteLoperRepositoryData.loper);
            _mockSignInManager
                .Setup(x => x.CheckPasswordSignInAsync(mockLoginRegisterData.user,
                    mockLoginRegisterData.loginDTO.Password, false))
                .Returns(Task.FromResult
                    (Microsoft.AspNetCore.Identity.SignInResult.Failed));

            var result = _accountController.CreateToken(mockLoginRegisterData.loginDTO);

            Assert.IsAssignableFrom<BadRequestResult>(result.Result.Result);
        }

        [Fact]
        public void CreateToken_UserIsNotNull_SucceededSignInResult_ReturnsCreated()
        {
            _mockUserManager
                .Setup(x => x.FindByNameAsync("test@test.be"))
                .Returns(Task.FromResult(mockLoginRegisterData.user));
            _mockUserManager
                .Setup(x => x.GetRolesAsync(mockLoginRegisterData.user))
                .Returns(Task.FromResult(mockLoginRegisterData.roles));
            _mockLoperRepository
                .Setup(x => x.GetByEmail("test@test.be"))
                .Returns(mockRouteLoperRepositoryData.loper);
            _mockSignInManager
                .Setup(x => x.CheckPasswordSignInAsync(mockLoginRegisterData.user,
                    mockLoginRegisterData.loginDTO.Password, false))
                .Returns(Task.FromResult
                    (Microsoft.AspNetCore.Identity.SignInResult.Success));
            _mockConfig
                .Setup(x => x[It.IsAny<string>()])
                .Returns("IfThisEndsUpInClassYouFail");

            var result = _accountController.CreateToken(mockLoginRegisterData.loginDTO);

            Assert.IsAssignableFrom<CreatedResult>(result.Result.Result);
        }
        #endregion

        #region POST Register
        [Fact]
        public void Register_FailedRegisterResult_ReturnsBadRequest()
        {
            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), mockLoginRegisterData.registerDTO.Password))
                .Returns(Task.FromResult
                    (IdentityResult.Failed()));

            var res = _accountController.Register(mockLoginRegisterData.registerDTO);

            Assert.IsAssignableFrom<BadRequestResult>(res.Result.Result);
        }

        [Fact]
        public void Register_SucceededRegisterResult_CreatedResult()
        {
            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), mockLoginRegisterData.registerDTO.Password))
                .Returns(Task.FromResult(IdentityResult.Success));
            _mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>()))
                .Returns(Task.FromResult(mockLoginRegisterData.roles));
            _mockConfig
               .Setup(x => x[It.IsAny<string>()])
               .Returns("IfThisEndsUpInClassYouFail");

            var res = _accountController.Register(mockLoginRegisterData.registerDTO);

            Assert.IsAssignableFrom<CreatedResult>(res.Result.Result);
        }
        #endregion

        #region GET CheckAvailableUserName
        [Fact]
        public void CheckAvailableUserName_UserNull_ReturnsFalse()
        {
            _mockUserManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IdentityUser>(null));

            var res = _accountController.CheckAvailableUserName("test@test.be");

            Assert.IsAssignableFrom<Boolean>(res.Result.Value);
            Assert.True(res.Result.Value);
        }

        [Fact]
        public void CheckAvailableUserName_UserNotNull_ReturnsTrue()
        {
            _mockUserManager
               .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<IdentityUser>(mockLoginRegisterData.user));

            var res = _accountController.CheckAvailableUserName("test@test.be");

            Assert.IsAssignableFrom<Boolean>(res.Result.Value);
            Assert.False(res.Result.Value);
        }
        #endregion
    }
}
