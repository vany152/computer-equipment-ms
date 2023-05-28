create or replace function get_customers_purchases(_customer_id integer) 
    returns setof sales_with_sale_position_ids
as $$
begin
    return query (
        select *, get_sale_position_ids_for_sale(s.id)
        from sales s 
        where s.customer_id = _customer_id
    );
end;
$$ language plpgsql;
