---- create trigger procedures
create or replace function delete_component()
    returns trigger
as $$
begin
    -- add row to "deleted" table
    insert into deleted.components (id, component_category_id, component_manufacturer_id, name, specifications, cost, warranty_period)
    values (old.id, old.component_category_id, old.component_manufacturer_id, old.name, old.specifications, old.cost, old.warranty_period)
    on conflict (id) do nothing;
    
    return old;
end;
$$ language plpgsql;

---- create triggers
create trigger delete_component
    after delete
    on components
    for each row
execute function delete_component();

create trigger log_change
    after insert or update or delete
    on components
    for each row
execute function log();
