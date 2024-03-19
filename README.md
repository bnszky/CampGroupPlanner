# About user interface
**Camp Group Planner is a web application that allows you to store information about events and the latest news from campus or youth camp.**
There are 2 types of tiles visible for users: News, Events.
- News are pieces of information about daily activities and interesting things. This tile includes basic fields like `title, description, and disclosure date`.
- Events are similar to news but additionally provide you a possibility to sign up for specific event. Events have `title, description, date (begin, end), members' capacity, localization`.

> [!WARNING]
> It'll be possible to add comments and reactions to the news by the user.
> There are plans to add teams, and team activities in the future (like volleyball, tug of war, paintball, and other unusual sports)

# Admin panel and positioning
Admin is a specific account to handle news, and events (basic CRUD operations), and: 
- import news from other web sources.
- access to view a list of people enrolled for the event
- can modify the order of news and events in user view

# Models
1. User (`id, name, surname, nick, email, password`)
2. News (`id, title, description, date`)
3. Event (`id, title, description, date, beginDate, endDate, participants, localization`)

# Changelog
- (22.02.24) Basic project setup: added simple views, user model and form, connected with DB using EF
