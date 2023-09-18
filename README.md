 # sequence_pro
Protein Sequence Analyser using C# 11 and .Net Core 7.

Database setup (Docker / postgres):
- navigate to Sequence_Pro.Application in terminal
- run 'docker compose up'

Run unit & integration tests (Visual Studio):
- Run tests in Sequence_Pro.Tests

------------------------
STEPS TO RUN API (Visual Studio)

Generate token:
1. run Token.API (swagger should open automatically)
2. Call POST /token endpoint. In "customClaims", add: "trusted_user": true OR "admin_user": true (or no custom claims)
3. Copy the Jwt

Run Application:
1. run Sequence_Pro.API (swagger should open automatically)
2. Click 'Authorize' in top right corner. Enter "Bearer <<addJwtHere>>".
3. Authorization: (Admin users: all endpoints, Trusted users: Post and Get endpoints, Any users: Get endpoints)
4. Call POST endpoint with a UniprotId, eg: "P12345". Details of endpoints below

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







