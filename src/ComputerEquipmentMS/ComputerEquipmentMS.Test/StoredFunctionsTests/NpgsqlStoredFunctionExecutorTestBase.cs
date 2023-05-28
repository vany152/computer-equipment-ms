using ComputerEquipmentMS.DataAccess;

namespace ComputerEquipmentMS.Test.StoredFunctionsTests;

public abstract class NpgsqlStoredFunctionExecutorTestBase : TestBase
{
    protected readonly NpgsqlStoredFunctionsExecutor Executor;

    protected NpgsqlStoredFunctionExecutorTestBase()
    {
        Executor = new NpgsqlStoredFunctionsExecutor(ConnectionString);
    }
    
    
    
    public sealed override Task InitializeAsync() => 
        Container.ExecScriptAsync(scriptContent: ConstructFillDbWithTestBataScript());

    protected abstract string ConstructFillDbWithTestBataScript();
}