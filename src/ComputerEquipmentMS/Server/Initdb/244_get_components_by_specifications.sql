create or replace function get_components_by_specifications(checking_specifications jsonb) 
    returns setof components
as $$
begin
    return query (
        select * 
        from components c
        where c.specifications @> checking_specifications
    );
end;
$$ language plpgsql;
