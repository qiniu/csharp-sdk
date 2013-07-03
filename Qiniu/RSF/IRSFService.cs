using System;

namespace QBox.RSF
{
	/// <summary>
	/// Resource Storage Fetch Interface
	/// </summary>
	public interface IRSFService
	{
		DumpRet ListPrefix(string bucketName, string prefix,string markerIn,int limit); 
	}
}

