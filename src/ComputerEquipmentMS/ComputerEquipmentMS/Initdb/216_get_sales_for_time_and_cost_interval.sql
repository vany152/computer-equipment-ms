create or replace function get_sales_for_time_and_cost_interval(
    _from timestamptz, _to timestamptz, 
    _min_cost numeric(15, 2), _max_cost numeric(15, 2)
    )
    returns setof sales_with_sale_position_ids
as $$
begin
    return query (
        select * from get_sales_for_time_interval(_from, _to)
        intersect
        select * from get_sales_for_cost_interval(_min_cost, _max_cost)
    );
end;
$$ language plpgsql;
