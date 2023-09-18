using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Core.Specification;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseController
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IGenericRepository<ProductBrand> _brandRepo;
		private readonly IGenericRepository<ProductType> _typeRepo;
		private readonly IMapper _mapper;

		public ProductsController(IGenericRepository<Product> productRepo
			,IGenericRepository<ProductBrand> brandRepo
			,IGenericRepository<ProductType> typeRepo
			,IMapper mapper)
		{
		
			_productRepo = productRepo;
			_brandRepo = brandRepo;
			_typeRepo = typeRepo;
			_mapper = mapper;
		}
		
		[HttpGet]
		//[Authorize]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams) 
		{
			var spec = new ProductsWithBrandAndTypeSpecifications(specParams);
			var products =await _productRepo.GetAllWithSpecAsync(spec);

			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			var countSpec = new ProductsWithFilterationForCountSpecification(specParams);
			var count = await _productRepo.GetCountWithSpecAsync(countSpec);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageSize,specParams.PageIndex,count,data));
		}

		[ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]//This Attributes just for improve Swagger Documentation
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductsWithBrandAndTypeSpecifications(id);
			var product = await _productRepo.GetByIdWithSpecAsync(spec);

			if (product is null) return NotFound(new ApiResponse(404));

			return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
		}

		[HttpGet("brands")]//Get: Api/Products/brands
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _brandRepo.GetAllAsync();
			return Ok(brands);
		}

		[HttpGet("types")]//Get: Api/Products/types
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
		{
			var types = await _typeRepo.GetAllAsync();
			return Ok(types);
		}

	}
}
