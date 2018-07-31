using System;
using System.Threading.Tasks;
using Qiniu.CDN;
using Qiniu.Http;
using Qiniu.Util;
using Xunit;

namespace Qiniu.Tests.CDN
{
    public class CdnManagerTests : TestEnv
    {
        [Fact]
        public void CreateTimestampAntiLeechUrlTest()
        {
            const string host = "http://qnls.example.com";
            const string fileName = "hello/6000694.ls";
            const string query = "";
            const int expireInSeconds = 3600;
            const string encryptKey = "xxx";
            var finalUrl = CdnManager.CreateTimestampAntiLeechUrl(host, fileName, query, encryptKey, expireInSeconds);
            Console.WriteLine(finalUrl);
        }

        [Fact]
        public async Task GetBandwidthDataTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);

            var domains = new[] { Domain };
            var start = "2017-08-01";
            var end = "2017-08-10";
            var granu = "day";

            var ret = await manager.GetBandwidthData(domains, start, end, granu);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            foreach (var domain in domains)
            {
                Console.WriteLine("bandwidth data of domain: " + domain);
                if (ret.Result.Time != null) Console.WriteLine(string.Join("\t", ret.Result.Time));
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
                        foreach (var v in ret.Result.Data[domain].Oversea) Console.Write(v + "\t");
                        Console.WriteLine();
                    }
                }
            }
        }

        [Fact]
        public async Task GetCdnLogListTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var day = "2017-08-10";
            var domains = new[] { Domain };
            var ret = await manager.GetCdnLogList(domains, day);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            foreach (var domain in ret.Result.Data.Keys)
            {
                Console.WriteLine("log list for domain: " + domain);
                foreach (var data in ret.Result.Data[domain]) Console.WriteLine($"{data.Name}\t{data.Size}\t{data.Mtime}\t{data.Url}");
            }
        }

        [Fact]
        public async Task GetFluxDataTest()
        {
            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);

            var domains = new[] { Domain };
            var start = "2017-08-01";
            var end = "2017-08-10";
            var granu = "day";

            var ret = await manager.GetFluxData(domains, start, end, granu);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.True(false, ret.ToString());
            }

            foreach (var domain in domains)
            {
                Console.WriteLine("flux data of domain: " + domain);
                if (ret.Result.Time != null) Console.WriteLine(string.Join("\t", ret.Result.Time));
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
                        foreach (var v in ret.Result.Data[domain].Oversea) Console.Write(v + "\t");
                        Console.WriteLine();
                    }
                }
            }
        }


        [Fact]
        public async Task PrefetchUrlsTest()
        {
            string[] urls =
            {
                $"http://{Domain}/images/1.png",
                $"http://{Domain}/images/2.png"
            };

            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var ret = await manager.PrefetchUrls(urls);
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
        public async Task RefreshDirsTest()
        {
            string[] dirs =
            {
                $"http://{Domain}/images1/",
                $"http://{Domain}/images2/"
            };

            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var ret = await manager.RefreshDirs(dirs);
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
        public async Task RefreshUrlsAndDirsTest()
        {
            string[] urls =
            {
                $"http://{Domain}/images/1.png",
                $"http://{Domain}/images/2.png"
            };

            string[] dirs =
            {
                $"http://{Domain}/images1/",
                $"http://{Domain}/images2/"
            };

            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var ret = await manager.RefreshUrlsAndDirs(urls, dirs);
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
        public async Task RefreshUrlsTest()
        {
            string[] urls =
            {
                $"http://{Domain}/images/1.png",
                $"http://{Domain}/images/2.png"
            };

            var mac = new Mac(AccessKey, SecretKey);
            var manager = new CdnManager(mac);
            var ret = await manager.RefreshUrls(urls);
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
