using Server.DataAccess;

namespace Server.Test.TestBase;

public abstract class NpgsqlStoredFunctionExecutorTestBase : TestBase
{
    protected readonly NpgsqlStoredFunctionsExecutor Executor;

    protected NpgsqlStoredFunctionExecutorTestBase()
    {
        Executor = new NpgsqlStoredFunctionsExecutor(ConnectionString);
    }
}