using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEnity> where TEnity: BaseEntity
	{
		public static IQueryable<TEnity> GetQuery(IQueryable<TEnity> inputQuery,ISepcification<TEnity> spec)
		{
			var query = inputQuery;//_dbcontext.Products
			if (spec.Criteria is not null)
				query = query.Where(spec.Criteria);//_dbcontext.Products.Where(x=>x.Id==id)

			if (spec.OrderBy is not null)
				query = query.OrderBy(spec.OrderBy);

			if (spec.OrderByDesc is not null)
				query = query.OrderByDescending(spec.OrderByDesc);//_dbcontext.Products.OrderByDesc(x=>x.{Name})

			if (spec.IsPaginationEnbled)
				query = query.Skip(spec.Skip).Take(spec.Take);

			//.Include
			//P=P.ProductBrand
			//P=P.ProductType
			query = spec.Includes.Aggregate(query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
			//_dbcontext.Products.{Where(x => x.Id == id)}.Include(P=P.ProductBrand)
			//_dbcontext.Products.Where(x => x.Id == id).Include(P=P.ProductBrand).Include(P=P.ProductType)
			return query;
		}
	}
}
