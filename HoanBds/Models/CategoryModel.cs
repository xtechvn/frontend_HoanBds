using Nest;

namespace HoanBds.Models
{
    public class CategoryModel
    {
        //public string name { get; set; }
        //public int cate_id { get; set; }
        //public int parent_id { get; set; }
        //public string path { get; set; }
        [PropertyName("id")]
        public int id { get; set; }

        [PropertyName("ParentId")]
        public int ParentId { get; set; }

        [PropertyName("PositionId")]
        public int? PositionId { get; set; }

        [PropertyName("Name")]
        public string Name { get; set; } = null!;

        [PropertyName("ImagePath")]
        public string? ImagePath { get; set; }

        [PropertyName("OrderNo")]
        public int? OrderNo { get; set; }

        [PropertyName("Path")]
        public string? Path { get; set; }

        [PropertyName("Status")]
        public int? Status { get; set; }

        [PropertyName("CreatedOn")]
        public DateTime? CreatedOn { get; set; }

        [PropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [PropertyName("Description")]
        public string? Description { get; set; }

        [PropertyName("IsShowHeader")]
        public bool IsShowHeader { get; set; }

        [PropertyName("IsShowFooter")]
        public bool IsShowFooter { get; set; }
        public List<CategoryModel> group_product_child { get; set; }

    }
}
