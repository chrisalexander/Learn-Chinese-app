using System;

namespace DBUtils.Tests.IDs
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
