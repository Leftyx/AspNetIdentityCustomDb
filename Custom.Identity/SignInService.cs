﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custom.Identity
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    public class SignInService: SignInManager<User, int>
    {
        public SignInService(UserManager userManager, IAuthenticationManager authenticationManager): base(userManager, authenticationManager)
        {

        }

        public override Task SignInAsync(User user, bool isPersistent, bool rememberBrowser)
        {
            return base.SignInAsync(user, isPersistent, rememberBrowser);
        }
    }
}
