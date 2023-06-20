---- create trigger procedures
create or replace function delete_customer()
    returns trigger
as $$
begin
    -- add row to "deleted" table
    insert into deleted.customers (id, name, contacts, registration_date)
    values (old.id, old.name, old.contacts, old.registration_date)
    on conflict (id) do nothing;
    
    return old;
end;
$$ language plpgsql;

---- create triggers
create trigger log_change
    after insert or update or delete 
    on customers
    for each row
execute procedure log();

create trigger delete_customer
    after delete
    on customers
    for each row
execute procedure delete_customer();
