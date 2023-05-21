create or replace function create_component_manufacturer(_manufacturer_name varchar(50))
    returns component_categories
as $$
declare new_component_manufacturer component_manufacturers;
begin
    insert into component_manufacturers (name)
    values (_manufacturer_name)
    returning * into new_component_manufacturer;

    return new_component_manufacturer;
end;
$$ language plpgsql;
