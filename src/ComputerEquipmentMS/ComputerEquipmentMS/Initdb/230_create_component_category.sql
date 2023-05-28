create or replace function create_component_category(_category_name varchar(50)) 
    returns component_categories
as $$
    declare new_component_category component_categories; 
begin
    insert into component_categories (name)
    values (_category_name)
    returning * into new_component_category;
    
    return new_component_category;
end;
$$ language plpgsql;
