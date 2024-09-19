using Domain.Enum;
using Domain.SortableFields;
using System.Text.Json.Serialization;

namespace Domain.Filters
{
    public class UserDataFilter
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public decimal? Salary { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool? Married { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserDataSortableFields OrderBy { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderByDirection Ascending { get; set; } = OrderByDirection.Ascending;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 100;
    }
}
