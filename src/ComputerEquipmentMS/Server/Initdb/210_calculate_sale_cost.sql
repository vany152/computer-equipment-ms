create or replace function calculate_sale_cost(_sale_id integer) 
    returns numeric(15, 2)
as $$
begin    
    return (
        select sum(cost - cost * discount_percentage / 100)
        from sale_positions sp
        where sp.sale_id = _sale_id
    ) * (1::numeric - (
        select discount_percentage
        from sales
        where id = _sale_id
    ) / 100::numeric);
end;
$$ language plpgsql;
