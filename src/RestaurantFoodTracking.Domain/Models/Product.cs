﻿using RestaurantFoodTracking.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantFoodTracking.Domain.Models
{
    public class Product:BaseEntity
    {
        public Guid ShopId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubCategoryId { get; set; }
        public string PictureUrl { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsThere { get; set; }
    }
}
