create or replace function calculate_configuration_cost(configuration_id integer)
    returns numeric(15, 2)
as $$
begin
    return (
        select sum(cost) 
        from components c
            join components_computer_configurations ccc on c.id = ccc.component_id
        where ccc.computer_configuration_id = configuration_id
    ) + (
        select margin 
        from computer_configurations
        where id = configuration_id
    );
end;
$$ language plpgsql;