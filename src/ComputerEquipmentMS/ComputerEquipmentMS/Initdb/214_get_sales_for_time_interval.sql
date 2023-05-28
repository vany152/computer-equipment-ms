create or replace function get_sales_for_time_interval(_from timestamptz, _to timestamptz)
    returns setof sales_with_sale_position_ids
as $$
begin
    return query (
        select *, get_sale_position_ids_for_sale(s.id)
        from sales s 
        where s.sale_moment between _from and _to
    );
end;
$$ language plpgsql;
