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
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Год")]
        public DateTime Year { get; set; }
        [Required]
        [Display(Name = "Режиссёр")]
        public string Producer { get; set; }
        
        public string PosterURL { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Автор")]
        public string User{ get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}