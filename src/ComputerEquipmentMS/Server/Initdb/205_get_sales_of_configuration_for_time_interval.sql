create or replace function get_sales_of_configuration_for_time_interval(
    _configuration_id integer,
    _from timestamptz,
    _to timestamptz
    )
    returns setof sale_position_info
as $$
begin
    return query (
        select * from get_sales_of_configuration(_configuration_id) s
        where s.sale_moment between _from and _to  
    );
end;
$$ language plpgsql;
