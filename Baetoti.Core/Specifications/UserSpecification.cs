using Baetoti.Core.Entites;
using Baetoti.Core.Specifications.Base;
using System;
using System.Linq.Expressions;

namespace Baetoti.Core.Specifications
{
    public class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(Expression<Func<User, bool>> criteria) : base(criteria)
        {
            //AddInclude(x => x.User);
            // AddInclude("User");
        }
    }
}
