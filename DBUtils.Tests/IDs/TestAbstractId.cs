using System;

namespace DBUtils.Tests.IDs
{
    public class TestAbstractId : AbstractId
    {
        public TestAbstractId(Guid root) : base(root.ToString()) { }

        public TestAbstractId() { }
    }
}
