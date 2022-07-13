using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Category Entitiy sindeki Id nin PrimaryKey olduğunu belirtir
            builder.HasKey(x => x.Id);
            //Id nin 1er 1er artması için kullanılır
            builder.Property(x=>x.Id).UseIdentityColumn();
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);

            //Tablo ismi Products olsun diye belirtilir
            builder.ToTable("Categories");
        }
    }
}
