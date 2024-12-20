﻿namespace HoanBds.Models.GroupProduct
{
    public partial class GroupProductModel
    {
        public int id { get; set; }

        public int parentid { get; set; }

        public int? positionid { get; set; }

        public string name { get; set; } = null!;

        public string? imagepath { get; set; }

        public int? orderno { get; set; }

        public string? path { get; set; }

        public int? status { get; set; }

        public DateTime? createdon { get; set; }

        public DateTime? modifiedon { get; set; }


        public string? description { get; set; }

        public bool isshowheader { get; set; }

        public bool isshowfooter { get; set; }

        public List<GroupProductModel> group_product_child { get; set; }
    }
}
