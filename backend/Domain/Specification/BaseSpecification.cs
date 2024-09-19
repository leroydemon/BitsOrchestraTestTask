using Domain.Interfaces;
using System.Linq.Expressions;

namespace Domain.Specification
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {

        public virtual List<Expression<Func<T, bool>>> Criterias { get; } = new();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; set; }

        protected ISpecification<T> ApplyFilter(Expression<Func<T, bool>> expr)
        {
            Criterias.Add(expr);

            return this;
        }

        protected void ApplyPaging(int skip, int take)
        {
            if (skip <= 0 && take <= 0)
            {
                IsPagingEnabled = false; 
                return;
            }

            Skip = Math.Max(0, (skip - 1) * take);
            Take = Math.Max(1, take); 
            IsPagingEnabled = true;
        }

        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) =>
            OrderBy = orderByExpression;

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) =>
            OrderByDescending = orderByDescendingExpression;

        protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression) =>
            GroupBy = groupByExpression;
    }
}
