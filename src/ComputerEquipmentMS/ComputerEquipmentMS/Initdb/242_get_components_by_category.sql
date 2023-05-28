create or replace function get_components_by_category(_category_id integer)
    returns setof components
as $$
begin
    return query (
        select *
        from components c
        where c.component_category_id = _category_id
    );
end;
$$ language plpgsql;
