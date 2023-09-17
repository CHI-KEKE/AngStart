using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Dtos.Marketing;
using API.Dtos.Orders;
using API.Dtos.Product;
using AutoMapper;
using Core.Entities;
using Core.Entities.Marketing;
using Core.Entities.Pay;
using Color = Core.Entities.Color;

namespace API.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(imageUrl => new Image { Url = imageUrl }).ToList()))

                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes.Select(sizemark => new Size { SizeMark = sizemark }).ToList()));

            CreateMap<API.Dtos.Color, Core.Entities.Color>();
            CreateMap<API.Dtos.Variant, Core.Entities.Variant>();

            CreateMap<CampaignDto, Campaign>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => SaveUploadedFile.SaveUploadedFileMethod(src.Picture)));

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => src.MainImage))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(img => img.Url).ToList()))
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.Colors.Select(color => color.Name).ToList()))
                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes.Select(size => size.SizeMark).ToList()))
                .ForMember(dest => dest.Stocks, opt => opt.MapFrom(src => src.Variants.Select(variant => variant.Stock).ToList()));


            CreateMap<ColorDataDto, Core.Entities.Color>();
            CreateMap<ItemDataDto, Item>()
            .ForMember(dest => dest.Product_Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<OrderDataDto, Order>()
            // .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Number, opt => opt.Ignore())
            .ForMember(dest => dest.Condition, opt => opt.Ignore())
            .ForMember(dest => dest.Shipping, opt => opt.Ignore())
            .ForMember(dest => dest.Payment, opt => opt.Ignore())
            .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
            .ForMember(dest => dest.Freight, opt => opt.Ignore())   
            .ForMember(dest => dest.Recipient, opt => opt.Ignore())
            .ForMember(dest => dest.List, opt => opt.MapFrom(src => src.List));
           
            // .ForMember(dest => dest.Id, opt => opt.Ignore())
            // .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
            
            
            //     .ForMember(dest => dest.Id, opt => opt.Ignore()); 
            



            CreateMap<ProductReceiveDto, Product>()
             .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.VariantColors.Zip(src.VariantColorNames, (code, name) => new Color { Code = code, Name = name })))
             .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.VariantSizes.Select(size => new Size { SizeMark = size }))) 
             .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.VariantStocks.Select((stock, index) => new Core.Entities.Variant { ColorCode = src.VariantColors[index], Size = src.VariantSizes[index], Stock = stock }))) 
             .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(imageFile => new Image { Url = SaveUploadedFile.SaveUploadedFileMethod(imageFile) }))) 
             .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => SaveUploadedFile.SaveUploadedFileMethod(src.MainImage))); 

        }
    }
}