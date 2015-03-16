using System;

namespace DBUtils.Tests.IDs
{
    public class TestAbstractId : AbstractId
    {
        public TestAbstractId(Guid root)
        {
            this.RootId = root;
        }
    }
}
