using System;

namespace DBUtils.Tests.IDs
{
    public class TestDescendantAbstractId : DescendantAbstractId<TestAbstractId>
    {
        public TestDescendantAbstractId(TestAbstractId parent, Guid root) : base(parent, root.ToString()) { }

        public TestDescendantAbstractId(TestAbstractId parent) : base(parent) { }

        public TestDescendantAbstractId(string root) : base(root) { }

        public TestDescendantAbstractId() { }
    }
}
