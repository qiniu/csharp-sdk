using System;
using System.Runtime;
using System.Reflection;
using System.Text;

namespace Qiniu.FileOp
{
	public class ImageMogrify
	{
		public bool AutoOrient { get; set; }

		public string Thumbnail { get; set; }

		public string Gravity { get; set; }

		public string Crop { get; set; }

		public int Quality { get; set; }

		public int Rotate { get; set; }

		public string Format { get; set; }

		public string MakeRequest (string url)
		{
			string spec = url + "?imageMogr";
			if (AutoOrient)
				spec += "/auto-orient";
			if (!String.IsNullOrEmpty (Thumbnail))
				spec += "/thumbnail/" + Thumbnail;
			if (!String.IsNullOrEmpty (Gravity))
				spec += "/gravity/" + Gravity;
			if (!String.IsNullOrEmpty (Crop))
				spec += "/crop/" + Crop;
			if (Quality != 0)
				spec += "/quality/" + Quality.ToString ();
			if (Rotate != 0)
				spec += "/rotate/" + Rotate.ToString ();
			if (!String.IsNullOrEmpty (Format))
				spec += "/format/" + Format;
			return spec;           
		}
	}
}
