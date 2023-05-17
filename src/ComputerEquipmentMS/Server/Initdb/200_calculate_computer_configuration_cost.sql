create or replace function 
    calculate_configuration_cost(configuration_id integer)
    returns numeric
as $$
begin
    return (
        select sum(cost) 
        from sale_positions 
        where computer_configuration_id = configuration_id
    );
end;
$$ language plpgsql;
