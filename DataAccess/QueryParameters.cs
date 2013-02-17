namespace DataAccess
{
    public class QueryParameters
    {
        public QueryParameters(string parameterName, string value)
        {
            Name = parameterName;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}