create or replace function get_change_log_for_action_and_interval(
    action text,
    "from" timestamptz,
    "to" timestamptz
    )
    returns setof change_logs
as $$
begin
    return query (
        select *
        from change_logs cl
        where cl.action = get_change_log_for_action_and_interval.action and
            cl.moment between "from" and "to"
    );
end;
$$ language plpgsql;
