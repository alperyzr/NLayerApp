﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Stock).IsRequired();
            //Price maksimum 18 karakter ve virgülden sonra 2 karakter alır (virgülden öncesi 18)
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            
            //Tablo ismi Products olsun diye belirtilir
            builder.ToTable("Products");

            //Her product'ın 1 tane kategorisi olabilir, 1 kategorinin  1 den faza product ı olabilir
            builder.HasOne(x => x.Category).WithMany(x=>x.Products).HasForeignKey(x=>x.CategoryId);
        }
    }
}
