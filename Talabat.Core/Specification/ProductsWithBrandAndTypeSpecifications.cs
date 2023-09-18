using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductsWithBrandAndTypeSpecifications:BaseSpecification<Product>
	{
		//This constructor for GetAllProducts 	//where(P=>P.Name.Contain(Search))
		public ProductsWithBrandAndTypeSpecifications(ProductSpecParams specParams) : base(P =>
		(string.IsNullOrEmpty(specParams.Search)||P.Name.ToLower().Contains(specParams.Search))&&
		(!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value) &&
		(!specParams.TypeId.HasValue || P.ProductTypeId == specParams.TypeId.Value))

			
		{
			Includes.Add(p => p.ProductBrand);
			Includes.Add(p => p.ProductType);
			AddOrderBy(P => P.Name);//Means by default sort it by name Asc		

			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				switch (specParams.Sort)
				{
					case "priceAsc":
						AddOrderBy(P => P.Price);
						break;
					case "priceDesc":
						AddOrderByDesc(P => P.Price);
						break;
					default:
						AddOrderBy(P => P.Name);
						break;
				
				}
			}
			//products items = 18
			//skip = 3
			//take = 5
			AddPagination(specParams.PageSize*(specParams.PageIndex-1), specParams.PageSize);
		}

		public ProductsWithBrandAndTypeSpecifications(int id):base(P=>P.Id == id)
		{
			Includes.Add(p => p.ProductBrand);
			Includes.Add(p => p.ProductType);
		}
	}
}
