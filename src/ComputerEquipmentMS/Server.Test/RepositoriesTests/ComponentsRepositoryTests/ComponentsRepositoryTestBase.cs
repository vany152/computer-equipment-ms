﻿using Server.DataAccess;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentsRepositoryTests;

public abstract class ComponentsRepositoryTestBase : RepositoryTestBase<Component, int>
{
    protected ComponentsRepositoryTestBase()
    {
        Repository = new ComponentsNpgsqlRepository(ConnectionString);
    }
    
    protected override void FillDbWithTestData() =>
        Container.ExecScriptAsync(
            """
            select create_component_category('video cards');
            select create_component_category('processors');
            select create_component_category('hard drives');

            select create_component_manufacturer('nvidia');
            select create_component_manufacturer('intel');
            select create_component_manufacturer('samsung');

            select create_component(1, 1, 'rtx 4090', '{"fans": "3", "mass": "2 kg"}'::jsonb, 90000, '12 months'::interval);
            select create_component(2, 2, 'intel core i7 12700', '{"power": "100 wt", "mass": "50 g"}'::jsonb, 30000, '12 months'::interval);
            select create_component(3, 3, 'samsung 980 evo ssd', '{"capacity": "980 gb", "reading speed": "3500 mb/sec", "mass": "50 g"}', 15000, '5 years'::interval);
            """
        ).Wait();
}