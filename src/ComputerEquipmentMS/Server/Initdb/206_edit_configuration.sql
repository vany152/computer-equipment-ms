create or replace function edit_configuration(
    _id integer,
    _new_name varchar(250),
    _new_warranty_period interval,
    _new_margin numeric(15, 2),
    _new_component_ids integer[]
    ) returns computer_configurations_with_component_ids
as $$
    declare edited_configuration computer_configurations;
begin
    if (array_length(_new_component_ids, 1) < 1) then
        raise exception 'configuration cannot consist of from zero number components';
    end if;
    
    update computer_configurations
    set name = _new_name,
        warranty_period = _new_warranty_period,
        margin = _new_margin
    where id = _id 
    returning *, _new_component_ids into edited_configuration;
    
    delete from components_computer_configurations
    where computer_configuration_id = _id;

    insert into components_computer_configurations (component_id, computer_configuration_id)
        select current_component_id, edited_configuration.id
        from unnest(_new_component_ids) as current_component_id;

    return edited_configuration;
    
end;
$$ language plpgsql;
