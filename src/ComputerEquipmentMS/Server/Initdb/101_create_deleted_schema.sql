begin;

-- create "deleted" schema
/*
 * This schema intended for storing deleted items
 */
create schema deleted;
set search_path to deleted;



-- create tables
create table customers as table public.customers with no data;
create table sales as table public.sales with no data;
create table sale_positions as table public.sale_positions with no data;
create table computer_configurations as table public.computer_configurations with no data;
create table components_computer_configurations as table public.components_computer_configurations with no data;
create table components as table public.components with no data;
create table component_categories as table public.component_categories with no data;
create table component_manufacturers as table public.component_manufacturers with no data;



-- create primary keys
alter table customers
    add constraint deleted_customers_pk
        primary key (id);

alter table component_categories
    add constraint deleted_component_categories_pk
        primary key (id);

alter table component_manufacturers
    add constraint deleted_component_manufacturers_pk
        primary key (id);

alter table sales
    add constraint deleted_sales_pk
        primary key (id);

alter table sale_positions
    add constraint deleted_sale_positions_pk
        primary key (id);

alter table computer_configurations
    add constraint deleted_computer_configurations_pk
        primary key (id);

alter table components_computer_configurations
    add constraint deleted_components_computer_configurations_pk
        primary key (component_id, computer_configuration_id);

alter table components
    add constraint deleted_components_pk
        primary key (id);



set search_path to public;

end;
