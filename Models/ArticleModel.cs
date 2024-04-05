namespace CampGroupPlanner.Models
{
	public class ArticleModel
	{
		public Guid Id { get; set; }
		public DateTime PublishedDate { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? SourceLink { get; set; }

	}
}
