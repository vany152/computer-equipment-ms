create or replace function create_customer(_name varchar(75), _contacts jsonb, _registration_date date)
    returns customers
as $$
    declare new_customer customers;
begin
    insert into customers (name, contacts, registration_date) 
    values (_name, _contacts, _registration_date)
    returning * into new_customer;
        
    return new_customer;
end;
$$ language plpgsql;
