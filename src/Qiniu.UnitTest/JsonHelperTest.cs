using NUnit.Framework;
using Qiniu.JSON;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class JsonHelperTest
    {
        [Test]
        public void SerializeTest()
        {
            var putPolicy = new Qiniu.IO.Model.PutPolicy()
            {
                Scope = "test:file-1",
                CallbackUrl = "http://a.bb.ccc/dddd",
                CallbackBodyType = Qiniu.Http.ContentType.APPLICATION_JSON,
                CallbackBody = "Key = $(key), Name = x:name",
                CallbackHost = "0.0.0.0",
                DetectMime = null,
                DeleteAfterDays =1,
                EndUser = "USER",
                FsizeMin = 1000,
                FsizeLimit = 100000,
                InsertOnly = null,
                MimeLimit = "image/*",
                PersistentOps = "OPS",
                PersistentNotifyUrl = "http://notifyurl",
                PersistentPipeline = "PIPELINE",
                ReturnUrl = "http://return",
                ReturnBody = "RETURN",
                SaveKey = "file-1",                
            };

            putPolicy.SetExpires(3600);

            JsonHelper.JsonSerializer = new AnotherJsonSerializer();

            string jstr = JsonHelper.Serialize(putPolicy);

            Assert.IsNotNull(jstr);
        }

        [Test]
        public void DeserializeTest()
        {
            string jstr = "{ \"scope\":\"test\", \"deadline\":12345 }";
            Qiniu.IO.Model.PutPolicy putPolicy = null;

            JsonHelper.JsonDeserializer = new AnotherJsonDeserializer();

            bool ok = JsonHelper.Deserialize(jstr,out putPolicy);

            Assert.IsTrue(ok);
            Assert.IsNotNull(putPolicy);
        }

    }

    class AnotherJsonSerializer:IJsonSerializer
    {
        public string Serialize<T>(T obj) where T:new()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings);
        }
    }

    class AnotherJsonDeserializer:IJsonDeserializer
    {
        public bool Deserialize<T>(string str, out T obj) where T : new()
        {
            obj = default(T);

            bool ok = true;

            try
            {
                obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            }
            catch (System.Exception)
            {
                ok = false;
            }

            return ok;
        }
    }

}
