# Connect to db
```
psql -h localhost -p 8080 -U postgres -d kusuri
```

# Backend is running on
port 5145
```
dotnet watch
```

# Frontend is running on
port 5173
```
npm run dev
```

# Create .sql for docker
```
dotnet ef migrations add [migraiton_name]
dotnet ef migrations script
```