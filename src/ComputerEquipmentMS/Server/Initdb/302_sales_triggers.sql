---- create trigger procedures
create or replace function delete_sale()
    returns trigger
as $$
begin
    -- add row to "deleted" table
    insert into deleted.sales (id, customer_id, sale_moment, discount_percentage)
    values (old.id, old.customer_id, old.sale_moment, old.discount_percentage)
    on conflict (id) do nothing;
    
    return old;
end;
$$ language plpgsql;

---- create triggers
create trigger log_change
    after insert or update or delete
    on sales
    for each row 
execute procedure log();

create trigger delete_sale
    after delete
    on sales
    for each row
execute procedure delete_sale();
