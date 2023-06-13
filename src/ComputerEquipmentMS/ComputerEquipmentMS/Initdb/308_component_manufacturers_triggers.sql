---- create trigger procedures
create or replace function delete_component_manufacturer()
    returns trigger
as $$
begin
    -- add row to "deleted" table
    insert into deleted.component_manufacturers (id, name)
    values (old.id, old.name)
    on conflict (id) do nothing;
    
    return old;
end;
$$ language plpgsql;

---- create triggers
create trigger delete_component_category
    after delete
    on component_manufacturers
    for each row
execute function delete_component_manufacturer();

create trigger log_change
    after insert or update or delete or truncate
    on component_manufacturers
    for each row
execute function log();
