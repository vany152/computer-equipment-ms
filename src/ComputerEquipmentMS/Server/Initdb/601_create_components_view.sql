create or replace view components_view as
    select c.id as id,
           cc.name as category,
           cm.name as manufacturer,
           c.name as name,
           c.specifications as specifications,
           c.cost as cost,
           c.warranty_period as warranty_period
    from components c
        join component_categories cc on c.component_category_id = cc.id
        join component_manufacturers cm on c.component_manufacturer_id = cm.id;