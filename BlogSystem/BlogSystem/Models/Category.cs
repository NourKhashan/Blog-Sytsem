using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        [StringLength(10, MinimumLength =3, ErrorMessage ="Length must be between 3 and 20")]
        [Remote("IsAlreadyExist", "Categories", HttpMethod = "POST", ErrorMessage = "Category already exists")]

        public string Name { get; set; }

        public virtual List<Article> Articles { get; set; }
    }
}