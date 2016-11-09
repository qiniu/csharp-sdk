using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Qiniu.Examples
{
    public class Settings
    {
        public static void Load(string file,out string ak,out string sk)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    ak = sr.ReadLine();
                    sk = sr.ReadLine();
                }
            }
        }
    }
}
