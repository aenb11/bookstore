
# Database setup

The backend uses MongoDB, which is configured in [`compose.yaml`](./compose.yaml). On the first start with a fresh `mongo_data` volume, MongoDB runs [`01-bookstore.js`](./mongo-init/01-bookstore.js) to create the `bookstore` database, collections, validators, and indexes from the bookstore DBML schema. Docker Compose also starts Mongo Express, a browser-based interface for viewing and managing the database.

## Start the database

Install and start Docker Desktop, then run these commands from the `backend` directory:

```powershell
docker compose up -d
docker compose ps
```

MongoDB is available at `localhost:27017`. To connect from an application running on your computer, use:

```text
mongodb://root:example@localhost:27017/?authSource=admin
```

For another service running in the same Compose project, use `mongo` as the hostname instead of `localhost`.

## Browse the database

Open [http://localhost:8081](http://localhost:8081) and sign in to Mongo Express:

- Username: `admin`
- Password: `admin123`

These credentials are for local development only. Change them before exposing the services to other users or networks.

## Stop the database

Stop the containers while keeping the database data:

```powershell
docker compose down
```

MongoDB data is stored in the `mongo_data` Docker volume and remains available when the containers are stopped. `docker compose down -v` also deletes that volume and its data.

Initialization scripts only run when MongoDB initializes an empty data directory. If `mongo_data` already exists, Compose will not automatically apply schema changes; back up the data and run a deliberate migration instead. Do not use `docker compose down -v` to trigger initialization unless you intend to permanently delete the existing database data.
