create function get_customers_purchases(customer_id integer) 
    returns setof sales
as $$
begin
    return query (
        select * 
        from sales s 
        where s.customer_id = get_customers_purchases.customer_id
    );
end;
$$ language plpgsql;
