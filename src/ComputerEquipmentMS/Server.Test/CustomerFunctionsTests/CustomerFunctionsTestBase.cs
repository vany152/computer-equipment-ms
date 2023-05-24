using Server.Test.TestBase;

namespace Server.Test.CustomerFunctionsTests;

public class CustomerFunctionsTestBase : NpgsqlStoredFunctionExecutorTestBase
{
    protected sealed override string ConstructFillDbWithTestBataScript() =>
        """
        select create_customer('Oleg', null, '2021-05-30'::date);
        select create_customer('Anna', '{"Email": "annagmail@gmail.com"}'::jsonb, '2020-01-02'::date);
        select create_customer('Alexander', '{"Phone": "8-800-555-35-35"}'::jsonb, '2022-07-15'::date);

        select create_component_category('video cards');
        select create_component_category('processors');
        select create_component_category('hard drives');

        select create_component_manufacturer('nvidia');
        select create_component_manufacturer('intel');
        select create_component_manufacturer('samsung');

        select create_component(1, 1, 'rtx 4090', '{}'::jsonb, 90000, '12 months'::interval);
        select create_component(2, 2, 'intel core i7 12700', '{}'::jsonb, 30000, '12 months'::interval);
        select create_component(3, 3, 'samsung 980 evo ssd', '{}'::jsonb, 15000, '30 months'::interval);

        select create_configuration('configuration 1', '1 year'::interval, 1000, '{1, 2}');
        select create_configuration('configuration 2', '2 years'::interval, 1500, '{1, 3}');

        select create_sale(1, '2021-05-30 12:00:00 +00'::timestamptz, 0::smallint, '{1}'::integer[], null);
        select create_sale(2, '2020-01-02 12:00:00 +00'::timestamptz, 5::smallint, '{1, 2}'::integer[], null);
        select create_sale(1, '2022-05-30 12:00:00 +00'::timestamptz, 10::smallint, '{1, 2, 2}'::integer[], '{5, 0, 15}'::smallint[]);
        """;
}