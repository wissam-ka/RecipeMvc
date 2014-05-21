using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeMvc.Models
{
    public class Ingredient
    {
        [Key]
        public long Id { get;internal set; }

        public string Name{ get; set; }
        public string Amount{ get; set; }
        public string Unit{ get; set; }

    }
}