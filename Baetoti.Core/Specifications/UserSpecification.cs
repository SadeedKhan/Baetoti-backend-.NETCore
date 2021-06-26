using Baetoti.Core.Entites;
using Baetoti.Core.Specifications.Base;
using System;
using System.Linq.Expressions;

namespace Baetoti.Core.Specifications
{
    public class UserSpecification : BaseSpecification<Employee>
    {
        public UserSpecification(Expression<Func<Employee, bool>> criteria) : base(criteria)
        {
            //AddInclude(x => x.User);
            // AddInclude("User");
        }
    }
}
