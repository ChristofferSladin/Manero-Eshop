﻿using ServiceLibrary.Models;

namespace ManeroWebApp.Models
{
    public class CategoryPageViewModel
    {
        public List<string>? GenderCategories { get; set; }
        public List<Category>? ProductCategories { get; set; }
        public List<Product>? Products { get; set; }
    }
}
