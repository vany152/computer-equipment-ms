create or replace function get_configurations_component_ids(_configuration_id integer) 
    returns integer[]
as $$
    declare
        current_component_id integer;
        component_ids integer[];
begin
    for current_component_id in 
        select component_id 
        from components_computer_configurations 
        where computer_configuration_id = _configuration_id 
    loop
        component_ids = array_append(component_ids, current_component_id);
    end loop;

    return component_ids;
end;
$$ language plpgsql;
