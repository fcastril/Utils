namespace Util.Common
{
    public class ItemFilters
    {
        public string Property { get; set; } = string.Empty;
        public dynamic Value { get; set; }
        public Operations Operator { get; set; }
        public LogicalOperators LogicalOperator { get; set; }
    }
}
