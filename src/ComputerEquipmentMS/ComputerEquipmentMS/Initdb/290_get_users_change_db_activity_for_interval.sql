create or replace function get_change_log_for_user_and_interval(
    _username text, 
    _from timestamptz, 
    _to timestamptz
    ) 
    returns setof change_logs
as $$
begin
    return query (
        select * 
        from change_logs cl
        where cl.username = _username and
            cl.moment between _from and _to
    );
end;
$$ language plpgsql;
