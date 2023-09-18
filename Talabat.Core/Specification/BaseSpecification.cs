using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class BaseSpecification<T> : ISepcification<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get; set; }
		public List<Expression<Func<T, object>>> Includes { get; set; } =new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderBy { get; set; }
		public Expression<Func<T, object>> OrderByDesc { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; }
		public bool IsPaginationEnbled { get; set; }

		public BaseSpecification()
		{
			//Includes = new List<Expression<Func<T, object>>>();
			
			//Includes.Add(P => P.ProductBrand);
		}

		public BaseSpecification(Expression<Func<T,bool>> criteriaExpression)
		{
			Criteria = criteriaExpression;
			//Includes = new List<Expression<Func<T, object>>>();
		}

		public void AddOrderBy(Expression<Func<T, object>> expression)
		{
			OrderBy = expression;
		}

		public void AddOrderByDesc (Expression<Func<T,object>> expression)
		{
			OrderByDesc = expression;
		}

		public void AddPagination(int skip,int take)
		{
			IsPaginationEnbled = true;
			Skip = skip;
			Take = take;
		}
	}


}
