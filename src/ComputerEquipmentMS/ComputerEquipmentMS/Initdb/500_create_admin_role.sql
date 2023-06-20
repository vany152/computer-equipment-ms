begin;

create user "admin" with password 'admin';
grant usage on schema public to "admin";

revoke all on all tables in schema public from "admin";
grant select, insert, update, delete on
    public.customers,
    public.sales,
    public.sale_positions,
    public.computer_configurations,
    public.components_computer_configurations,
    public.components,
    public.component_categories,
    public.component_manufacturers,
    public.change_logs
    to "admin";

grant all privileges on
    public.computer_configurations_with_component_ids,
    public.sale_position_info,
    public.sales_with_sale_position_ids
    to "admin";

grant usage on sequence
    customers_id_seq,
    sales_id_seq,
    sale_positions_id_seq,
    computer_configurations_id_seq,
    components_id_seq,
    component_categories_id_seq, 
    component_manufacturers_id_seq,
    change_logs_id_seq
    to "admin"; 

revoke execute on all functions in schema public from "admin";
grant execute on function 
    create_configuration(varchar, interval, numeric, integer[]),
    get_configurations_component_ids(integer),
    get_configurations_by_name_pattern(varchar),
    calculate_configuration_cost(integer),
    get_sales_of_configuration(integer),
    get_sales_of_configuration_for_time_interval(integer, timestamptz, timestamptz),
    edit_configuration(integer, varchar, interval, numeric, integer[]),

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
    get_components_by_specifications(jsonb),
    
    get_change_log_for_user_and_interval(text, timestamptz, timestamptz),
    get_change_log_for_action_and_interval(text, timestamptz, timestamptz)
    to "admin";


revoke all on all tables in schema deleted from "admin";
grant usage on schema deleted to "admin";
grant select, insert, delete, truncate on
    deleted.customers,
    deleted.sales,
    deleted.sale_positions,
    deleted.computer_configurations,
    deleted.components_computer_configurations,
    deleted.components,
    deleted.component_categories,
    deleted.component_manufacturers
    to "admin";

commit;
