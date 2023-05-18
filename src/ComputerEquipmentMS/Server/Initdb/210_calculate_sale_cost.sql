create or replace function calculate_sale_cost(sale_id integer) 
    returns numeric(15, 2)
as $$
begin
    return (
        select sum(cost * discount_percentage / 100)
        from sale_positions sp
        where sp.sale_id = calculate_sale_cost.sale_id
    ) * (
        select discount_percentage
        from sales
        where id = sale_id
    ) / 100;
end;
$$ language plpgsql;
