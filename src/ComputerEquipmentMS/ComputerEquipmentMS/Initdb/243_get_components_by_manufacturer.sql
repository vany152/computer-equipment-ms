create or replace function get_components_by_manufacturer(_manufacturer_id integer)
    returns setof components
as $$
begin
    return query (
        select *
        from components c
        where c.component_manufacturer_id = _manufacturer_id
    );
end;
$$ language plpgsql;
