create or replace function get_sales_of_configuration_for_time_interval(
    configuration_id integer,
    "from" timestamptz,
    "to" timestamptz
    )
    returns setof sale_position_info
as $$
begin
    return query (
        select * from get_sales_of_configuration(configuration_id) s
        where s.sale_moment between "from" and "to"  
    );
end;
$$ language plpgsql;
