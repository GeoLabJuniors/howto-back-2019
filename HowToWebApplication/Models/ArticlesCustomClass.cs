﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace HowToWebApplication.Models
{
    public class ArticlesCustomClass
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }


        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }


        [Required]
        [Display(Name = "User")]
        public int UsersId { get; set; }

        [Display(Name = "Category")]
        public int[] CategoriesList { get; set; }


        [Display(Name = "Article title")]
        public  List<int> RequestsList { get; set; }

    }
}