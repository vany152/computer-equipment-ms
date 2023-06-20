create or replace function get_customers_by_contact(_contact jsonb) 
    returns setof customers
as $$
begin
    return query (
        select * 
        from customers c
        where c.contacts @> _contact
    );
end;
$$ language plpgsql;
