create or replace function create_component(
    _component_category_id integer,
    _component_manufacturer_id integer,
    _name varchar,
    _specifications jsonb,
    _cost numeric(15, 2),
    _warranty_period interval
    ) 
    returns components
as $$
    declare new_component components;
begin
    insert into components (component_category_id, component_manufacturer_id, name, specifications, cost, warranty_period)
    values (_component_category_id, _component_manufacturer_id, _name, _specifications, _cost, _warranty_period)
    returning * into new_component;
    
    return new_component;
end;
$$ language plpgsql;
