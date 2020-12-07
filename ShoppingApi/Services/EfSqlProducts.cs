using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace ShoppingApi.Services
{
    public class EfSqlProducts : ILookupProducts
    {
        private readonly ShoppingDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;


        public EfSqlProducts(ShoppingDataContext context, IMapper mapper, MapperConfiguration mapper_config)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapper_config;
        }

        public async Task<GetProductDetailsResponse> GetProductById(int id)
        {

            var response = await _context.Products
                .Where(p => p.Id == id && p.RemovedFromInventory == false)
                //.Select(p => new GetProductDetailsResponse
                //{
                //    Id = p.Id,
                //    Name = p.Name,
                //    Category = p.Category.Name,
                //    Count = p.Count,
                //    Price = p.Price
                //}).SingleOrDefaultAsync();

                //.Select(p => _mapper.Map<GetProductDetailsResponse>(p)
                .ProjectTo<GetProductDetailsResponse>(_mapperConfig)
                .SingleOrDefaultAsync();

            //check change

            return response;

            //return Task.FromResult(new GetProductDetailsResponse
            //{
            //    Id = id,
            //    Name = "Some Product",
            //    Category = "Bread",
            //    Count = 1,
            //    Price = 1.89M
            //});
        }
    }
}
