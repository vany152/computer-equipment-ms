create or replace function get_customers_by_name_pattern(name_pattern varchar)
    returns setof customers
as $$
begin
    return query (
        select *
        from customers c
        where c.name ~* name_pattern
    );
end;
$$ language plpgsql;
