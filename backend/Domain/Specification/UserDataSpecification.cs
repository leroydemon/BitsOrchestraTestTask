using Domain.Filters;
using Domain.Entity;
using Domain.Enum;
using System.Linq.Expressions;
using Domain.SortableFields;

namespace Domain.Specification
{
    public class UserDataSpecification : SpecificationBase<UserData>
    {
        public UserDataSpecification(UserDataFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                ApplyFilter(u => u.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.Phone))
            {
                ApplyFilter(u => u.Phone.Contains(filter.Phone));
            }

            if (filter.Salary.HasValue)
            {
                ApplyFilter(u => u.Salary == filter.Salary.Value);
            }

            if (filter.DateOfBirth.HasValue)
            {
                ApplyFilter(u => u.DateOfBirth == filter.DateOfBirth.Value);
            }

            if (filter.Married.HasValue)
            {
                ApplyFilter(u => u.Married == filter.Married.Value);
            }
            if (filter.Married.HasValue)
            {
                ApplyFilter(u => u.Married == filter.Married.Value);
            }

            ApplySorting(filter.OrderBy, filter.Ascending);
            ApplyPaging(filter.Skip, filter.Take);
        }

        private void ApplySorting(UserDataSortableFields sortBy, OrderByDirection ascending)
        {
            Expression<Func<UserData, object>> orderByExpression = sortBy switch
            {
                UserDataSortableFields.Name => u => u.Name,
                UserDataSortableFields.Phone => u => u.Phone,
                UserDataSortableFields.Salary => u => u.Salary,
                UserDataSortableFields.DateOfBirth => u => u.DateOfBirth,
                UserDataSortableFields.Married => u => u.Married,
                _ => u => u.CreatedDate
            };

            if (ascending == OrderByDirection.Ascending)
            {
                ApplyOrderBy(orderByExpression);
            }
            else
            {
                ApplyOrderByDescending(orderByExpression);
            }
        }
    }
}
