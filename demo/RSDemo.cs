using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using QBox.Auth;
using QBox.RS;

namespace QBox.Example
{
    public class RSDemo
    {
        public static string bucketName;
        public static string key;
        public static string localFile;
        public static string DEMO_DOMAIN;
        public static Client conn;
        public static RSService rs;

        public static void Main()
        {
            Config.ACCESS_KEY = "<Please apply your access key>";
            Config.SECRET_KEY = "<Dont send your secret key to anyone>";

            Config.ACCESS_KEY = "iDn5DxTwQTuxHnXfZ5evK54VtoPHavFZA9lerwQ5";
            Config.SECRET_KEY = "YDVvLC00BMGlwCKHFt3qJfslCala2Z7Bl4lNLMKF";

            bucketName = "csharpbucket";
            DEMO_DOMAIN = "csharpbucket.dn.qbox.me";
            conn = new DigestAuthClient();
            rs = new RSService(conn, bucketName);
            localFile = Process.GetCurrentProcess().MainModule.FileName;
            key = System.IO.Path.GetFileName(localFile);

            CallRet callRet = rs.MkBucket();
            PrintRet(callRet);

            RSPutFile();
            RSClientPutFile();
            Get();
            Stat();
            Publish();
            UnPublish();
            Delete();
            Drop();

            Console.ReadLine();
        }

        public static void RSPutFile()
        {
            Console.WriteLine("\n===> RSService.PutFile");
            PutFileRet putFileRet = rs.PutFile(key, null, localFile, null);
            PrintRet(putFileRet);
            if (putFileRet.OK)
            {
                Console.WriteLine("Hash: " + putFileRet.Hash);
            }
            else
            {
                Console.WriteLine("Failed to PutFile");
            }
        }

        public static void RSClientPutFile()
        {
            Console.WriteLine("\n==> PutAuth");
            PutAuthRet putAuthRet = rs.PutAuth();
            PrintRet(putAuthRet);
            if (putAuthRet.OK)
            {
                Console.WriteLine("Expires: " + putAuthRet.Expires.ToString());
                Console.WriteLine("Url: " + putAuthRet.Url);
            }
            else
            {
                Console.WriteLine("Failed to PutAuth");
            }

            Console.WriteLine("\n===> RSClient.PutFile");
            PutFileRet putFileRet = RSClient.PutFile(putAuthRet.Url, bucketName, key, null, localFile, null, "key=<key>");
            PrintRet(putFileRet);
            if (putFileRet.OK)
            {
                Console.WriteLine("Hash: " + putFileRet.Hash);
            }
            else
            {
                Console.WriteLine("Failed to RSClient.PutFile");
            }

            Console.WriteLine("\n===> Generate UpToken");
            var authPolicy = new AuthPolicy(bucketName, 3600);
            string upToken = authPolicy.MakeAuthTokenString();
            Console.WriteLine("upToken: " + upToken);

            Console.WriteLine("\n===> RSClient.PutFileWithUpToken");
            putFileRet = RSClient.PutFileWithUpToken(upToken, bucketName, key, null, localFile, null, "key=<key>");
            PrintRet(putFileRet);
            if (putFileRet.OK)
            {
                Console.WriteLine("Hash: " + putFileRet.Hash);
            }
            else
            {
                Console.WriteLine("Failed to RSClient.PutFileWithUpToken");
            }
        }

        public static void Get()
        {
            Console.WriteLine("\n===> Get");
            GetRet getRet = rs.Get(key, "attName");
            PrintRet(getRet);
            if (getRet.OK)
            {
                Console.WriteLine("Hash: " + getRet.Hash);
                Console.WriteLine("FileSize: " + getRet.FileSize);
                Console.WriteLine("MimeType: " + getRet.MimeType);
                Console.WriteLine("Url: " + getRet.Url);
            }
            else
            {
                Console.WriteLine("Failed to Get");
            }

            Console.WriteLine("\n===> GetIfNotModified");
            getRet = rs.GetIfNotModified(key, "attName", getRet.Hash);
            PrintRet(getRet);
            if (getRet.OK)
            {
                Console.WriteLine("Hash: " + getRet.Hash);
                Console.WriteLine("FileSize: " + getRet.FileSize);
                Console.WriteLine("MimeType: " + getRet.MimeType);
                Console.WriteLine("Url: " + getRet.Url);
            }
            else
            {
                Console.WriteLine("Failed to GetIfNotModified");
            }
        }

        public static void Stat()
        {
            Console.WriteLine("\n===> Stat");
            StatRet statRet = rs.Stat(key);
            PrintRet(statRet);
            if (statRet.OK)
            {
                Console.WriteLine("Hash: " + statRet.Hash);
                Console.WriteLine("FileSize: " + statRet.FileSize);
                Console.WriteLine("PutTime: " + statRet.PutTime);
                Console.WriteLine("MimeType: " + statRet.MimeType);
            }
            else
            {
                Console.WriteLine("Failed to Stat");
            }
        }

        public static void Delete()
        {
            Console.WriteLine("\n===> Delete");
            CallRet deleteRet = rs.Delete(key);
            PrintRet(deleteRet);
            if (!deleteRet.OK)
            {
                Console.WriteLine("Failed to Delete");
            }
        }

        public static void Drop()
        {
            Console.WriteLine("\n===> Drop");
            CallRet dropRet = rs.Drop();
            PrintRet(dropRet);
            if (!dropRet.OK)
            {
                Console.WriteLine("Failed to Drop");
            }
        }

        public static void Publish()
        {
            Console.WriteLine("\n===> Publish");
            CallRet publishRet = rs.Publish(DEMO_DOMAIN);
            PrintRet(publishRet);
            if (!publishRet.OK)
            {
                Console.WriteLine("Failed to Publish");
            }
        }

        public static void UnPublish()
        {
            Console.WriteLine("\n===> UnPublish");
            CallRet publishRet = rs.Unpublish(DEMO_DOMAIN);
            PrintRet(publishRet);
            if (!publishRet.OK)
            {
                Console.WriteLine("Failed to UnPublish");
            }
        }

        public static void PrintRet(CallRet callRet)
        {
            Console.WriteLine("\n[CallRet]");
            Console.WriteLine("StatusCode: " + callRet.StatusCode.ToString());
            Console.WriteLine("Response:\n" + callRet.Response);
            Console.WriteLine();
        }
    }
}
