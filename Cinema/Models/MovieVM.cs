using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class MovieVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Year")]
        [RegularExpression("^[0-9]{4,4}$", ErrorMessage = "Count must be a natural number")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Producer")]
        public string Producer { get; set; }
        
        public string PosterURL { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Author")]
        public string User{ get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}