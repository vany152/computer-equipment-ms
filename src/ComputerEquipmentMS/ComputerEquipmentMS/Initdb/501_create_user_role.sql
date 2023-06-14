begin;

create user "user" with password 'user';
grant usage on schema public to "user";

revoke all on all tables in schema public from "user";
grant select, insert, update on
    public.customers
    to "user";

grant select, insert on
    public.sales,
    public.sale_positions,
    public.computer_configurations,
    public.components_computer_configurations,
    public.components,
    public.component_categories,
    public.component_manufacturers,
    public.change_logs
    to "user";

grant all privileges on
    public.computer_configurations_with_component_ids,
    public.sale_position_info,
    public.sales_with_sale_position_ids
    to "user";

grant usage on sequence
    customers_id_seq,
    sales_id_seq,
    sale_positions_id_seq,
    computer_configurations_id_seq,
    components_id_seq,
    component_categories_id_seq,
    component_manufacturers_id_seq,
    change_logs_id_seq
    to "user";

revoke execute on all functions in schema public from "user";
grant execute on function
    create_configuration(varchar, interval, numeric, integer[]),
    get_configurations_component_ids(integer),
    get_configurations_by_name_pattern(varchar),
    calculate_configuration_cost(integer),
    get_sales_of_configuration(integer),

    get_sale_position_ids_for_sale(integer),
    create_sale(integer, timestamptz, smallint, integer[], smallint[]),
    calculate_starting_sale_cost(integer),
    calculate_final_sale_cost(integer),
    get_sales_for_cost_interval(numeric, numeric),
    get_sales_for_time_interval(timestamptz, timestamptz),
    get_sales_for_time_and_cost_interval(timestamptz, timestamptz, numeric, numeric),
    get_sale(integer),
    get_sales(),

    create_customer(varchar, jsonb, date),
    get_customers_by_name_pattern(varchar),
    get_customers_by_contact(jsonb),
    get_customers_purchases(integer),
    get_customers_purchases(integer, timestamptz, timestamptz),

    create_component_category(varchar),
    create_component_manufacturer(varchar),

    create_component(integer, integer, varchar, jsonb, numeric, interval),
    get_components_by_name_pattern(varchar),
    get_components_by_category(integer),
    get_components_by_manufacturer(integer),
    get_components_by_specifications(jsonb)
    to "user";


revoke all on all tables in schema deleted from "user";
grant usage on schema deleted to "user";
grant insert on
    deleted.customers,
    deleted.sales,
    deleted.sale_positions,
    deleted.computer_configurations,
    deleted.components_computer_configurations,
    deleted.components,
    deleted.component_categories,
    deleted.component_manufacturers
    to "user";

commit;
  