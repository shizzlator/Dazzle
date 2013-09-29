using System.Data;
using System.Data.SqlClient;
using DataAccess.Interfaces;
using DataAccess.Query;
using ExampleUsages.DTOs;

namespace ExampleUsages.Repositories
{
    public class ContactRepository : IRepository
    {
        private readonly IDatabaseSession _databaseSession;

        public ContactRepository(IDatabaseSession databaseSession)
        {
            _databaseSession = databaseSession;
        }

        //SIMPLE INSERT - using Stored Proc (note the DataQueryBuilder method BuildStoredQuery())
        public int Create(Contact contact)
        {
            //In this example it would be the Sproc responsibility to pass back ID, prob using SCOPE_IDENTITY()
            return (int)_databaseSession.ExecuteScalar
            (
                _databaseSession.CreateQuery()
                .WithStoredProc("CreateContact")
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

            var dataQuery = _databaseSession.CreateQuery()
                .WithQueryText("create_contact")
                .WithParam("@ContactId", contactIdOutputParam)
                .WithParam("@FirstName", contact.FirstName)
                .WithParam("@Surname", contact.Surname)
                .WithParam("@Telephone", contact.Telephone);

            //Need to cast it to an SqlParameter as the Mapping of data types to the generic DataParameter is whack.
            //TODO: An SqlInputOutputParameter class would end this fuckery.
            contact.Id = (int)((SqlParameter)_databaseSession.ExecuteUpdate(dataQuery, "@ContactId")).Value;
            return contact;
        }

        //SIMPLE GET - Using DataQueryBuilder
        public Contact Get(int contactId)
        {
            var reader = _databaseSession.ExecuteReader
           (
               _databaseSession.CreateQuery()
               .WithQueryText("select * from contact where ID = @contactId")
               .WithParam("@contactId", contactId)
            );
            using (reader)
            {
                return reader.Read() ? new Contact()
                    {
                        FirstName = reader.Get<string>("FirstName"),
                        Surname = reader.Get<string>("Surname"),
                        Telephone = reader.Get<string>("Telephone")
                    } : null;
            }
        }

        //INLINE PARAMETERISED QUERY
        public Contact RunQuery(string queryText, params QueryParameters[] queryParams)
        {
            var dataQuery = _databaseSession.CreateQuery().WithQueryText(queryText);
            foreach (var queryParam in queryParams)
            {
                dataQuery = dataQuery.WithParam(queryParam.Name, queryParam.Value);
            }
            using (var reader = _databaseSession.ExecuteReader(dataQuery))
            {
                return reader.Read() ? new Contact()
                    {
                        FirstName = reader.Get<string>("FirstName"), 
                        Surname = reader.Get<string>("Surname"),
                        Telephone = reader.Get<string>("Telephone")
                    } : null;
            }
        }

        //QUERY - Using DataQuery class
        public Contact RunQuery(IDataQuery dataQuery)
        {
            var reader = _databaseSession.ExecuteReader(dataQuery);
            using (reader)
            {
                return reader.Read() ? new Contact()
                {
                    FirstName = reader.Get<string>("FirstName"),
                    Surname = reader.Get<string>("Surname"),
                    Telephone = reader.Get<string>("Telephone")
                } : null;
            }
        }

        public void Save(Contact contact)
        {
            //This is where I would save my Contact using update(has Id) or create (no Id)
        }
    }
}