create or replace function get_components_by_category(category_id integer)
    returns setof components
as $$
begin
    return query (
        select *
        from components c
        where c.component_category_id = category_id
    );
end;
$$ language plpgsql;
