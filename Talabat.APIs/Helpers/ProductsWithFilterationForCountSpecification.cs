using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.APIs.Helpers
{
    public class ProductsWithFilterationForCountSpecification:BaseSpecification<Product>
	{
		public ProductsWithFilterationForCountSpecification(ProductSpecParams specParams):base(P=>
		(string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) && 
		(!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value) &&
		(!specParams.TypeId.HasValue || P.ProductTypeId == specParams.TypeId.Value))
		{

		}
	}
}
