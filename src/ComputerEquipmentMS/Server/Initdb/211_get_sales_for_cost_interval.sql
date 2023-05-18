create function get_sales_for_cost_interval(min_cost numeric(15, 2), max_cost numeric(15, 2)) 
    returns setof sales
as $$
begin
    return query (
        select * 
        from sales s
        where calculate_sale_cost(s.id) between min_cost and max_cost
    );
end;
$$ language plpgsql;
