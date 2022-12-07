﻿using RM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RM
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {          
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            UserTracking NewUser = new UserTracking();
            NewUser.IPAddress = Request.ServerVariables["REMOTE_ADDR"];          
            NewUser.insert(NewUser);
        }


    }
}
