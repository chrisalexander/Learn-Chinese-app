using System;
using DBUtils.Model;

namespace DBUtils.Tests.Model.IDs
{
    public class TestAbstractId : AbstractId
    {
        public TestAbstractId(Guid root)
        {
            this.RootId = root;
        }
    }
}
