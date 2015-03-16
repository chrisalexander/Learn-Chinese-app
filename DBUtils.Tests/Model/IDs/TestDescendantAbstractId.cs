using System;
using DBUtils.Model;

namespace DBUtils.Tests.Model.IDs
{
    public class TestDescendantAbstractId : DescendantAbstractId<TestAbstractId>
    {
        public TestDescendantAbstractId(TestAbstractId parent, Guid root)
        {
            this.ParentId = parent;
            this.RootId = root;
        }
    }
}
