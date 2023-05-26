﻿using Server.DataAccess;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentCategoriesRepositoryTests;

public abstract class ComponentCategoriesRepositoryTestBase : RepositoryTestBase<ComponentCategory, int>
{
    protected ComponentCategoriesRepositoryTestBase()
    {
        Repository = new ComponentCategoriesNpgsqlRepository(ConnectionString);
    }
    
    protected override void FillDbWithTestData() =>
        Container.ExecScriptAsync(
            """
            select create_component_category('video cards');
            select create_component_category('processors');
            select create_component_category('hard drives');
            """
        ).Wait();
}