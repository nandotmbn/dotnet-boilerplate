#!/bin/bash

# Function to create a database
create_database() {
  local dbname=$1
  echo "Creating database $dbname..."
  docker exec databases-psql-1 psql -U postgres -p $PORT -c "CREATE DATABASE $dbname"
}

# Function to grant privileges on a database
grant_privileges() {
  local dbname=$1
  echo "Granting privileges on database $dbname..."
  docker exec databases-psql-1 psql -U postgres -p $PORT -c "GRANT ALL PRIVILEGES ON DATABASE $dbname TO gasspolldatabasejoss;"
}

# Function to grant privileges on the public schema
grant_schema_privileges() {
  local dbname=$1
  echo "Granting schema privileges on database $dbname..."
  docker exec databases-psql-1 psql -U postgres -d $dbname -p $PORT -c "GRANT ALL ON SCHEMA public TO gasspolldatabasejoss;"
}

# Main function to set up a database
setup_database() {
  local dbname=$1
  create_database $dbname
  grant_privileges $dbname
  grant_schema_privileges $dbname
}

# Environment variable for port (set this according to your setup)
PORT=21601

# Set up the databases
setup_database database