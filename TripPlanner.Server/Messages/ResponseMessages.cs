namespace TripPlanner.Server.Messages
{
    public static class ResponseMessages
    {
        // Authorization related messages
        public const string UserAlreadyLoggedIn = "User is already logged in.";
        public const string InvalidEmail = "The email address provided is invalid.";
        public const string EmailNotConfirmed = "The email address has not been confirmed.";
        public const string UserNotFound = "No user found with the provided email address.";
        public const string InvalidLogin = "Invalid email or password.";
        public const string EmailSendFailed = "Could not send the email.";
        public const string EmailSendSuccessful = "Link has been sent to your email.";
        public const string PasswordResetFailed = "Could not reset the password.";
        public const string PasswordResetSuccessful = "Password has been reset successfully.";
        public const string EmailConfirmed = "Email has been confirmed successfully.";
        public const string EmailConfirmationFailed = "Email confirmation failed.";
        public const string UnexpectedError = "Unexpected error occurred.";

        // Image or Region validation
        public const string RegionValidationError = "Region validation error.";
        public const string ImageUploadError = "Image upload error.";
        public const string InvalidModelState = "Invalid model state.";

        // Article
        public const string ArticleNotFound = "Couldn't find article with this ID.";
        public const string CouldNotFetchArticles = "Couldn't fetch articles from database.";
        public const string ArticleExists = "Article already exists.";
        public const string ArticleCreated = "Article created successfully.";
        public const string ArticleUpdated = "Article updated successfully.";
        public const string ArticleDeleted = "Article deleted successfully. {0}";
        public const string ArticleDeleteError = "Error deleting article.";
        public const string ArticleUpdateError = "Error updating article.";
        public const string ArticleCreateError = "Error creating article.";
        public const string ArticlesFetched = "Articles fetched successfully.";

        // Attraction
        public const string CouldNotFetchAttractions = "Couldn't fetch attractions from database.";
        public const string AttractionNotFound = "Attraction not found.";
        public const string AttractionCreated = "Attraction created successfully.";
        public const string AttractionUpdated = "Attraction updated successfully.";
        public const string AttractionDeleted = "Attraction deleted successfully.";
        public const string AttractionDeleteError = "Error deleting attraction.";
        public const string AttractionCreateError = "Error creating attraction.";
        public const string AttractionUpdateError = "Error updating attraction.";
        public const string AttractionsFetched = "Attractions fetched successfully.";

        // Region
        public const string CouldNotFetchDescription = "Couldn't fetch description for region.";
        public const string DescriptionFetchedRegion = "Fetched description for region.";
        public const string CouldNotFetchCities = "Couldn't fetch cities for region.";
        public const string CitiesFetchedRegion = "Fetched cities for region.";
        public const string CouldNotFetchImages = "Couldn't fetch images for region.";
        public const string ImagesFetchedRegion = "Fetched images for region.";
        public const string CouldNotFetchRegionNames = "Couldn't fetch region names.";
        public const string CouldNotFetchRegion = "Couldn't fetch region.";
        public const string RegionNamesFetched = "Region names fetched successfully";
        public const string RegionFetched = "Regions fetched successfully";
        public const string RegionCreateError = "Couldn't create region.";
        public const string RegionCreated = "Region created successfully.";
        public const string RegionUpdateError = "Couldn't update region.";
        public const string RegionUpdated = "Region updated successfully.";
        public const string RegionDeleteError = "Couldn't delete region.";
        public const string RegionDeleted = "Region deleted successfully.";
        public const string RegionNotFound = "Region with this name doesn't exist.";

        // Review
        public const string CouldNotFetchReviews = "Couldn't fetch reviews.";
        public const string ReviewCreateError = "Couldn't create review.";
        public const string ReviewDeleteError = "Couldn't delete review.";
        public const string ReviewNotFound = "Review not found.";
        public const string ReviewCreated = "Review created successfully.";
        public const string ReviewDeleted = "Review deleted successfully.";
        public const string ReviewsFetched = "Reviews fetched successfully.";
    }
}
