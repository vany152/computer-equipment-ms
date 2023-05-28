create or replace function get_customers_purchases(_customer_id integer, _from timestamptz, _to timestamptz)
    returns setof sales_with_sale_position_ids
as $$
begin
    return query (
        select *
        from get_customers_purchases(_customer_id)
        where sale_moment between _from and _to
    );
end;
$$ language plpgsql;
