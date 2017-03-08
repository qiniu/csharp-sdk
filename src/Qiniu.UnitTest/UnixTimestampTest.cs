using NUnit.Framework;
using Qiniu.Util;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class UnixTimestampTest
    {
        [Test]
        public void ConvertToTimestampTest()
        {
            var dt1 = System.DateTime.Now;
            long ts = UnixTimestamp.ConvertToTimestamp(dt1);
            var dt2 = UnixTimestamp.ConvertToDateTime(ts);
            int diff = (int)dt1.Subtract(dt2).TotalSeconds;
            Assert.AreEqual(0, diff);
        }

    }
}
