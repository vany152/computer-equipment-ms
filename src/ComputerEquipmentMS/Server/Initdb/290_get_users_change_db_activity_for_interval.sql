create or replace function get_change_log_for_user_and_interval(
    username text, 
    "from" timestamptz, 
    "to" timestamptz
    ) 
    returns setof change_logs
as $$
begin
    return query (
        select * 
        from change_logs cl
        where cl.username = get_change_log_for_user_and_interval.username and
            cl.moment between "from" and "to"
    );
end;
$$ language plpgsql;
