create view sales_view as
    select s.id as sale_id,
           c.name as customer,
           s.sale_moment as sale_moment,
           s.discount_percentage as additional_discount_percentage,
           calculate_sale_cost(s.id) as sale_final_cost,
           sp.id as sale_position_id,
           cc.name as configuration,
           get_configurations_component_ids(cc.id) as configurations_component_ids,
           calculate_configuration_cost(cc.id) as sale_position_final_cost,
           sp.cost as sale_position_start_cost,
           sp.discount_percentage as sale_position_discount_percentage,
           sp.warranty_period as warranty_period
    from customers c
        join sales s on c.id = s.customer_id
        join sale_positions sp on s.id = sp.sale_id
        join computer_configurations cc on sp.computer_configuration_id = cc.id;