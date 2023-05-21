create or replace function get_sales_for_time_interval("from" timestamptz, "to" timestamptz)
    returns setof sales
as $$
begin
    return query (
        select *
        from sales s 
        where s.sale_moment between "from" and "to"
    );
end;
$$ language plpgsql;
