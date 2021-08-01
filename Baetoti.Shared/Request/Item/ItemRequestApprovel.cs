using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Request.Item
{
    public class ItemRequestApprovel
    {
        public long ItemID { get; set; }

        public bool IsApproved { get; set; }
    }
}
