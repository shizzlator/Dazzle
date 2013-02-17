namespace DataAccess.Interfaces
{
    public interface IDataQueryBuilder
    {
        IDataQueryBuilder WithParam(string name, object value);
        IDataQueryBuilder WithCommandText(string commandText);
        IDataQuery BuildTextQuery();
        IDataQuery BuildStoredQuery();
    }
}