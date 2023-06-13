---- create trigger procedures
create or replace function delete_components_computer_configurations_row()
    returns trigger
as $$
begin
    -- add row to "deleted" table
    insert into deleted.components_computer_configurations (component_id, computer_configuration_id)
    values (old.component_id, old.computer_configuration_id)
    on conflict (component_id, computer_configuration_id) do nothing;
    
    return old;
end;
$$ language plpgsql;

---- create triggers
create trigger delete_components_computer_configurations_row
    after delete
    on components_computer_configurations
    for each row
execute function delete_components_computer_configurations_row();

create trigger log_change
    after insert or update or delete or truncate
    on components_computer_configurations
    for each row
execute function log();
