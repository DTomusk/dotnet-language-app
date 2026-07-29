## Planning
- [ ] Deployment
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: 
    - DoD: 

- [ ] Validation pipeline
    - Priority: high
    - Area: language analysis
    - Type: feature
    - Why: NLP pipelines will analyse anything you put into them. We need a separate process to determine whether the language that a user inputting is even recognisable as the language that they're trying to practise. 
    - DoD: 
        - [ ] in memory language agnostic processing 
        - [ ] in memory language specific validation strategies (heuristics)
        - [ ] python language detection pipeline 

- [ ] Frontend
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: 
    - DoD: 



## Item template 

- [ ] Item name
    - Priority: 
    - Area: 
    - Type: (bug, tech debt, feature)
    - Why: 
    - DoD: 

## Done
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