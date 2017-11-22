using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace PhotographyEvent.Models
{
    public class EventUserPhotos
    {
        public int EventID { get; set; }
        public string UserID { get; set; }
    }
}