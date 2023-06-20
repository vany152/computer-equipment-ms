create or replace function get_components_by_name_pattern(_name_pattern varchar) 
    returns setof components
as $$
begin
    return query (
        select *
        from components c
        where c.name ~* _name_pattern
    );
end;
$$ language plpgsql;
