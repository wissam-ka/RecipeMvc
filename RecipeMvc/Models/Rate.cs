 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace RecipeMvc.Models
{
    public class Rate
    {
        public long Id { get; internal set; }
        public long ReId { get; set; }
        [Range(0, 10)]
        public double Comm { get; set; }
    }
}