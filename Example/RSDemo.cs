using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace QBox
{
    public class RSDemo
    {
        public static string tableName = "Bucket";
        public static string key;
        public static string localFile;
        public static string DEMO_DOMAIN = "iovip.qbox.me/bucket";
        public static Client conn;
        public static RSService rs;

        public static void PutFile()
        {
            Console.WriteLine("\n--- PutFile ---");
            PrintInput(tableName, key, localFile, null);
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

        public static void Get()
        {
            Console.WriteLine("\n--- Get ---");
            PrintInput(tableName, key, null, null);
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

            Console.WriteLine("\n--- GetIfNotModified ---");
            PrintInput(tableName, key, null, null);
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
            Console.WriteLine("\n--- Stat ---");
            PrintInput(tableName, key, null, null);
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
            Console.WriteLine("\n--- Delete ---");
            PrintInput(tableName, key, null, null);
            DeleteRet deleteRet = rs.Delete(key);
            PrintRet(deleteRet);
            if (!deleteRet.OK)
            {
                Console.WriteLine("Failed to Delete");
            }
        }

        public static void Drop()
        {
            Console.WriteLine("\n--- Drop ---");
            PrintInput(tableName, null, null, null);
            DropRet dropRet = rs.Drop();
            PrintRet(dropRet);
            if (!dropRet.OK)
            {
                Console.WriteLine("Failed to Drop");
            }
        }

        public static void Publish()
        {
            Console.WriteLine("\n--- Publish ---");
            PrintInput(tableName, null, null, DEMO_DOMAIN);
            PublishRet publishRet = rs.Publish(DEMO_DOMAIN);
            PrintRet(publishRet);
            if (!publishRet.OK)
            {
                Console.WriteLine("Failed to Publish");
            }
        }

        public static void UnPublish()
        {
            Console.WriteLine("\n--- UnPublish ---");
            PrintInput(tableName, null, null, DEMO_DOMAIN);
            PublishRet publishRet = rs.UnPublish(DEMO_DOMAIN);
            PrintRet(publishRet);
            if (!publishRet.OK)
            {
                Console.WriteLine("Failed to UnPublish");
            }
        }

        public static PutAuthRet PutAuth()
        {
            Console.WriteLine("\n--- PutAuth ---");
            PrintInput(null, null, null, null);
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
            return putAuthRet;
        }

        public static void CliPutFile()
        {
            PutAuthRet putAuthRet = PutAuth();
            Console.WriteLine("\n--- CliPutFile ---");
            PrintInput(tableName, key, localFile, null);
            PutFileRet putFileRet = RSClient.PutFile(putAuthRet.Url, tableName,
                            key, null, localFile, null, "key=Config.cs");
            PrintRet(putFileRet);
            if (putFileRet.OK)
            {
                Console.WriteLine("Hash: " + putFileRet.Hash);
            }
            else
            {
                Console.WriteLine("Failed to CliPutFile");
            }

        }

        public static void PrintInput(
            string tblName, string key, string localFile, string domain)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n[In]");
            if (!String.IsNullOrEmpty(tblName))
                sb.AppendLine("TableName: " + tblName);
            if (!String.IsNullOrEmpty(key))
                sb.AppendLine("Key: " + key);
            if (!String.IsNullOrEmpty(localFile))
                sb.AppendLine("LocalFile: " + localFile);
            if (!String.IsNullOrEmpty(domain))
                sb.AppendLine("Domain: " + domain);
            Console.WriteLine(sb.ToString());
        }


        public static void PrintRet(CallRet callRet)
        {
            Console.WriteLine("\n[Out]");
            
            Console.WriteLine("\nCallRet");
            Console.WriteLine("StatusCode: " + callRet.StatusCode.ToString());
            Console.WriteLine("Response:\n" + callRet.Response);
            Console.WriteLine();
        }

        public static void Main()
        {
            Config.ACCESS_KEY = "RLT1NBD08g3kih5-0v8Yi6nX6cBhesa2Dju4P7mT";
            Config.SECRET_KEY = "k6uZoSDAdKBXQcNYG3UOm4bP3spDVkTg-9hWHIKm";

            conn = new DigestAuthClient();
            rs = new RSService(conn, tableName);
            localFile = Process.GetCurrentProcess().MainModule.FileName;
            key = System.IO.Path.GetFileName(localFile);

            PutFile();
            Get();
            Stat();
            Delete();
            CliPutFile();
            Get();
            Stat();
            Publish();
            UnPublish();
            Drop();

            Console.ReadLine();
        }
    }
}
