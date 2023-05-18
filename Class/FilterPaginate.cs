namespace Util.Common
{
    public class FilterPaginate
    {
        public FilterPaginate()
        {

        }
        public string Property { get; set; }
        public object Value { get; set; }
        public Operations Operator { get; set; }
        public LogicalOperators Conditional { get; set; } 
    }
}
