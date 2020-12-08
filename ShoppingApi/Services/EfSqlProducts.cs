using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;

namespace ShoppingApi.Services
{
    public class EfSqlProducts : ILookupProducts, IProductCommands
    {
        private readonly ShoppingDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;
        private readonly IOptions<ConfigurationForPricing> _pricingOptions;


        public EfSqlProducts(ShoppingDataContext context, IMapper mapper, MapperConfiguration mapper_config, IOptions<ConfigurationForPricing> pricingOptions)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapper_config;
            _pricingOptions = pricingOptions;
        }

        public async Task<GetProductDetailsResponse> Add(PostProductRequest productToAdd)
        {
            var product = _mapper.Map<Product>(productToAdd);

            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Name == productToAdd.Name);
            if (category == null)
            {
                var newCategory = new Category { Name = productToAdd.Category };
                _context.Categories.Add(newCategory);
                product.Category = newCategory;
            }
            else
            {
                product.Category = category;
            }
            //product.Price = productToAdd.Cost.Value * 1.10M;
            product.Price = productToAdd.Cost.Value * _pricingOptions.Value.MarkUp;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<GetProductDetailsResponse>(product);
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
