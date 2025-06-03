# Use Alpine Linux as base image
FROM alpine:latest

# Set environment variables
ENV POSTGRES_USER=gasspolldatabasejoss
ENV POSTGRES_PASSWORD=B1lyOounnTgfv7587

# Install PostgreSQL
RUN apk update && apk add postgresql16 postgresql16-contrib

# Initialize database cluster and start PostgreSQL
RUN mkdir /run/postgresql && chown postgres /run/postgresql
USER postgres
RUN initdb -D /var/lib/postgresql/data --locale=en_US.utf8 && \
    sed -i "s/#listen_addresses = 'localhost'/listen_addresses = '*'/" /var/lib/postgresql/data/postgresql.conf && \
    echo "host    all             all             0.0.0.0/0               md5" >> /var/lib/postgresql/data/pg_hba.conf && \
    pg_ctl -D /var/lib/postgresql/data -l /var/lib/postgresql/logfile start && \
    psql -U postgres -p 21601 -c "CREATE USER gasspolldatabasejoss WITH ENCRYPTED PASSWORD 'B1lyOounnTgfv7587';"

# Expose PostgreSQL port
EXPOSE 21601

# Start PostgreSQL
CMD ["postgres", "-D", "/var/lib/postgresql/data", "-c", "config_file=/var/lib/postgresql/data/postgresql.conf"]
