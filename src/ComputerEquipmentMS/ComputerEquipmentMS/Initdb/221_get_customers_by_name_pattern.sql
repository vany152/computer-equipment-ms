create or replace function get_customers_by_name_pattern(_name_pattern varchar)
    returns setof customers
as $$
begin
    return query (
        select *
        from customers c
        where c.name ~* _name_pattern
    );
end;
$$ language plpgsql;
