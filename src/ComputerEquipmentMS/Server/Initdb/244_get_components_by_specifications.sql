create or replace function get_components_by_specifications(_specifications jsonb) 
    returns setof components
as $$
begin
    return query (
        select * 
        from components c
        where c.specifications @> _specifications
    );
end;
$$ language plpgsql;
