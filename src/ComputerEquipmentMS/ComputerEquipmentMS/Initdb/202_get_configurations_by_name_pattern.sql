create or replace function get_configurations_by_name_pattern(_name_pattern varchar) 
    returns setof computer_configurations_with_component_ids
as $$
begin
    return query (
        select *, get_configurations_component_ids(cc.id)
        from computer_configurations cc
        where cc.name ~* _name_pattern
    );
end;
$$ language plpgsql;
