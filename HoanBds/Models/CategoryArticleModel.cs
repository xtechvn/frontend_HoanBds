namespace HoanBds.Models
{
    public class CategoryArticleModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Lead { get; set; }
        public int Status { get; set; }
        public string Image169 { get; set; } = null!;
        public string Image43 { get; set; } = null!;
        public string Image11 { get; set; } = null!;
        public DateTime PublishDate { get; set; }
        public string AuthorName { get; set; }
        public string MainCategoryName { get; set; }
        public string MainCategoryId { get; set; }

        public int Position { get; set; }
        public int? PageView { get; set; }
        public string ListCategoryId { get; set; }
        public string ListCategoryName { get; set; }
    }
}
