/*
 * if _configuration_discounts is null, then discount on all sale positions will be 0
 */
create or replace function create_sale(
    _customer_id integer, 
    _sale_moment timestamptz, 
    _general_discount_percentage smallint,
    _configuration_ids integer[],
    _configuration_discounts smallint[]
    ) 
    returns sales_with_sale_position_ids
as $$
    declare 
        new_sale sales_with_sale_position_ids;
        
        number_of_configurations integer;
        number_of_discounts integer;
        current_configuration computer_configurations;
        
        current_configuration_id integer;
        current_configuration_discount smallint;
        current_configuration_cost numeric(15, 2);
begin
    number_of_configurations = array_length(_configuration_ids, 1);

    if (_configuration_discounts is not null) then
        number_of_discounts = array_length(_configuration_discounts, 1);
        
        if (number_of_configurations != number_of_discounts) then
            raise exception 'number of configurations % is different from number of discounts %', number_of_configurations, number_of_discounts;
        end if;
    else
        _configuration_discounts = array_fill(0, array[number_of_configurations]);
    end if;
        
    insert into sales (customer_id, sale_moment, discount_percentage) 
    values (_customer_id, _sale_moment, _general_discount_percentage)
    returning *, _configuration_ids into new_sale;
    
    for i in 1..number_of_configurations loop
        current_configuration_id = _configuration_ids[i];
        current_configuration_discount = _configuration_discounts[i];
        
        select * into current_configuration from computer_configurations where id = current_configuration_id;
        current_configuration_cost = (select * from calculate_configuration_cost(current_configuration.id));
        
        insert into sale_positions (sale_id, computer_configuration_id, cost, discount_percentage, warranty_period)
        values (new_sale.id, current_configuration.id, current_configuration_cost, current_configuration_discount, current_configuration.warranty_period);
    end loop;
    
    return new_sale;
end;
$$ language plpgsql;
