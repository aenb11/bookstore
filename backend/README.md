
# Database setup

The backend uses MongoDB, which is configured in [`compose.yaml`](./compose.yaml). Docker Compose also starts Mongo Express, a browser-based interface for viewing and managing the database.

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
