# Connect to db
```
psql -h localhost -p 8080 -U postgres -d kusuri
```

# Backend is running on
port 5145
```
gcloud init
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

# Get gemini text embeddings with curl
```
export GOOGLE_API_KEY=[key]
curl "https://generativelanguage.googleapis.com/v1beta/models/text-embedding-004:embedContent?key=$GOOGLE_API_KEY" \
-H 'Content-Type: application/json' \
-d '{"model": "models/text-embedding-004",
     "content": {
     "parts":[{
     "text": ""}]}, }' 2> /dev/null | head 
```

# Debug to see if there's error creating index in Redis
```
FT.INFO <indexname>
```