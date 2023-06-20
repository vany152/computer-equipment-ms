create or replace function get_sales_for_cost_interval(_min_cost numeric(15, 2), _max_cost numeric(15, 2)) 
    returns setof sales_with_sale_position_ids
as $$
begin
    return query (
        select *, get_sale_position_ids_for_sale(s.id)
        from sales s
        where calculate_final_sale_cost(s.id) between _min_cost and _max_cost
    );
end;
$$ language plpgsql;
