-- this table will be used as return type in some stored functions
create table sales_with_sale_position_ids (
    sale_position_ids integer[]
) inherits (sales);
