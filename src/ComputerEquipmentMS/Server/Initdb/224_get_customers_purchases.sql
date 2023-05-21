create or replace function get_customers_purchases(customer_id integer, "from" timestamptz, "to" timestamptz)
    returns setof sales
as $$
begin
    return query (
        select *
        from get_customers_purchases(get_customers_purchases.customer_id)
        where sale_moment between "from" and "to"
    );
end;
$$ language plpgsql;
