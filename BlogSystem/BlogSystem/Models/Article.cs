using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Length must be between 3 and 20")]

        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public byte[] Photo { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        //[ForeignKey("User")]
        //public string UserId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }

        //public virtual ApplicationUser User { get; set; }
    }
}