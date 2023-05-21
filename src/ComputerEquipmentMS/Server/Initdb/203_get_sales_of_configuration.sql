create or replace function get_sales_of_configuration(configuration_id integer)
    returns table (
        sale_position_id    integer,
        sale_moment         timestamptz,
        starting_cost       numeric(15, 2),
        final_cost          numeric(15, 2),
        discount_percentage smallint,
        warranty_period     interval
    )
as $$
begin
    return query (
        select 
            sp.id, 
            s.sale_moment, 
            sp.cost, 
            sp.cost * sp.discount_percentage / 100, 
            sp.discount_percentage, 
            sp.warranty_period
        from sale_positions sp
            join sales s on s.id = sp.sale_id
        where sp.computer_configuration_id = configuration_id
    );
end;
$$ language plpgsql;
