using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace BasicCompany.Feature.Blog
{
    public static class Templates
    {
        public static class Post
        {
            public static readonly ID Id = new ID("{4F5919BD-5B93-4D02-BAD1-5E13692C1ED8}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{DE6BBF4B-1B59-45FA-8E19-F37F64A7B33F}");
                public static readonly ID PreviewImage = new ID("{31C675F8-4CF7-4046-AE33-C67568918EB4}");
                public static readonly ID Author = new ID("{865011A0-6160-4B17-ABAE-38C47DD3337A}");
                public static readonly ID ShortDescription = new ID("{0B398AAF-913B-4631-ACAD-E126F0737E43}");
                public static readonly ID Content = new ID("{8371ED22-E93E-4466-A5D2-78873FA0D677}");
                public static readonly ID Tags = new ID("{6288ACFE-0839-443A-B6AF-B39A3D53D871}");
            }

        }
    }
}