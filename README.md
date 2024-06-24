This is final project for Volvo .NET Academy

# Database and models diagram

There are main 4 models: Attraction, Article, Region, Review
Admin can perform all CRUD operations with Attraction, Article and Region and delete Reviews

User (no matter is authenticated or not) can see only Articles (higher or equal with MinPositivityRate set by Admin and stored in Configuration table in the db), Attractions, Regions or Reviews
User can also add own review assigned to specific region 

![image](https://github.com/bnszky/CampGroupPlanner/assets/76440830/9d07cbb4-d4b2-4b21-aa07-dec8038a8648)


