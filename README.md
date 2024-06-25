__This is the final project for Volvo .NET Academy__

# Main idea
This application was created to extract travel news content from different sources to improve user experience by searching for unusual attractions and articles related to regions.
Users can choose a region and find some attractions and articles. Users can rate regions based on their own experience or opinions about country, culture, and other useful factors.

# Database and models diagram

## Models
There are main 4 models: Attraction, Article, Region, Review
Admin can perform all CRUD operations with Attraction, Article, and Region and delete Reviews

## Operations
1. Admin can perform all CRUD operations with Attraction, Article, and Region and delete Reviews
2. The User (no matter is authenticated or not) can see only Articles (higher or equal to MinPositivityRate set by Admin and stored in the Configuration table in the DB), Attractions, Regions, or Reviews
3. User can also add their review assigned to a specific region (do you recommend region or not based on experience in a surrounding area) 

## Database Diagram (Microsoft SQL Database)

![image](https://github.com/bnszky/CampGroupPlanner/assets/76440830/9d07cbb4-d4b2-4b21-aa07-dec8038a8648)

# Articles aggregation process
1. Finding articles
Articles are fetched with the use of NewsApi and/or from the list of RSS configured in appsettings.json (it is skipped for regionName).
2. Check if doesn't exist in the DB and take 60 (MAX_NUMBER) random articles
3. Rate articles with ChatGptApi (model=gpt-4o)
4. If something goes wrong with ChatGptApi, try to assign and rate articles by keywords matching algorithm
5. If no errors try to store selected articles in the DB

# Attraction fetching process
1. Get cities for region
2. Use cities that are set for all regions (with __geonames__ API)
Take some cities to get attractions (search for attractions in a specific radius of the city (__Foursquare Places Api__))
3. Store in the DB

# Authentication and authorization
Authentication is performed with ASP .Net Identity (AspNetUsers and Roles)
You can restore your password and you must confirm your email after registration to use your rights
Authorization was implemented with JWT Tokens.
If you log in you'll receive your token to perform all tasks.

# Frontend part 
Frontend part is fully written in JS with React using the component library [Material UI](https://mui.com/material-ui/getting-started/)
communicate through ASP.NET API
![image](https://github.com/bnszky/CampGroupPlanner/assets/76440830/322cc6a4-c80f-4f47-9e29-d9043fb7cd91)
![image](https://github.com/bnszky/CampGroupPlanner/assets/76440830/fc73446b-f818-49b7-8334-7d197e6535e9)

# Other Important Features related to architecture
1. Serilog to display important information in a console and in a Log folder
2. AutoMapper and DtoClasses for more flexible integration with the DB and nice-looking API models
3. Swagger UI
4. xUnit and Moq for testing services

# Settings
_appsettings.json_
```
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Error",
        "System": "Error"
      }
    },
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.log",
          "rollingInterval": "Day"
        }
      }
    ]
  },
  "ConnectionStrings": {
    "TripAppDb": "ConnectionString"
  },
  "Jwt": {
    "Key": "JwtKey",
    "Issuer": "TripPlanner.com"
  },
  "AdminSettings": {
    "Email": "admin@admin.com",
    "Password": "Admin123!"
  },
  "Foursquare": {
    "Key": "Foursquare Api Key for fetching attractions"
  },
  "RssFeeds": {
    "Links": [
      "https://1000daysoftravel.godaddysites.com/f.atom",
      "https://feeds.feedburner.com/breakingtravelnews",
      "https://feeds.bbci.co.uk/news/world/rss.xml",
      "https://feeds.bbci.co.uk/news/world/europe/rss.xml",
      "https://www.aljazeera.com/xml/rss/all.xml",
      "http://rss.cnn.com/rss/edition_world.rss"
      // Add other relevant RSS feed URLs here
    ]
  },
  "Ports": {
    "Client": "https://localhost:5173",
    "Server": "https://localhost:7279"
  },
  "NewsApi": {
    "Key": "Key for fetching news from api"
  },
  "OpenAI": {
    "Key": "key for ChatGptApi",
    "Model": "gpt-4o"
  },
  "ProjectPath": "PATH_TO\\repos\\TripPlanner\\TripPlanner.Server",
  "AllowedHosts": "*"
}
```






