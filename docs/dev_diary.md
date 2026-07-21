# 2026-07-21
## CD 
Now that I have deployables, I'd like to automatically deploy to them on merge to main. I would also like to run migrations on the hooked up database before deployment. For migrations, I need to figure out how to run them locally for a remote database and then make that into a repeatable pipeline action. I think I just need to run the migration script I already have, but change the connection string I'm using. 

I'm using supabase for postgres because they have a generous free tier. I could set up postgres in fly and that might be easier, but honestly, it doesn't make much of a difference. 

I've now got my .net server running on fly and I've managed to call the health check endpoints using postman. As expected, the live one works and the ready one doesn't because .net isn't talking to python yet, so that's the next thing I want to fix. 

Ok, I've now got the ready endpoint working in postman, whoopie! It would be good to run the healthcheck endpoints in my CD pipeline once we've deployed to check it's all working, but for now I'm quite happy. The next thing I want to do is to make the background job frequency configurable because it's really annoying how much it fills up the logs and there's no way for me to control it. I can make it less frequent now and then later up the frequency once there's a reason to. 

## More deployment 
Not feeling the best today, so probably won't do too much. I've been thinking a bit more about where to host this project, and it honestly seems like k8's or even k3's is going to be too expensive for my needs. I've used fly in the past, which has been really cheap for previous projects, so I'm tempted to just go in that direction. Other options including renting a VPS and hosting there, or self-hosting. I have an old PC that I'm not using any more, so that could be a good option, but I'm not sure if I'd want to have it on all the time, and self-hosting is a lot more effort to set up than VPS, and that's more effort than something like fly. 

My goal right now should just be to get it hosted somewhere, and if fly is the cheapest and simplest, I should go for that. I can always migrate to a VPS if I want more control or take some time to figure out self-hosting on my PC. 

For now, I'll use fly. I'll have two fly apps, one for .net and one for python. Python will be private so it can be accessed by .net but not publicly. .net will be the entrypoint for everything. Keeping them separate will allow for independent scaling. At some point, I may separate the background processing into a worker deployment, and that should be super easy as well because they are essentially independent already, it's just how they're running is coupled. 

For creating a new fly app: `fly launch --name language-api --no-deploy`, then edit .toml file manually to add in `Dockerfile`

For deploying a specific fly app: `fly deploy -c ../fly/api.toml` run this from the specific project directory (e.g. `./server` in this case) to prevent needlessly sending files to the builder.

# 2026-07-19
## Deployment
Now that I have some functionality, no matter how rudimentary, the next thing I need to think about is deployment, deployment, deployment. I've learned the hard way how annoying it is to work on deploying a project that's already reached a certain level of maturity. If I'm not buildig with the aim of deploying, then what's the point? So, I want to get my .net and python servers deployed to a test environment ASAP. 

The first step towards that goal is containerisation. I want to be able to run both services in Docker locally, then I'd like to learn some basic kubernetes to manage my deployments (probably k3's, I don't want to pay much and my requirements are pretty simple). I already run my database through docker, so I don't need to set up a new compose file. 

# 2026-07-17
## Lemma stats
I was in a state of complete focus this evening. I implemented a couple of new events and a new endpoint and got them working. I think it's about time I merged the language analysis branch, not because it's done by any means, but I think I want my branches to be more focused and I've still delivered a meaningful piece of work. Later, I would like to work on deployments and unit and integration tests. I would also like to add rate limiting and refresh tokens, as well as build out a super basic react ui. For now, I'm very happy with the work that I've done. 

The other thing was I wanted to come back to the validation pipeline. I think this should be built with the strategy pattern. Each language should have its own strategy e.g. in italian you don't expect to have chinese characters, but in chinese you do. Also, some languages use spaces and others don't. I want to define simple heuristics for guessing whether a piece of text is definitely not a given language. 

