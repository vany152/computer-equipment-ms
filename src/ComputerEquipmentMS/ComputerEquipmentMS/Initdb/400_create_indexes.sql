---- create indexes
create extension pg_trgm;

-- customers
create index customers_name_idx on customers using gin (name gin_trgm_ops);
create index customers_contacts_idx on customers using gin (contacts jsonb_path_ops);

-- sales
create index sales_sale_moment_idx on sales using btree (sale_moment);

-- sale_positions
create index sale_positions_cost_idx on sale_positions using btree (cost);

-- computer_configurations
create index computer_configurations_name_idx on computer_configurations using gin (name gin_trgm_ops);

-- components
create index components_name_idx on components using gin (name gin_trgm_ops);
create index components_specifications_idx on components using gin (specifications jsonb_path_ops);
