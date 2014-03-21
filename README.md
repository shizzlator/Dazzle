public class HomeController : Controller
{
    private readonly IRepositoryFactory _repositoryFactory;

    public HomeController() : this(new RepositoryFactory("My Connection String"))
    {
    }

    public HomeController(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public ActionResult Index()
    {
        var user = _repositoryFactory.GetInstanceOf<UserRepository>().GetUser(465);
        return View(new UserModel(user));
    }
}

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

	//SIMPLE UPDATE - using Stored Proc (note the DataQueryBuilder method BuildStoredQuery())
    public int Update(Contact contact)
    {
        //In this example it would be the Sproc responsibility to pass back ID, prob using SCOPE_IDENTITY()
        return (int)_databaseSession.ExecuteUpdate
        (
            _databaseSession.CreateQuery()
            .WithStoredProc("UpdateContact")
            .WithParam("@FirstName", contact.FirstName)
            .WithParam("@Surname", contact.Surname)
            .WithParam("@Telephone", contact.Telephone)
            );
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
}

Example repository (also found in the library project). 
It shows a few ways in which you can execute queries.

If you want to run things within a transaction then you should use the UnitOfWork class:

- This will wrap anything you do with a transaction - _unitOfWork.Repository<MyRepo>().MyMethod()
- Remember to call Commit()/Rollback() on the UnitOfWork when you are done. EVEN for selects (as it stands).
- You could call Commit() on request end or by overriding OnActionExecuted or an equivalent event/method.
- There is also a TransactionalTestFixture included for integration tests. Just inherit from it and override ConnectionString.

