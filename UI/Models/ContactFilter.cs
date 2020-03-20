using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class ContactFilter
    {
        public string First { get; set; }
        public string CountryId { get; set; }
        public string ProvinceId { get; set; }
        public string CityId { get; set; }
    }
}