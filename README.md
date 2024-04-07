![image](https://github.com/bnszky/CampGroupPlanner/assets/76440830/7e32ef04-3624-4b8f-bdaa-273fd5304973)

![image](https://github.com/bnszky/CampGroupPlanner/assets/76440830/730200fd-9356-4d86-bd64-e46874b3b36c)


# About
**This application allows you to fetch travel articles from different sources.**
- All articles should be linked to the location described in the article.
- Articles could be filtered by categories (not prepared yet)
- Articles could be searched by interactive map (news from specific locations)
- The user will be able to prepare a review for every article in the database

# Admin
Admin is a specific account to handle news, (basic CRUD operations), and: 
- import and aggregate articles from other web resources.
- verify the location and images of articles
- change the position of articles by altering the "position ratio" yet not defined

# Aggregation steps
1. Choose the RSS feed source link
2. Get basic information about articles (title, author, description, published date, image)
3. If an image is not specified, try to search image on a website for an article
4. Try to detect categories and locations described in articles (Named Entity Recognition)
5. Rate content by positive ratio (not specified yet)
6. Prepare a preview of posts for the admin
7. Admin can see posts and edit to improve perception and correctness
8. If everything is good try to add all articles to the database

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

