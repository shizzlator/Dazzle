       _,,--,,_        
     /`       .`\      
    /  '  _.-'   \     
    |  `'_{}_    |     
    |  /`    `\  |     
     \/ ==  == \/      
     /| (.)(.) |\      The Swamy says:
     \|  __)_  |/      READ THIS SHIZZLE!!
      |\/____\/|       
      | ` ~~ ` |       
      \        /       
       `.____.`   

Below is an example repository. (also found in the library project). 
It shows a few ways in which you can execute queries.

If you want to run things within a transaction then you should use the UnitOfWork class:
- This will wrap anything you do with a transaction - _unitOfWork.Repository<MyRepo>().MyMethod()
- Remember to call Commit()/Rollback() on the UnitOfWork when you are done.
- There is also a TransactionalTestFixture included.
- Inherit from this if you want your integration tests to rollback when they are complete (override the connection string prop)
- HAVE A LOOK AT THE ACTUAL DATAACCES PROJECT https://github.com/shizzlator/DataAccess for a better understanding!


public class ContactRepository : IContactRepository, IRepository
    {
        private readonly IDatabaseSession _dbSession;
        private readonly IDataQueryBuilder _dataQueryBuilder;

        public ContactRepository(IDatabaseSession dbSession, IDataQueryBuilder dataQueryBuilder)
        {
            _dbSession = dbSession;
            _dataQueryBuilder = dataQueryBuilder;
        }

        //SIMPLE INSERT - using Stored Proc (note the DataQueryBuilder method BuildStoredQuery())
        public int Create(Contact contact)
        {
            //In this example it would be the Sproc responsibility to pass back ID, prob using SCOPE_IDENTITY()
            return (int)_dbSession.RunScalarCommandFor
            (
                _dataQueryBuilder.WithCommandText("create_contact")
                .WithParam("@FirstName", contact.FirstName)
                .WithParam("@Surname", contact.Surname)
                .WithParam("@Telephone", contact.Telephone).BuildStoredQuery()
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

            var dataQuery = _dataQueryBuilder
                .WithCommandText("create_contact")
                .WithParam("@ContactId", contactIdOutputParam)
                .WithParam("@FirstName", contact.FirstName)
                .WithParam("@Surname", contact.Surname)
                .WithParam("@Telephone", contact.Telephone)
                .BuildTextQuery();

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
               _dataQueryBuilder.WithCommandText("select * from contact where ID = @contactId")
               .WithParam("@contactId", contactId).BuildTextQuery()
            );
            using (reader)
            {
                return reader.Read() ? new Contact() { FirstName = (string)reader["FirstName"], Surname = (string)reader["Surname"], Telephone = (string)reader["Telephone"]} : null;
            }
        }

        //INLINE PARAMETERISED QUERY
        public Contact RunQuery(string query, params QueryParameters[] queryParams)
        {
            var queryBuilder = _dataQueryBuilder.WithCommandText(query);
            foreach (var queryParam in queryParams)
            {
                queryBuilder = queryBuilder.WithParam(queryParam.Name, queryParam.Value);
            }
            using (var reader = _dbSession.RunReaderFor(queryBuilder.BuildTextQuery()))
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