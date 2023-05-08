# passwordless pgadmin4
FROM dpage/pgadmin4:latest

ENV PGADMIN_DEFAULT_EMAIL="pgadmin@pgadmin.org"
ENV PGADMIN_DEFAULT_PASSWORD="admin"
ENV PGADMIN_CONFIG_SERVER_MODE="False"
ENV PGADMIN_CONFIG_MASTER_PASSWORD_REQUIRED="False"

ENV POSTGRES_HOST="postgres"
ENV POSTGRES_PORT="5432"
ENV POSTGRES_DB="*"

USER root
COPY primary_entrypoint.sh /primary_entrypoint.sh
RUN chmod +x /primary_entrypoint.sh && \
    touch /pgadmin4/servers.json && \
    chown pgadmin:root /pgadmin4/servers.json
USER pgadmin

ENTRYPOINT /primary_entrypoint.sh