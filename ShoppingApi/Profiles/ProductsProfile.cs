﻿using AutoMapper;
using ShoppingApi.Data;
using ShoppingApi.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            //Data.Product -> Models.Products.GetProductDetailsResponse
            CreateMap<Product, GetProductDetailsResponse>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<PostProductRequest, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.RemovedFromInventory, opt => opt.MapFrom(x => false))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(x => DateTime.Now));
        }
    }
}
