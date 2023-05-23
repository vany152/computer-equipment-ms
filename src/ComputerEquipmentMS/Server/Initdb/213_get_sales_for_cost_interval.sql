create or replace function get_sales_for_cost_interval(min_cost numeric(15, 2), max_cost numeric(15, 2)) 
    returns setof sales_with_sale_position_ids
as $$
begin
    return query (
        select *, get_sale_position_ids_for_sale(s.id)
        from sales s
        where calculate_sale_cost(s.id) between min_cost and max_cost
    );
end;
$$ language plpgsql;
