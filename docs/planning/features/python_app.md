# Python language analysis 
Want to build a language analysis service that can be called by the main server (both in synchronous requests and in the background as part of event handling). 

This will be a rest api that the server can call. We may move to gRPC later if we need to, but that's too much overhead for an MVP. 

## Slice 1
The first deliverable is a service that integrates with the server and does some meaningful work. The most basic thing we could do is return the lemmas from a sentence. Even that is a lot of work. Whether something counts as a sentence is a large question, and so is what validation we need to do beforehand to ensure that. For now, we're going to consider quite a happy path where we test with actual sentences in the target language, and later we'll start trying to break it (sentences in different languages, random text, empty strings, strings that are too long, spelling mistakes, words that mean different things in different contexts, etc.).

## Outcome
Given a user has created a submission, when the submission is analysed, the SubmissionAnalysis entity stores the lemmas from the submission

### Step 1: DONE
Outcome: locally running python rest api with hardcoded language model that tokenizes a hardcoded string in one language

Requirements: 
- spacy installed and language model downloaded 
- fastapi endpoint that returns hardcoded string's tokens by running empty pipeline 
- app lifecycle to download language model once 

### Step 2: DONE
Outcome: tokenize a string in a request body

### Step 3: DONE
Outcome: replace tokenization with lemmatization

### Step 4: 
Outcome: synchronous .net method can call python app api

Add synchronous endpoint call to integrate .net app with python app. This can just be a healthcheck endpoint for now. I want to run both the .net server and the python app locally, but soon I'll want to containerise the python app (and potentially the .net server).