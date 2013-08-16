using System;
using System.Text;
using Qiniu.Util;

namespace Qiniu.FileOp
{
	public class TextWaterMarker:WaterMarker
	{      
		public string text;
		private string fontName;
		private int fontSize;
		private string color;

		public TextWaterMarker (string text, string fontname = "", string color = "", int fontsize = 0, int dissolve = 50, MarkerGravity gravity = MarkerGravity.SouthEast, int dx = 10, int dy = 10)
            : base(dissolve,gravity, dx, dy)
		{
			this.text = text;
			this.fontName = fontname;
			this.fontSize = fontsize;
			this.color = color;
		}

		public override string MakeRequest (string url)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append (string.Format ("{0}?watermark/{1}", url, 2));
			if (string.IsNullOrEmpty (text)) {
				throw new Exception ("No Text To Draw");
			}
			sb.Append ("/text/" + text.ToBase64URLSafe ());

			if (!string.IsNullOrEmpty (fontName)) {
				sb.Append ("/font/" + fontName.ToBase64URLSafe ());
			}
			if (fontSize > 0) {
				sb.Append ("/fontsize/" + fontSize);
			}
			if (!string.IsNullOrEmpty (color)) {
				sb.Append ("/fill/" + color.ToBase64URLSafe ());
			}
			sb.Append ("/dissolve/" + dissolve);
			sb.Append ("/gravity/" + Gravitys [(int)gravity]);
			sb.Append ("/dx/" + dx);
			sb.Append ("/dy/" + dy);
			return sb.ToString ();            
		}
	}
}
