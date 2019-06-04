using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.Models
{
    public class ArticleCreateModelView
    {
        public List<SelectListItem> Categories { get; set; }
        public Article Article { get; set; }

    }


    public class ArticleCommentsModelView
    {
        public List<Comment> Comments { get; set; }
        public Article Article { get; set; }

    }

}