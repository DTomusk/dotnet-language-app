# How to add a new language 
1. Add language code to LanguageCode value object in .net server
2. Add language code and spacy model name pair to lifecycle.py map 
3. Run `python -m spacy download de_core_news_sm` in `./python` with the venv activated
4. Run `pip freeze > requirements.txt` in `./python`

TODO: update once we have validation strategies and python language identification