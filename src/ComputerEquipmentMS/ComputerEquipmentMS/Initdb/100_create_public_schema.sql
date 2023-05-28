begin;

-- create "public" schema
create schema if not exists public;
set search_path to public;



-- create auxiliary functions
create or replace function is_string_null_or_empty(str varchar)
    returns boolean
as
$$
begin
    return str is null or trim(str) = '';
end;
$$ language plpgsql;

create or replace function is_interval_negative("interval" interval)
    returns boolean
as
$$
begin
    return "interval" < '0'::interval;
end;
$$ language plpgsql;

create or replace function can_value_be_percent(value smallint)
    returns boolean
as
$$
begin
    return value between 0 and 100;
end;
$$ language plpgsql;



-- create tables
create table customers (
    id                serial,

    name              varchar(75) not null,
    contacts          jsonb,
    registration_date date        not null,

    constraint customers_pk
        primary key (id),

    constraint non_empty_name_check
        check ( not is_string_null_or_empty(name) )
);

create table sales (
    id                  serial,
    customer_id         integer     not null,

    sale_moment         timestamptz not null default current_timestamp,
    discount_percentage smallint    not null default 0,

    constraint sales_pk
        primary key (id),
        
    constraint value_must_be_percent
        check ( can_value_be_percent(discount_percentage) )
);

create table sale_positions (
    id                        serial,
    sale_id                   integer        not null,
    computer_configuration_id integer        not null,

    cost                      numeric(15, 2) not null,
    discount_percentage       smallint       not null default 0,
    warranty_period           interval       not null default '0'::interval,

    constraint sale_positions_pk
        primary key (id),

    constraint non_negative_warranty_period
        check ( not is_interval_negative(warranty_period) ),
    
    constraint value_must_be_percent
        check ( can_value_be_percent(discount_percentage) )
);

create table computer_configurations (
    id              serial,

    name            varchar(250),
    warranty_period interval       not null default '0'::interval,
    margin          numeric(15, 2) not null,

    constraint computer_configurations_pk
        primary key (id),

    constraint non_empty_name_check
        check ( not is_string_null_or_empty(name) ),

    constraint non_negative_warranty_period
        check ( not is_interval_negative(warranty_period) )
);

create table components_computer_configurations (
    component_id              integer,
    computer_configuration_id integer,

    constraint components_computer_configurations_pk
        primary key (component_id, computer_configuration_id)
);

create table components (
    id                        serial,

    component_category_id     integer        not null,
    component_manufacturer_id integer        not null,

    name                      varchar(250)   not null,
    specifications            jsonb          not null,
    cost                      numeric(15, 2) not null,
    warranty_period           interval       not null default '0'::interval,

    constraint components_pk
        primary key (id),

    constraint non_empty_name_check
        check ( not is_string_null_or_empty(name) ),
        
    constraint non_null_specifications
        check ( specifications <> 'null'::jsonb ),

    constraint non_negative_warranty_period
        check ( not is_interval_negative(warranty_period) )
);

create table component_categories (
    id   serial,
    name varchar(50) not null not null,

    constraint component_categories_pk
        primary key (id),

    constraint non_empty_name_check
        check ( not is_string_null_or_empty(name) )
);

create table component_manufacturers (
    id   serial,
    name varchar(50) not null,

    constraint component_manufacturers_pk
        primary key (id),

    constraint non_empty_name_check
        check ( not is_string_null_or_empty(name) )
);

create table change_logs (
    id        bigserial,
    moment    timestamptz not null default current_timestamp,

    username  text        not null default current_user,
    action    text        not null,
    "table"   text        not null,
    old_value text,
    new_value text,

    constraint change_logs_pk
        primary key (id)
);



-- create foreign keys
alter table sales
    add constraint sales_customers_fk
        foreign key (customer_id)
            references customers (id)
            on update cascade on delete cascade;

alter table sale_positions
    add constraint sale_positions_sales_fk
        foreign key (sale_id)
            references sales (id)
            on update cascade on delete cascade,
    add constraint sale_positions_computer_configurations_fk
        foreign key (computer_configuration_id)
            references computer_configurations (id)
            on update cascade on delete cascade;

alter table components_computer_configurations
    add constraint components_computer_configurations_components_fk
        foreign key (component_id)
            references components (id)
            on update cascade on delete cascade,
    add constraint components_computer_configurations_computer_configurations_fk
        foreign key (computer_configuration_id)
            references computer_configurations (id)
            on update cascade on delete cascade;

alter table components
    add constraint components_component_categories_fk
        foreign key (component_category_id)
            references component_categories (id)
            on update cascade on delete cascade,
    add constraint components_component_manufacturers_fk
        foreign key (component_manufacturer_id)
            references component_manufacturers (id)
            on update cascade on delete cascade;

end;
