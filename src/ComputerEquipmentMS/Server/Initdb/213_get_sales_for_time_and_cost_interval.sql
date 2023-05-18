create or replace function get_sales_for_time_and_cost_interval(
    "from" timestamptz, "to" timestamptz, 
    min_cost numeric(15, 2), max_cost numeric(15, 2)
    )
    returns setof sales
as $$
begin
    return query (
        select * from get_sales_for_time_interval("from", "to")
        intersect
        select * from get_sales_for_cost_interval(min_cost, max_cost)
    );
end;
$$ language plpgsql;
