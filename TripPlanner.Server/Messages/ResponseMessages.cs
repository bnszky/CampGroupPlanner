namespace TripPlanner.Server.Messages;
public static class ResponseMessages
{
    // Authorization related messages
    public static readonly string UserAlreadyLoggedIn = "User is already logged in.";
    public static readonly string InvalidEmail = "The email address provided is invalid.";
    public static readonly string EmailNotConfirmed = "The email address has not been confirmed.";
    public static readonly string UserNotFound = "No user found with the provided email address.";
    public static readonly string InvalidLogin = "Invalid email or password.";
    public static readonly string EmailSendFailed = "Could not send the email.";
    public static readonly string EmailSendSuccessful = "Link has been sent to your email.";
    public static readonly string PasswordResetFailed = "Could not reset the password.";
    public static readonly string PasswordResetSuccessful = "Password has been reset successfully.";
    public static readonly string EmailConfirmed = "Email has been confirmed successfully.";
    public static readonly string EmailConfirmationFailed = "Email confirmation failed.";
    public static readonly string UnexpectedError = "Unexpected error occurred.";

    // Image or Region validation
    public static readonly string RegionValidationError = "Region validation error.";
    public static readonly string ImageUploadError = "Image upload error.";
    public static readonly string InvalidModelState = "Invalid model state.";

    // Article
    public static readonly string ArticleNotFound = "Couldn't find article with this ID.";
    public static readonly string CouldNotFetchArticles = "Couldn't fetch articles from database.";
    public static readonly string ArticleExists = "Article already exists.";
    public static readonly string ArticleCreated = "Article created successfully.";
    public static readonly string ArticleUpdated = "Article updated successfully.";
    public static readonly string ArticleDeleted = "Article deleted successfully. {0}";
    public static readonly string ArticleDeleteError = "Error deleting article.";
    public static readonly string ArticleUpdateError = "Error updating article.";
    public static readonly string ArticleCreateError = "Error creating article.";
    public static readonly string ArticlesFetched = "Articles fetched successfully.";

    // Attraction
    public static readonly string CouldNotFetchAttractions = "Couldn't fetch attractions from database.";
    public static readonly string AttractionNotFound = "Attraction not found.";
    public static readonly string AttractionCreated = "Attraction created successfully.";
    public static readonly string AttractionUpdated = "Attraction updated successfully.";
    public static readonly string AttractionDeleted = "Attraction deleted successfully.";
    public static readonly string AttractionDeleteError = "Error deleting attraction.";
    public static readonly string AttractionCreateError = "Error creating attraction.";
    public static readonly string AttractionUpdateError = "Error updating attraction.";
    public static readonly string AttractionsFetched = "Attractions fetched successfully.";

    // Region
    public static readonly string CouldNotFetchDescription = "Couldn't fetch description for region.";
    public static readonly string DescriptionFetchedRegion = "Fetched description for region.";
    public static readonly string CouldNotFetchCities = "Couldn't fetch cities for region.";
    public static readonly string CitiesFetchedRegion = "Fetched cities for region.";
    public static readonly string CouldNotFetchImages = "Couldn't fetch images for region.";
    public static readonly string ImagesFetchedRegion = "Fetched images for region.";
    public static readonly string CouldNotFetchRegionNames = "Couldn't fetch region names.";
    public static readonly string CouldNotFetchRegion = "Couldn't fetch region.";
    public static readonly string RegionNamesFetched = "Region names fetched successfully";
    public static readonly string RegionFetched = "Regions fetched successfully";
    public static readonly string RegionCreateError = "Couldn't create region.";
    public static readonly string RegionCreated = "Region created successfully.";
    public static readonly string RegionUpdateError = "Couldn't update region.";
    public static readonly string RegionUpdated = "Region updated successfully.";
    public static readonly string RegionDeleteError = "Couldn't delete region.";
    public static readonly string RegionDeleted = "Region deleted successfully.";
    public static readonly string RegionNotFound = "Region with this name doesn't exist.";

    // Review
    public static readonly string CouldNotFetchReviews = "Couldn't fetch reviews.";
    public static readonly string ReviewCreateError = "Couldn't create review.";
    public static readonly string ReviewDeleteError = "Couldn't delete review.";
    public static readonly string ReviewNotFound = "Review not found.";
    public static readonly string ReviewCreated = "Review created successfully.";
    public static readonly string ReviewDeleted = "Review deleted successfully.";
    public static readonly string ReviewsFetched = "Reviews fetched successfully.";
}

