 # sequence_pro
Protein Sequence Analyser using C# 11 and .Net Core 7.

Database setup (Docker / postgres):
- navigate to Sequence_Pro.Application in terminal
- run 'docker compose up'


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







