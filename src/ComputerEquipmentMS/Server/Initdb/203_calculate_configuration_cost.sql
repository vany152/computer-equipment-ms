/*
 * returns -1 if configuration with specified id does not exist
 */
create or replace function calculate_configuration_cost(_configuration_id integer)
    returns numeric(15, 2)
as $$
    declare result numeric(15, 2);
begin
    result = (
        select sum(cost) 
        from components c
            join components_computer_configurations ccc on c.id = ccc.component_id
        where ccc.computer_configuration_id = _configuration_id
    ) + (
        select margin 
        from computer_configurations
        where id = _configuration_id
    );
    
    if (result is null) then
        result = -1;
    end if;
    
    return result;
end;
$$ language plpgsql;
