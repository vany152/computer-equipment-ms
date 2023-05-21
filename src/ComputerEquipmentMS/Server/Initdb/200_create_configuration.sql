create or replace function create_configuration(
    _name varchar(250),
    _warranty_period interval,
    _margin numeric(15, 2),
    _component_ids integer[]
    ) 
    returns setof computer_configurations
as $$
    declare new_configuration_id integer;
begin  
    if (array_length(_component_ids, 1) < 1) then
        raise exception 'cannot create configuration from zero components';
    end if;
    
    insert into computer_configurations (name, warranty_period, margin)
    values (_name, _warranty_period, _margin)
    returning id into new_configuration_id;
     
    insert into components_computer_configurations (component_id, computer_configuration_id) 
        select current_component_id, new_configuration_id
        from unnest(_component_ids) as current_component_id; 
    
    return query (
        select * from computer_configurations
        where id = new_configuration_id
    );
end;
$$ language plpgsql;
