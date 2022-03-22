﻿using SerialSales.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialSales.Domain.Entity
{
    public class ChildCategory: BaseEntity
    {
        public Guid Id { get; set; }
        public Guid SubCategoryId { get; set; }
        public string Name { get; set; }
    }
}