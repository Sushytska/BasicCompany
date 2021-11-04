using Sitecore.Data;

namespace BasicCompany.Feature.News
{
    public static class Templates
    {
        public static class News
        {
            public static readonly ID Id = new ID("{F562AF3B-1C23-4684-B7C4-18952BC08033}");

            public static class Fields
            {
                public static readonly ID NewsTitle = new ID("{CD883091-5CCC-4E05-87F6-375F57069DE8}");
                public static readonly ID NewsHeaderImage = new ID("{5629D19F-0B55-40CA-AD15-5219A6A5C0FE}");
                public static readonly ID NewsContent = new ID("{8EA70858-0096-4184-989A-84F7BA837056}");
                public static readonly ID NewsAuthor = new ID("{B0DD1564-B311-47F2-BFDD-928806959B8A}");
                public static readonly ID NewsRelated = new ID("{531EA361-5C1D-406F-94A8-97786B36C568}");
            }
        }
    }
}