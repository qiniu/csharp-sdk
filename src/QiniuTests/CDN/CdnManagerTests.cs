using NUnit.Framework;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.Tests;
using System;

namespace Qiniu.CDN.Tests
{
    [TestFixture]
    public class CdnManagerTests : TestEnv
    {

        [Test]
        public void RefreshUrlsAndDirsTest()
        {
            string[] urls = {
                string.Format("http://{0}/images/1.png",Domain),
                string.Format("http://{0}/images/2.png",Domain)
            };

            string[] dirs = {
                string.Format("http://{0}/images1/",Domain),
                string.Format("http://{0}/images2/",Domain)
            };

            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager manager = new CdnManager(mac);
            RefreshResult ret = manager.RefreshUrlsAndDirs(urls, dirs);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail(ret.ToString());
            }
            Console.WriteLine(ret.Result.Code);
            Console.WriteLine(ret.Result.Error);
            Console.WriteLine(ret.Result.UrlQuotaDay);
            Console.WriteLine(ret.Result.UrlSurplusDay);
            Console.WriteLine(ret.Result.DirQuotaDay);
            Console.WriteLine(ret.Result.DirSurplusDay);
            Console.WriteLine(ret.Result.RequestId);
            if (ret.Result.InvalidUrls != null)
            {
                foreach (string url in ret.Result.InvalidUrls)
                {
                    Console.WriteLine(url);
                }
            }
            if (ret.Result.InvalidDirs != null)
            {
                foreach (string dir in ret.Result.InvalidDirs)
                {
                    Console.WriteLine(dir);
                }
            }
        }

        [Test]
        public void RefreshUrlsTest()
        {
            string[] urls = {
                string.Format("http://{0}/images/1.png",Domain),
                string.Format("http://{0}/images/2.png",Domain)
            };

            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager manager = new CdnManager(mac);
            RefreshResult ret = manager.RefreshUrls(urls);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail(ret.ToString());
            }
            Console.WriteLine(ret.Result.Code);
            Console.WriteLine(ret.Result.Error);
            Console.WriteLine(ret.Result.UrlQuotaDay);
            Console.WriteLine(ret.Result.UrlSurplusDay);
            Console.WriteLine(ret.Result.RequestId);
            if (ret.Result.InvalidUrls != null)
            {
                foreach (string url in ret.Result.InvalidUrls)
                {
                    Console.WriteLine(url);
                }
            }
        }

        [Test]
        public void RefreshDirsTest()
        {
            string[] dirs = {
                string.Format("http://{0}/images1/",Domain),
                string.Format("http://{0}/images2/",Domain)
            };

            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager manager = new CdnManager(mac);
            RefreshResult ret = manager.RefreshDirs(dirs);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail(ret.ToString());
            }
            Console.WriteLine(ret.Result.Code);
            Console.WriteLine(ret.Result.Error);
            Console.WriteLine(ret.Result.DirQuotaDay);
            Console.WriteLine(ret.Result.DirSurplusDay);
            Console.WriteLine(ret.Result.RequestId);
            if (ret.Result.InvalidDirs != null)
            {
                foreach (string dir in ret.Result.InvalidDirs)
                {
                    Console.WriteLine(dir);
                }
            }
        }


        [Test]
        public void PrefetchUrlsTest()
        {
            string[] urls = {
                string.Format("http://{0}/images/1.png",Domain),
                string.Format("http://{0}/images/2.png",Domain)
            };

            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager manager = new CdnManager(mac);
            PrefetchResult ret = manager.PrefetchUrls(urls);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail(ret.ToString());
            }
            Console.WriteLine(ret.Result.Code);
            Console.WriteLine(ret.Result.Error);
            Console.WriteLine(ret.Result.QuotaDay);
            Console.WriteLine(ret.Result.SurplusDay);
            Console.WriteLine(ret.Result.RequestId);
            if (ret.Result.InvalidUrls != null)
            {
                foreach (string url in ret.Result.InvalidUrls)
                {
                    Console.WriteLine(url);
                }
            }
        }

        [Test]
        public void GetBandwidthDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager manager = new CdnManager(mac);

            string[] domains = new string[] { Domain };
            string start = "2017-08-01";
            string end = "2017-08-10";
            string granu = "day";

            BandwidthResult ret = manager.GetBandwidthData(domains, start, end, granu);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail(ret.ToString());
            }

            foreach (string domain in domains)
            {
                Console.WriteLine("bandwidth data of domain: " + domain);
                foreach (string t in ret.Result.Time)
                {
                    Console.Write(t + "\t");
                }
                Console.WriteLine();
                if (ret.Result.Data.ContainsKey(domain))
                {
                    if (ret.Result.Data[domain].China != null)
                    {
                        Console.WriteLine("China:");
                        foreach (int v in ret.Result.Data[domain].China)
                        {
                            Console.Write(v + "\t");
                        }
                        Console.WriteLine();
                    }


                    if (ret.Result.Data[domain].Oversea != null)
                    {
                        Console.WriteLine("Oversea:");
                        foreach (UInt64 v in ret.Result.Data[domain].Oversea)
                        {
                            Console.Write(v + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        [Test]
        public void GetFluxDataTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager manager = new CdnManager(mac);

            string[] domains = new string[] { Domain };
            string start = "2017-08-01";
            string end = "2017-08-10";
            string granu = "day";

            FluxResult ret = manager.GetFluxData(domains, start, end, granu);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail(ret.ToString());
            }

            foreach (string domain in domains)
            {
                Console.WriteLine("flux data of domain: " + domain);
                foreach (string t in ret.Result.Time)
                {
                    Console.Write(t + "\t");
                }
                Console.WriteLine();
                if (ret.Result.Data.ContainsKey(domain))
                {
                    if (ret.Result.Data[domain].China != null)
                    {
                        Console.WriteLine("China:");
                        foreach (UInt64 v in ret.Result.Data[domain].China)
                        {
                            Console.Write(v + "\t");
                        }
                        Console.WriteLine();
                    }


                    if (ret.Result.Data[domain].Oversea != null)
                    {
                        Console.WriteLine("Oversea:");
                        foreach (int v in ret.Result.Data[domain].Oversea)
                        {
                            Console.Write(v + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        [Test]
        public void GetCdnLogListTest()
        {
            Mac mac = new Mac(AccessKey, SecretKey);
            CdnManager manager = new CdnManager(mac);
            string day = "2017-08-10";
            string[] domains = new string[] { Domain};
            LogListResult ret = manager.GetCdnLogList(domains, day);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail(ret.ToString());
            }

            foreach (string domain in ret.Result.Data.Keys)
            {
                Console.WriteLine("log list for domain: " + domain);
                foreach (LogData data in ret.Result.Data[domain])
                {
                    Console.WriteLine(data.Name + "\t" + data.Size + "\t" + data.Mtime + "\t" + data.Url);
                }
            }
        }


        [Test]
        public void CreateTimestampAntiLeechUrlTest()
        {
            string host = "http://qnls.example.com";
            string fileName = "hello/6000694.ls";
            string query = "";
            int expireInSeconds = 3600;
            string encryptKey = "xxx";
            string finalUrl = CdnManager.CreateTimestampAntiLeechUrl(host, fileName, query, encryptKey, expireInSeconds);
            Console.WriteLine(finalUrl);
        }

    }
}