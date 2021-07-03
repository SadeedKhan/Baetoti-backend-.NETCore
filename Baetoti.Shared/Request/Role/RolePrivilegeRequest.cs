using System.Collections.Generic;

namespace Baetoti.Shared.Request.Role
{
    public class RolePrivilegeRequest
    {
        public long ID { get; set; }

        public string RoleName { get; set; }

        public List<MenuRequest> Menu { get; set; }

        RolePrivilegeRequest()
        {
            Menu = new List<MenuRequest>();
        }

    }

    public class MenuRequest
    {
        public long ID { get; set; }

        public bool IsSelected { get; set; }

        public List<PrivilegeRequest> SelectedPrivileges { get; set; }

        public List<SubMenuRequest> SelectedSubMenu { get; set; }

        MenuRequest()
        {
            SelectedPrivileges = new List<PrivilegeRequest>();
            SelectedSubMenu = new List<SubMenuRequest>();
        }

    }

    public class SubMenuRequest
    {
        public long ID { get; set; }

        public bool IsSelected { get; set; }

        public List<PrivilegeRequest> SelectedPrivileges { get; set; }

        SubMenuRequest()
        {
            SelectedPrivileges = new List<PrivilegeRequest>();
        }

    }

    public class PrivilegeRequest
    {
        public long ID { get; set; }

        public bool IsSelected { get; set; }

    }
}
