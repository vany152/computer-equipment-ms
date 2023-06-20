set -e

# Making /pgpass file 
touch /var/lib/pgadmin/pgpass
echo "$POSTGRES_HOST:$POSTGRES_PORT:$POSTGRES_DB:$POSTGRES_USER:$POSTGRES_PASSWORD" | tee "/var/lib/pgadmin/pgpass" >/dev/null

# Making servers.json file
touch servers.json
tee /pgadmin4/servers.json >/dev/null <<EOF
{
  "Servers": {
    "1": {
      "Name": "$POSTGRES_NAME",
      "Group": "Servers",
      "Host": "$POSTGRES_HOST",
      "Port": $POSTGRES_PORT,
      "MaintenanceDB": "$POSTGRES_DB",
      "Username": "$POSTGRES_USER",
      "SSLMode": "prefer",
      "PassFile": "/var/lib/pgadmin/pgpass"
    }
  }
}
EOF

# set -rw-.. permission to /pgpass
chmod 600 /var/lib/pgadmin/pgpass
# set owner to /pgpass 
chown pgadmin:root /var/lib/pgadmin/pgpass

exec /entrypoint.sh "$@"