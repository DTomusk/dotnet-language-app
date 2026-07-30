## Planning
- [ ] Submission page
    - Priority: high 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: 
    - DoD: 
        - [ ] successful submission redirects to submission page 
        - [ ] shows analysis progress (pending, successful, error etc.)
        - [ ] user can see list of submissions and click into analyses and text

- [ ] Make worker a separate deployable
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: want to decouple api from backgrounding, so can scale independently, have separate logs etc. 
    - DoD: 
        - [ ] worker deployed separately via fly 
        - [ ] background service removed from api server 

- [ ] Flashcards
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: users might want to practise specific words and phrases, they should be able to note down these words and then practise them with space repetition
    - DoD: 

- [ ] Internationalise backend
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: currently, error messages etc. are all in English. Goal is to have the site fully in the target language 
    - DoD: 

## Item template 

- [ ] Item name
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: 
    - DoD: 

## Done
- [x] Validation pipeline
    - Priority: high
    - Area: language analysis
    - Type: feature
    - Why: NLP pipelines will analyse anything you put into them. We need a separate process to determine whether the language that a user inputting is even recognisable as the language that they're trying to practise. 
    - DoD: 
        - [x] in memory language agnostic processing 
        - [x] in memory language specific validation strategies (heuristics)
        - [x] python language detection pipeline 

- [x] Deployment
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: 
    - DoD: 
        - [x] .net server deployed 
        - [x] python deployed 
        - [x] basic ci/cd implemented

- [x] Frontend
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: 
    - DoD: 
        - [x] site built 

- [x] Multi-language support
    - Priority: high 
    - Area: throughout
    - Type: tech debt
    - Why: the step of going from 1 langauge to 2 is much bigger than going from 2 to more, so I want to get this over with as soon as possible, otherwise I'm in danger of making poor architectural decisions that will make multi-language support more difficult in the future
    - DoD: 

- [x] Python language app slice 1
    - Priority: high 
    - Area: language analysis
    - Type: feature
    - Why: MVP use case
    - DoD: 