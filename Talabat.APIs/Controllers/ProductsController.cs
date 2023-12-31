﻿using AutoMapper;
using Demo.Pl.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Core.Specification;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseController
	{
		///private readonly IGenericRepository<Product> _productRepo;
		///private readonly IGenericRepository<ProductBrand> _brandRepo;
		///private readonly IGenericRepository<ProductType> _typeRepo;
		
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public ProductsController(
			IMapper mapper,
			IUnitOfWork unitOfWork
			
			///IGenericRepository<Product> productRepo
			///,IGenericRepository<ProductBrand> brandRepo
			///,IGenericRepository<ProductType> typeRepo
								)
		{
			///_productRepo = productRepo;
			///_brandRepo = brandRepo;
			///_typeRepo = typeRepo;
		
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		[Cached(500)]
		[HttpGet]
		//[Authorize]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams) 
		{
			var spec = new ProductsWithBrandAndTypeSpecifications(specParams);
			var products =await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			var countSpec = new ProductsWithFilterationForCountSpecification(specParams);
			var count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageSize,specParams.PageIndex,count,data));
		}

		[ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]//This Attributes just for improve Swagger Documentation
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [Cached(500)]
        [HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductsWithBrandAndTypeSpecifications(id);
			var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

			if (product is null) return NotFound(new ApiResponse(404));

			return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
		}
       
		[Cached(500)]
        [HttpGet("brands")]//Get: Api/Products/brands
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
			return Ok(brands);
		}

        [Cached(600)]
        [HttpGet("types")]//Get: Api/Products/types
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
		{
			var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
			return Ok(types);
		}

	}
}
