using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.Store
{
    public class StoreResponse
    {
        public long ID { get; set; }
        public long ProviderID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool IsAddressHidden { get; set; }
        public string VideoURL { get; set; }
        public int Status { get; set; }
        public string CoverImage { get; set; }
        public string BusinessLogo { get; set; }
        public string InstagramGallery { get; set; }
    }
}
