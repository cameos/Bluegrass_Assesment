using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class NewContact
    {
        //person information
        public string contactFirstName { get; set; }
        public string contactLastName { get; set; }
        public string contactIdNumber { get; set; }
        public string contactStatus { get; set; }
        public string contactGender { get; set; }
        public string contactPhone { get; set; }
        public string contactEmail { get; set; }
        public HttpPostedFileBase contactImage { get; set; }


        //address information
        public string addressNumber { get; set; }
        public string addressName { get; set; }
        public string contactCountry { get; set; }
        public string contactProv { get; set; }
        public string contactCiti { get; set; }
        public string addressSuburb { get; set; }
        public string addressPostalCode { get; set; }

    }
}