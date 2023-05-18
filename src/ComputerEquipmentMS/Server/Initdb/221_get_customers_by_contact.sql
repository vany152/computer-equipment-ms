create or replace function get_customers_by_contact(contact jsonb) 
    returns setof customers
as $$
begin
    return query (
        select * 
        from customers c
        where c.contacts @> contact
    );
end;
$$ language plpgsql;
