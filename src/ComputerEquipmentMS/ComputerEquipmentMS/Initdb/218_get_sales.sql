/*
 * function are not used in other functions of "sale" group 
 * because of performance decreasing
 */

create or replace function get_sales()
    returns setof sales_with_sale_position_ids
as $$
begin
    return query (
        select s.id,
               s.customer_id,
               s.sale_moment,
               s.discount_percentage,
               get_sale_position_ids_for_sale(s.id) as sale_position_ids
        from sales as s
    );
end;
$$ language plpgsql;
