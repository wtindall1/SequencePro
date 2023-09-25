 # sequence_pro
Protein Sequence Analyser using C# 11 and .Net Core 7.

------------------------
LOCAL SETUP (Visual Studio)

Database Containers (Docker, Postgres):
- Navigate to Sequence_Pro.Application in terminal
- Run 'docker compose up'

Initalise Database (EF migrations):
- Navigate to to Sequence_Pro.Application in VS Package Manager Console or dotnet CLI and run :
    - VS Package Manager Console: 'Update-Database'
    - dotnet CLI: 'dotnet ef database update'

Generate Token:
1. Run Token.API (swagger should open automatically)
2. Call POST /token endpoint. In "customClaims", add: "trusted_user": true OR "admin_user": true (or no custom claims)
3. Copy the Jwt

Run Application:
1. Run Sequence_Pro.API (swagger should open automatically)
2. Click 'Authorize' in top right corner. Enter "Bearer <addJwtHere>".
3. Authorization: (Admin users: all endpoints, Trusted users: Post and Get endpoints, Any users: Get endpoints)
4. Call POST endpoint with a UniprotId, eg: "P12345". Details of endpoints below

Run unit & integration tests (Visual Studio):
- Run tests in Sequence_Pro.Tests

------------------------

Endpoints:

POST /api/sequenceAnalysis :
- Calls uniprot API to retrieve sequence data
- Carries out sequence analysis
- Saves analysis record in db

GET /api/sequenceAnalysis:
- retrieves all records from db

GET /api/sequenceAnalysis/{IdOrUniprotId}:
- retrieves record with unique Id OR last record with that uniprotId

DELETE /api/sequenceAnalysis/{Id}
- deletes record with unique Id







