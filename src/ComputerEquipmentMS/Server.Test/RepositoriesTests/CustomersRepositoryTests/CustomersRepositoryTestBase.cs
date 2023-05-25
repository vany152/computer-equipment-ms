using Server.DataAccess;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.CustomersRepositoryTests;

public abstract class CustomersRepositoryTestBase : RepositoryTestBase<Customer, int>
{
    protected CustomersRepositoryTestBase()
    {
        Repository = new CustomersNpgsqlRepository(ConnectionString);
    }
    
    protected override void FillDbWithTestData() =>
        Container.ExecScriptAsync(
            """
                select create_customer('Oleg', null, '2021-05-30'::date);
                select create_customer('Anna', '{"Email": "annagmail@gmail.com"}'::jsonb, '2020-01-02'::date);
                select create_customer('Alexander', '{"Phone": "8-800-555-35-35"}'::jsonb, '2022-07-15'::date);
                select create_customer('Natali', '{"Phone": "8-800-500-30-30", "Email": "annagmail@gmail.com"}'::jsonb, '2022-07-15'::date);
                """
        ).Wait();
}