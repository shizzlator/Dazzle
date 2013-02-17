using System;
using System.Collections.Generic;
using DataAccess.Interfaces;

namespace DataAccess.Temp
{
    public class ContactRepository : IContactRepository
    {
        private readonly IDatabaseSession _dbSession;
        private readonly IDataQueryBuilder _dataQueryBuilder;

        public ContactRepository(IDatabaseSession dbSession, IDataQueryBuilder dataQueryBuilder)
        {
            _dbSession = dbSession;
            _dataQueryBuilder = dataQueryBuilder;
        }

        public void Create(Contact contact)
        {
            _dbSession.RunUpdateCommandFor
            (
                _dataQueryBuilder.WithCommandText("insert into contact values(@FirstName, @Surname, @Telephone)")
                .WithParam("@FirstName", contact.FirstName)
                .WithParam("@Surname", contact.Surname)
                .WithParam("@Telephone", contact.Telephone).BuildTextQuery()
             );
        }

        public Contact Get(int contactId)
        {
            var reader = _dbSession.RunReaderFor
           (
               _dataQueryBuilder.WithCommandText("select * from contact where ID = @contactId")
               .WithParam("@contactId", contactId).BuildTextQuery()
            );
            using (reader)
            {
                return reader.Read() ? new Contact() { FirstName = (string)reader["FirstName"], Surname = (string)reader["Surname"] } : null;
            }
        }

        public Contact RunQuery(string query, params QueryParameters[] queryParams)
        {
            var queryBuilder = _dataQueryBuilder.WithCommandText(query);
            foreach (var queryParam in queryParams)
            {
                queryBuilder = queryBuilder.WithParam(queryParam.Name, queryParam.Value);
            }
            var reader = _dbSession.RunReaderFor(queryBuilder.BuildTextQuery());
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