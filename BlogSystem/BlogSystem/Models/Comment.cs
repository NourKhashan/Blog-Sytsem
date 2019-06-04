using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogSystem.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [ForeignKey("Article")]

        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}