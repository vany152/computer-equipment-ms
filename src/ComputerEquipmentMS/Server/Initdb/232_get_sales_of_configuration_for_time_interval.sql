create or replace function get_sales_of_configuration_for_time_interval(
    configuration_id integer,
    "from" timestamptz,
    "to" timestamptz
    )
    returns table (
        sale_position_id    integer,
        starting_cost       numeric(15, 2),
        final_cost          numeric(15, 2),
        discount_percentage smallint,
        warranty_period     interval
    )
as $$
begin
    return query (
        select * from get_sales_of_configuration(configuration_id)
        where sale_moment between "from" and "to"  
    );
end;
$$ language plpgsql;
