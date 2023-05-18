create or replace function does_json_contain_all_values(given_json jsonb, "values" jsonb[]) 
    returns boolean
as $$
    declare
        value jsonb;
        contains boolean;
begin
    foreach value in array "values" loop
        contains = given_json @> value;
    end loop;
        
    return contains;
end;
$$ language plpgsql;
