create or replace function get_sale_position_ids_for_sale(_sale_id integer) 
    returns integer[]
as $$
    declare
        current_sale_position_id integer;
        sale_position_ids integer[];
begin
    for current_sale_position_id in
        select sale_id
        from sale_positions
        where sale_id = _sale_id
    loop
        sale_position_ids = array_append(sale_position_ids, current_sale_position_id);
    end loop;

    return sale_position_ids;
end;
$$ language plpgsql;
