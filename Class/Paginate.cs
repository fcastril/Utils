using System.Collections.Generic;

namespace Util.Common
{
    public class Paginate<T> where T : class, new()
    {
        public Paginate()
        {
        }
        public int Count { get; set; }
        public int Page { get; set; }
        public LogicalOperators Operator { get; set; }
        public List<FilterPaginate> FiltersPaginate { get; set; }
        public int RowsTotal { get; set; }
        public int PagesTotal { get; set; }
        public List<T> Data { get; set; }
    }
}
