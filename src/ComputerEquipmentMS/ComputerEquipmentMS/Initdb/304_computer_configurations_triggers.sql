---- create trigger procedures
create or replace function delete_computer_configuration()
    returns trigger
as $$
begin
    -- add row to "deleted" table
    insert into deleted.computer_configurations (id, name, warranty_period, margin)
    values (old.id, old.name, old.warranty_period, old.margin)
    on conflict (id) do nothing;
    
    return old;
end;
$$ language plpgsql;

---- create triggers
create trigger delete_computer_configuration
    after delete
    on computer_configurations
    for each row
execute function delete_computer_configuration();

create trigger log_change
    after insert or update or delete
    on computer_configurations
    for each row
execute function log();
