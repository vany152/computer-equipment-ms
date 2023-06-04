/*
 * returns -1 if sale with specified id does not exist
 */
create or replace function calculate_starting_sale_cost(_sale_id integer)
    returns numeric(15, 2)
as $$
declare result numeric(15, 2);
begin
    result = (
        select sum(cost - cost * discount_percentage / 100)
        from sale_positions sp
        where sp.sale_id = _sale_id
    );

    if (result is null) then
        result = -1;
    end if;

    return result;
end;
$$ language plpgsql;
