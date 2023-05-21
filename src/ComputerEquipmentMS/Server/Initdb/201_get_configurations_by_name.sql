create or replace function get_configurations_by_name_pattern(name_pattern varchar) 
    returns setof computer_configurations
as $$
begin
    return query (
        select * 
        from computer_configurations cc
        where cc.name ~* name_pattern
    );
end;
$$ language plpgsql;
