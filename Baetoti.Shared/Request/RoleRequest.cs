using System.Collections.Generic;

namespace Baetoti.Shared.Request
{
    public class RoleRequest
    {
        public string RoleName { get; set; }

        public List<MenuRequest> Menu = new List<MenuRequest>();

    }

    public class MenuRequest
    {
        public long ID { get; set; }

        public bool IsSelected { get; set; }

        public List<PrivilegeRequest> SelectedPrivileges = new List<PrivilegeRequest>();

        public List<SubMenuRequest> SelectedSubMenu = new List<SubMenuRequest>();
    }

    public class SubMenuRequest
    {
        public long ID { get; set; }

        public bool IsSelected { get; set; }

        public List<PrivilegeRequest> SelectedPrivileges = new List<PrivilegeRequest>();
    }

    public class PrivilegeRequest
    {
        public long ID { get; set; }

        public bool IsSelected { get; set; }

    }
}
