create or replace function get_components_by_manufacturer(manufacturer_id integer)
    returns setof components
as $$
begin
    return query (
        select *
        from components c
        where c.component_manufacturer_id = manufacturer_id
    );
end;
$$ language plpgsql;
