using DamiaanAPI.Data.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DamiaanAPI.Tests.MockData
{
    public class MockLoginRegisterData
    {
        #region Properties

        #region LoginDTO
        public LoginDTO loginDTO { get; set; }
        #endregion

        #region RegisterDTO
        public RegisterDTO registerDTO;
        #endregion

        #region IdentityResult
        public IdentityResult identityResultSucceed;
        public IdentityResult identityResultFail;
        #endregion

        #region IdentityUser
        public IdentityUser user { get; set; }
        #endregion

        #region Roles
        public IList<string> roles;
        #endregion

        #endregion

        #region Constructor
        public MockLoginRegisterData()
        {
            // INITIALIZE DATA
            #region LoginDTO
            loginDTO = new LoginDTO() { Email = "test@test.be", Password = "P@ssword1"};
            #endregion

            #region RegisterDTO
            registerDTO = new RegisterDTO()
            {
                Email = "test@test.be",
                Password = "P@ssword1",
                FirstName = "test",
                LastName = "test",
                Gemeente = "testGemeente",
                PasswordConfirmation = "P@ssword1"
            };
            #endregion

            #region IdentityResult
            identityResultSucceed = IdentityResult.Success;
            #endregion

            #region IdentityUser
            user = new IdentityUser() { Email = "test@test.be", UserName = "test@test.be" };
            #endregion

            #region Roles
            roles = new List<string> { "Admin" };
            #endregion
            // END
        }
        #endregion
    }
}
