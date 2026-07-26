# dotnet-language-app
App for practising languages by producing written content and having it analysed by an NLP pipeline. 

## Tech stack
- .Net 10 server handles API and background processing 
- Python language analysis service runs Spacy NLP pipeline 
- Vite react frontend with MUI components, RHF for forms, and Tanstack query 
- Github actions for CI/CD 
- Fly for deploying Docker containers