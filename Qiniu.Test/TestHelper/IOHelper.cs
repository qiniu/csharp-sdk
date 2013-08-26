using System;
using System.IO;

namespace Qiniu.Test.TestHelper
{
	public class TmpFIle
	{
		public string FileName;
		static string fileContent="Hello,Qiniu Cloud!";
		public TmpFIle(int fileSize)
		{
			FileName = string.Format ("./tmpFile{0}", Guid.NewGuid ().ToString ());
			using (FileStream fstream = new FileStream (FileName,FileMode.CreateNew))
			using (StreamWriter writer = new StreamWriter (fstream)) {
				for (int i=0; i<fileSize/fileContent.Length; i++) {
					writer.WriteLine (fileContent);
				}
				writer.WriteLine (fileContent.Substring (0, fileSize % fileContent.Length));
			}
		}

		~TmpFIle()
		{
			if (File.Exists (FileName)) {
				File.Delete (FileName);
			}
		}

		public void Del()
		{

			File.Delete (FileName);
		}

	}

}

