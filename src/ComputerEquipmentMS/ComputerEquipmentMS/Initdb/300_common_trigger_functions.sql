create or replace function log()
    returns trigger
as $$
begin
    insert into change_logs (moment, username, action, "table", old_value, new_value)
    values (current_timestamp, current_user, tg_op, tg_table_name, old::text, new::text);
    
    return null;
end;
$$ language plpgsql;
