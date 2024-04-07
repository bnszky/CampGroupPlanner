# About
**This application allows you to fetch travel articles from different sources.**
- All articles should be linked to the location described in the article.
- Articles could have images that could be liked or not by users to monitor how images affect users' attention
- The user will be able to prepare a review for every article in the database

> [!WARNING]
> The user could choose an article by image. There will be a displayed gallery of images without any text that furthers to articles (interesting experiment)
> The user could also choose an article on a map (pin on the map or near you (radius 100km)) thanks to the location.

# Admin
Admin is a specific account to handle news, (basic CRUD operations), and: 
- import and aggregate articles from other web resources.
- verify the location and images of articles
- change the position of articles by altering the "position ratio" yet not defined

# Entity Diagram (In progress)
![image](https://github.com/bnszky/CampGroupPlanner/assets/76440830/17c4d562-a9ec-45f3-8cac-c6c59145e985)

# Azure AI-Language Tool
To use entities detection system in text you must set your key and endpoint:
```
dotnet user-secrets init
dotnet user-secrets set "AzureKeyCredential" "YOUR_KEY"
dotnet user-secrets set "AzureLanguageEndpoint" "YOUR_REGION"
```
> [!CAUTION]
> LocationDetectionService isn't fully implemented so now it isn't working!

