using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTracker.Models.DTOs
{
    public class Urls
    {
        public readonly string Root = "/";
        public readonly string Home = "/Home/Home";
        public readonly string Admin = "/Home/Admin";
        public readonly string Profile = "/Account/Profile";
        public readonly string Login = "/Account/Login";
        public readonly string Logout = "/Account/Logout";
        public readonly string Claims = "/Account/Claims";
    }
}
