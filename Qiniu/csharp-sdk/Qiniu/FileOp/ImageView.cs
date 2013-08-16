using System;

namespace Qiniu.FileOp
{
	public class ImageView
	{
		/// <summary>
		/// 缩略模式
		/// </summary>
		/// <value>The mode.</value>
		public int Mode { get; set; }
		/// <summary>
		/// Width = 0 表示不限定宽度
		/// </summary>
		/// <value>The width.</value>
		public int Width { get; set; }
		/// <summary>
		/// Height = 0 表示不限定高度
		/// </summary>
		/// <value>The height.</value>
		public int Height { get; set; }
		/// <summary>
		///质量, 1-100
		/// </summary>
		/// <value>The quality.</value>
		public int Quality { get; set; }
		/// <summary>
		/// 输出格式，如jpg, gif, png, tif等等
		/// </summary>
		/// <value>The format.</value>
		public string Format { get; set; }
		/// <summary>
		/// Makes the request.
		/// </summary>
		/// <returns>The request.</returns>
		/// <param name="url">URL.</param>
		public string MakeRequest (string url)
		{
			string spec = url + "?imageView/" + Mode.ToString ();
			if (Width != 0)
				spec += "/w/" + Width.ToString ();
			if (Height != 0)
				spec += "/h/" + Height.ToString ();
			if (Quality != 0)
				spec += "/q/" + Quality.ToString ();
			if (!String.IsNullOrEmpty (Format))
				spec += "/format/" + Format;
			return spec;
		}
	}
}
