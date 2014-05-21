using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Collections;
using RecipeMvc.Filters;

namespace RecipeMvc.Models
{
    public class RespComp
    {

         [Required] 
        public long Id { get; set; }
        //[Required] 
        public string Title { get; set; }
         [Range(0,10)]
        public double FRate { get; set; }
        public int CookingTime { get; set; }
        public int Number { get; set; }
        public int RNum { get;set; }
        public string FilePath { get; set; }
        public string FoodPreparation { get; set; }
        public virtual List<Rate> Rates { get; private set; }
        public virtual List<Ingredient> Ingredients { get; set; }
        public int RateCount
        {
            get { return Rates.Count; }
        }
        public RespComp()
        {
             Rates = new List<Rate>();
        }
        
    }
}