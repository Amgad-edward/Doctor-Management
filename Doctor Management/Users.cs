using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doctor_Management
{
    public class Users  
    {

        public dynamic Admin => admin;

        public dynamic Login => login;

        public  dynamic User => user;


        private dynamic admin;

        private dynamic login;

        private dynamic user;

        public Users()
        {
           
        }
        public Users(HttpContext httpContext)
        {
            add(httpContext);
        }

        public Users(dynamic admin, dynamic login, dynamic user)
        {
            this.admin = admin;
            this.login = login;
            this.user = user;
        }

        void add(HttpContext httpContext)
        {
            if (httpContext.Session.Keys.ToList().Count > 0)
            {
                this.user = httpContext.Session.GetString("U");
                this.login = true;
                this.admin = Convert.ToBoolean(httpContext.Session.GetInt32("A"));
            }
            else
            {
                this.user = "";
                this.login = false;
                this.admin = false;
            }
        }


    }
}
