using System;
using Qiniu.Http;
using Qiniu.Tests;
using Qiniu.Util;
using Xunit;

namespace Qiniu.CDN.Tests
{
    public class CdnManagerTests : TestEnv
    {
        [Fact]
        public void CreateTimestampAntiLeechUrlTest()
        {
            var host = "http://qnls.example.com";
            var fileName = "hello/6000694.ls";
            var query = "";
            var expireInSeconds = 3600;
            var encryptKey = "xxx";
            var finalUrl = CdnManager.CreateTimestampAntiLeechUrl(host, fileName, query, encryptKey, expireInSeconds);
            Console.WriteLine(finalUrl);
        }

        [Fact]
        public void GetBandwidthDataTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);

            var domains = new [] { Domain };
            var start = "2017-08-01";
            var end = "2017-08-10";
            var granu = "day";

            var ret = manager.GetBandwidthData(domains, start, end, granu);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            foreach (var domain in domains)
            {
                Console.WriteLine("bandwidth data of domain: " + domain);
                foreach (var t in ret.Result.Time) Console.Write(t + "\t");
                Console.WriteLine();
                if (ret.Result.Data.ContainsKey(domain))
                {
                    if (ret.Result.Data[domain].China != null)
                    {
                        Console.WriteLine("China:");
                        foreach (int v in ret.Result.Data[domain].China) Console.Write(v + "\t");
                        Console.WriteLine();
                    }


                    if (ret.Result.Data[domain].Oversea != null)
                    {
                        Console.WriteLine("Oversea:");
                        foreach (var v in ret.Result.Data[domain].Oversea) Console.Write(v + "\t");
                        Console.WriteLine();
                    }
                }
            }
        }

        [Fact]
        public void GetCdnLogListTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var day = "2017-08-10";
            var domains = new [] { Domain };
            var ret = manager.GetCdnLogList(domains, day);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            foreach (var domain in ret.Result.Data.Keys)
            {
                Console.WriteLine("log list for domain: " + domain);
                foreach (var data in ret.Result.Data[domain]) Console.WriteLine(data.Name + "\t" + data.Size + "\t" + data.Mtime + "\t" + data.Url);
            }
        }

        [Fact]
        public void GetFluxDataTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);

            var domains = new [] { Domain };
            var start = "2017-08-01";
            var end = "2017-08-10";
            var granu = "day";

            var ret = manager.GetFluxData(domains, start, end, granu);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            foreach (var domain in domains)
            {
                Console.WriteLine("flux data of domain: " + domain);
                foreach (var t in ret.Result.Time) Console.Write(t + "\t");
                Console.WriteLine();
                if (ret.Result.Data.ContainsKey(domain))
                {
                    if (ret.Result.Data[domain].China != null)
                    {
                        Console.WriteLine("China:");
                        foreach (var v in ret.Result.Data[domain].China) Console.Write(v + "\t");
                        Console.WriteLine();
                    }


                    if (ret.Result.Data[domain].Oversea != null)
                    {
                        Console.WriteLine("Oversea:");
                        foreach (int v in ret.Result.Data[domain].Oversea) Console.Write(v + "\t");
                        Console.WriteLine();
                    }
                }
            }
        }


        [Fact]
        public void PrefetchUrlsTest()
        {
            string[] urls =
            {
                string.Format("http://{0}/images/1.png", Domain),
                string.Format("http://{0}/images/2.png", Domain)
            };

            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var ret = manager.PrefetchUrls(urls);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            Console.WriteLine(ret.Result.Code);
            Console.WriteLine(ret.Result.Error);
            Console.WriteLine(ret.Result.QuotaDay);
            Console.WriteLine(ret.Result.SurplusDay);
            Console.WriteLine(ret.Result.RequestId);
            if (ret.Result.InvalidUrls != null)
            {
                foreach (var url in ret.Result.InvalidUrls) Console.WriteLine(url);
            }
        }

        [Fact]
        public void RefreshDirsTest()
        {
            string[] dirs =
            {
                string.Format("http://{0}/images1/", Domain),
                string.Format("http://{0}/images2/", Domain)
            };

            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var ret = manager.RefreshDirs(dirs);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            Console.WriteLine(ret.Result.Code);
            Console.WriteLine(ret.Result.Error);
            Console.WriteLine(ret.Result.DirQuotaDay);
            Console.WriteLine(ret.Result.DirSurplusDay);
            Console.WriteLine(ret.Result.RequestId);
            if (ret.Result.InvalidDirs != null)
            {
                foreach (var dir in ret.Result.InvalidDirs) Console.WriteLine(dir);
            }
        }

        [Fact]
        public void RefreshUrlsAndDirsTest()
        {
            string[] urls =
            {
                string.Format("http://{0}/images/1.png", Domain),
                string.Format("http://{0}/images/2.png", Domain)
            };

            string[] dirs =
            {
                string.Format("http://{0}/images1/", Domain),
                string.Format("http://{0}/images2/", Domain)
            };

            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var ret = manager.RefreshUrlsAndDirs(urls, dirs);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
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
                foreach (var url in ret.Result.InvalidUrls) Console.WriteLine(url);
            }

            if (ret.Result.InvalidDirs != null)
            {
                foreach (var dir in ret.Result.InvalidDirs) Console.WriteLine(dir);
            }
        }

        [Fact]
        public void RefreshUrlsTest()
        {
            string[] urls =
            {
                string.Format("http://{0}/images/1.png", Domain),
                string.Format("http://{0}/images/2.png", Domain)
            };

            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var ret = manager.RefreshUrls(urls);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            Console.WriteLine(ret.Result.Code);
            Console.WriteLine(ret.Result.Error);
            Console.WriteLine(ret.Result.UrlQuotaDay);
            Console.WriteLine(ret.Result.UrlSurplusDay);
            Console.WriteLine(ret.Result.RequestId);
            if (ret.Result.InvalidUrls != null)
            {
                foreach (var url in ret.Result.InvalidUrls) Console.WriteLine(url);
            }
        }
    }
}