# 2026-07-16
## Spacy
I'm very excited to be running NLP pipelines now. There's so much information it can extract from text, so there are lots of directions in which the langauge analysis can go. For now, I want to focus on vocabulary, which will be based on lemmas (ran, runs, running aren't separate vocab). Later I can add more stuff, but I think this is the MVP. 

I'm going to add a validation pipeline later. That's going to run synchronously when a user creates a submission. This is to ensure that there's less pressure to validate data entering the analyzer, and that users get instant feedback on their submissions. For now, I'm going to wait. Later I will build a separate submission validation service that will have a bunch of guards for stuff like proportion of spaces, certain punctuation, certain characters, and once the submission has passed all of that, it will be handed off to an external validation service that runs a language recogniser just to check that it's using the expected language. 

For the analysis service, we want valid language codes and schema based validation. Right now, the valid language codes is limited to "it" and is hardcoded. Later, we may want a better way of managing language codes and language models, but honestly hardcoded could be fine for a while. The most important thing is to get out of functions as fast as possible to save unnecessary calls and work, so use guards, normalization, validation etc. 

## Shared types 
For a moment, I was considering adding some tooling to export the python app's openapi definitions into the codebase and generate c# types from that, but I think for now, to keep it simple, I'm just going to handwrite the types and make sure they match up manually. If this becomes burdensome, I can introduce tooling, but I don't think it's much overhead to handwrite stuff and I don't think there will be enough types for a while to warrant adding some kind of generation tooling. 

## Health check
Before I can do any real, meaningful integration with my external python app, I want to be confident that I can query the python app. To this end, I want to implement health checks. I've created a new health check controller and a health check interface. Each external dependency will implement this interface, and then the health check controller will return whether everything is ready or not, as well as the individual status of each. After this step, I will be confident that I can use my .net server to call my python app, so then that's when the fun begins. 

# 2026-07-15
## Python 
Strictly speaking, I shouldn't need the python language processing service implemented in order to verify the event handler I have for language analysis, but at the same time, it would be helpful for me to play around with actual language analysis so I can better design the data structures that my analysis will be dealing with. 

A language analysis is an entity and an aggregate root. A language analysis is done for a submission. It will contain lemmas (which are value objects), as well as other data later on (grammatical mistakes, that kind of stuff). An analysis completing will trigger another event to update aggregated data on a user's vocabulary and other stats that are extracted from the full set of a user's submissions and not just one analysis. 

I want to build a Python FastAPI app that runs in Docker. I'm planning on using spacy for NLP, but I could use stanza as well. I don't think it matters too much at this point, and I've heard spacy is faster. We can always reevaluate later, right now we're firmly in PoC territory. Basically, I just want to play around with NLP and see what data I can extract from a given text, and what of that I would like to store. It should be minimal to start with, just something that I can get my teeth into and extract some value from. 

I will start by querying the FastAPI app manually locally. Later I will implement an ILanguageAnalysisService in my .Net server that wraps an http client that connects to the FastAPI app. By that time, I would like to have determined exactly what data I'm capturing. Once I have the python app set up, I can also run integration tests against it. My thinking is that the python app doesn't need to deal with persistence at all. It will just take data in and spit data out. 

# 2026-07-14
## Idempotency and atomicity 
How should event handlers deal with failure? That's a good question. 

Let's take the example I'm working on right now. We have a text analyser. This creates an analysis entity. The idempotency service should ensure that an event is handled by a handler just once. Before invoking the handler, we check whether that handler has handled that specific event and then decide whether to do it. Therefore, an event handler should throw if it has failed and we want the dispatcher to retry. 

What is each service's responsibility in this flow? 
- Event publisher: ensures atomicity between the change that triggers the event and the publishing of the event. E.g. user gets created and user created event raised, if user gets created, then the event was raised. It's either both or neither. Creates event. 
- Outbox processor: polls events to send them to dispatcher. Marks events as delivered once it knows that the event has been delivered at least once. This means that an event could reach the dispatcher multiple times. 
- Dispatcher: invokes the handler on each registered handler for an event type. Checks idempotency service to see whether that handler has handled that specific event already. If handler throws an exception, we should mark it as a failure and retry (potentially with exponential back-off). If handler doesn't throw, then we assume the event has been handled correctly, so we mark the event as handled in the idempotency service. 
- Event handler: handles a specific reaction to an event. If this throws, it's a failure, otherwise it's a success (from the dispatcher's point of view). For now, we rely on dispatcher retries to deal with transient failures in event handlers. Later, we can think about whether we need clean-up jobs. 

# 2026-07-13 Language analysis
## Branching 
I feel like the solution has reached a level of maturity that I can start branching now and working on feature branches. Earlier, I was just pushing everything straight to main, and, while I could keep doing that, branching helps to keep my changes focused and self-contained. If something goes irreparably wrong, I can just get rid of the branch, and it should also help focus my commit history in main (as long as I limit merge types). 

## Language processing 
I want to build a service for analysing a user's langauge usage in the background. It's going to be built in python because python has a stronger NLP ecosystem than .Net, but everything else will be in .Net. The idea is to create a dedicated python deployable that does the NLP heavy lifting, but all data persistence etc. will be done by the .Net server. 

For now, my base use case is storing a user's vocabulary. This in itself is complicated and I have to make decisions about storing lemmas, tokens, stems etc. but I think it's simple enough to develop in one slice. I will start with the happy path and then work on handling misspellings and other invalid stuff. Thankfully, we already have an event driven architecture, so backgrounding this is easy. 

One of the questions I had was whether I should analyse submissions or in the database, or send the content of the submission to the python service. My first thought is that the python service shouldn't have access to the server database. It's purely for processing and not persistence. It shouldn't persist anything itself, rather it should take in data and send data out. The other question in this regard is what should the event store that gets raised when a submission gets made. Should it store the id of the submission, or should it store the content of the submission? At the moment it doesn't matter too much because submissions are immutable, but we could run into data inconsistency later down the line if the submission is mutable. At this point, I don't expect to make perfect decisions because I can't predict everything that might happen, so I just have to try to make good decisions. 

My other thought is that submissions should be immutable to provide a record of improvement. You can go through all your past submissions to see what they used to be like, and that could keep track how well you're doing. Editing and versioning just makes things more complicated, and that's complexity that I don't want to introduce just yet (only when it needs to be). 

Current thoughts: event stores submission id. Event table doesn't store submission content for a couple of reasons: to prevent data leaks and to decrease amount of data stored. We can always change the payload of the event later if we add a new version processor, but I think this should help. The language analysis stuff is also in the same language practice bounded context, so it should be fine for the processor to access the language practice database (it's all the same database at the moment, but later we might want microservices and I think it helps to try to keep bounded context quite cleanly separated). On the data side, I feel like we'll want to start encrypting submissions to protect data more later. 

## Plan
There are a couple of moving pieces here that we can tackle separately. First off, there's a language submission event to raise when a language submission gets created. We'll need an event handler to handle that event. The event handler will depend on a language processor interface, the implementation of which will be a python service that could be built with fastapi, but we can use a dummy service for now. We'll want to start thinking about the shape of the data that the service returns. The python service should return structured data that the processing service can save in the database somewhere. 

I'm going to start with the event and then the event consumer. The event only needs to store the submission id for now, we don't know who a submission was made by (plus the submission will have the user if we need it anyway). We want to analyse the submission in a vacuum, later we can add more context. I just want to keep it as simple as possible. 

On second thought, if the event handler is just going to query the text immediately anyway, then we could just send the content, plus it may be useful to have a snapshot of the data at the time. I think it doesn't make too much of a difference, but the fewer dependencies the analyser has the better. 

## Language analysis data structure 
We can get a lot of data out of a language submission, and I don't expect we'll capture everything we will get in this first slice. 

Words (tokens) have lemmas. What do we want to happen when we analyse a sentence? I'm going to start by assuming completely "correct" sentences in the sense of the words involved. 