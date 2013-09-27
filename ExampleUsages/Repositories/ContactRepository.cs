using System.Data;
using System.Data.SqlClient;
using DataAccess;
using DataAccess.DataQuery;
using DataAccess.Interfaces;
using ExampleUsages.DTOs;
using ExampleUsages.Repositories.Interfaces;

namespace ExampleUsages.Repositories
{
    public class ContactRepository : IContactRepository, IRepository
    {
        private readonly IDatabaseSession _dbSession;

        //private readonly IDataQueryBuilder _dataQueryBuilder;

        public ContactRepository(IDatabaseSession dbSession)
        {
            _dbSession = dbSession;
        }

        //SIMPLE INSERT - using Stored Proc (note the DataQueryBuilder method BuildStoredQuery())
        public int Create(Contact contact)
        {
            //In this example it would be the Sproc responsibility to pass back ID, prob using SCOPE_IDENTITY()
            return (int)_dbSession.RunScalarCommandFor
            (
                _dbSession.CreateQuery()
                .WithQueryText("create_contact")
                .WithParam("@FirstName", contact.FirstName)
                .WithParam("@Surname", contact.Surname)
                .WithParam("@Telephone", contact.Telephone)
             );
        }

        //CREATE QUERY - Using stored proc and output param (only caters for one paramter at the moment)
        public Contact CreateContact(Contact contact)
        {
            var contactIdOutputParam = new DataParam //TODO: make an SqlInputOutputParameter class
                                           {
                                               Value = 0,
                                               Direction = ParameterDirection.InputOutput,
                                               Size = 4,
                                               Type = SqlDbType.Int
                                           };

            var dataQuery = _dbSession.CreateQuery()
                .WithQueryText("create_contact")
                .WithParam("@ContactId", contactIdOutputParam)
                .WithParam("@FirstName", contact.FirstName)
                .WithParam("@Surname", contact.Surname)
                .WithParam("@Telephone", contact.Telephone);

            //Need to cast it to an SqlParameter as the Mapping of data types to the generic DataParameter is whack.
            //TODO: An SqlInputOutputParameter class would end this fuckery.
            contact.Id = (int)((SqlParameter)_dbSession.RunUpdateCommandFor(dataQuery, "@ContactId")).Value;
            return contact;
        }

        //SIMPLE GET - Using DataQueryBuilder
        public Contact Get(int contactId)
        {
            var reader = _dbSession.RunReaderFor
           (
               _dbSession.CreateQuery()
               .WithQueryText("select * from contact where ID = @contactId")
               .WithParam("@contactId", contactId)
            );
            using (reader)
            {
                return reader.Read() ? new Contact() { FirstName = (string)reader["FirstName"], Surname = (string)reader["Surname"], Telephone = (string)reader["Telephone"]} : null;
            }
        }

        //INLINE PARAMETERISED QUERY

        public Contact RunQuery(string queryText, params QueryParameters[] queryParams)
        {
            var dataQuery = _dbSession.CreateQuery().WithQueryText(queryText);
            foreach (var queryParam in queryParams)
            {
                dataQuery = dataQuery.WithParam(queryParam.Name, queryParam.Value);
            }
            using (var reader = _dbSession.RunReaderFor(dataQuery))
            {
                return reader.Read() ? new Contact()
                    {
                        FirstName = (string)reader["FirstName"], 
                        Surname = (string)reader["Surname"], 
                        Telephone = (string)reader["Telephone"]
                    } : null;
            }
        }

        //QUERY - Using DataQuery class

        public Contact RunQuery(IDataQuery dataQuery)
        {
            var reader = _dbSession.RunReaderFor(dataQuery);
            using (reader)
            {
                return reader.Read() ? new Contact()
                {
                    FirstName = (string)reader["FirstName"],
                    Surname = (string)reader["Surname"],
                    Telephone = (string)reader["Telephone"]
                } : null;
            }
        }
    }
}