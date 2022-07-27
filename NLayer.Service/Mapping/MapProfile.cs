using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Mapping
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            //Cast Edilecek bütün DTO lar burada belirtilmek zorundadır
            //AutoMapper kütüphanesi ile mapleme işlemi yapılıyor
            //ReversalMap EntityToDto ve DtoToEntity için ters işlem amacıyla iki farklı yöntem ile çalışsın diye kullanılır
            //Bu işlemler sonrası API projesinin startup kısmında blirtilmesi gerekiyor
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<ProductFeature,ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductWithCategoryDto>();
            CreateMap<Category, CategoryWithProductsDto>();
        }
    }
}
