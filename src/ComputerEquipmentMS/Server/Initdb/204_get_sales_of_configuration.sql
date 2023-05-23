create or replace function get_sales_of_configuration(_configuration_id integer)
    returns setof sale_position_info
as $$
begin
    return query (
        select 
            sp.id, 
            s.sale_moment, 
            sp.cost as starting_cost, 
            sp.cost * (1 - sp.discount_percentage / 100::numeric) as final_cost, 
            sp.discount_percentage, 
            sp.warranty_period
        from sale_positions sp
            join sales s on s.id = sp.sale_id
        where sp.computer_configuration_id = _configuration_id
    );
end;
$$ language plpgsql;
