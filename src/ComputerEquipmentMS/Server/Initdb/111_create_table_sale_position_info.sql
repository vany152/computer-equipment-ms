create table sale_position_info (
    sale_position_id    integer,
    sale_moment         timestamptz,
    starting_cost       numeric,
    final_cost          numeric,
    discount_percentage smallint,
    warranty_period     interval
);
    