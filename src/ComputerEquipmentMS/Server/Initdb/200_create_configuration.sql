create or replace function create_configuration(
    _name varchar(250),
    _warranty_period interval,
    _margin numeric(15, 2),
    _component_ids integer[]
    ) 
    returns computer_configurations
as $$
    declare new_configuration computer_configurations;
begin  
    if (array_length(_component_ids, 1) < 1) then
        raise exception 'cannot create configuration from zero components';
    end if;
    
    insert into computer_configurations (name, warranty_period, margin)
    values (_name, _warranty_period, _margin)
    returning * into new_configuration;
     
    insert into components_computer_configurations (component_id, computer_configuration_id) 
        select current_component_id, new_configuration.id
        from unnest(_component_ids) as current_component_id; 
    
    return new_configuration;
end;
$$ language plpgsql;
