---- create trigger procedures
create or replace function delete_sale_position()
    returns trigger
as $$
begin
    -- add row to "deleted" table
    insert into deleted.sale_positions (id, sale_id, computer_configuration_id, cost, discount_percentage, warranty_period)
    values (old.id, old.sale_id, old.computer_configuration_id, old.cost, old.discount_percentage, old.warranty_period)
    on conflict (id) do nothing;
    
    return old;
end;
$$ language plpgsql;

---- create triggers
create trigger delete_sale_positions
    after delete
    on sale_positions
    for each row
execute function delete_sale_position();

create trigger log_change
    after insert or update or delete
    on sale_positions
    for each row
execute function log();
