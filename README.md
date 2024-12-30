# SkillMiner

This project is used as a playground for experimenting with patterns from DDD and an implementation of Clean Architecture.

## What Does it Do?

The system is designed to inform professionals which skills are in-demand currently for a given profession. It does this by web-scraping job listings and then using an LLM to extract keywords from those job-listings. It then simply sees which keywords appeared most frequently on job listings posted within the last few months. The keyword popularity logic could be a little more sophisticated, but it's good enough for now.

## DDD Features Currently Implemented

- Repositories
- Aggregate Root Entities (although, currently only using an `Entity` class to represent an aggregate root as well as entities on the aggregate root. I'll consider maybe making a separate class for it.)
- Strongly Typed Ids (GUIDs on a non-PK column, but with an index for past access. Then with a shadow integer primary key that isn't on the domain model.)
- Value Objects
- Domain Events. I don't really have much of a use-case for them at the moment, but the system is setup to support them. Might be useful when experimenting.

## DDD Features Pending

- Entity validation - Leaning towards a not-always-valid model, as described here https://medium.com/@iamprovidence/validation-in-ddd-3c049b9087b. Not implemented yet, but will give it a go eventually. Entities have validate methods on them that could be invoked by an EF Core interceptor, so that's an idea.

- Domain Services - Don't really have much of a need for theses at the minute.

- Factories - The only aggregate at the moment is pretty simple, so again there's not much use for this currently.
