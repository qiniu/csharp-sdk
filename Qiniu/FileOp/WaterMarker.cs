using Qiniu.RPC;

namespace Qiniu.FileOp
{
	public enum MarkerGravity
	{
		NorthWest = 0,
		North,
		NorthEast,
		West,
		Center,
		East,
		SouthWest,
		South,
		SouthEast
	}

	public class WaterMarker
	{
		protected static string[] Gravitys = new string[9] {
			"NorthWest",
			"North",
			"NorthEast",
			"West",
			"Center",
			"East",
			"SouthWest",
			"South",
			"SouthEast"
		};
		protected int dx;
		protected int dy;
		protected int dissolve;

		public int Dissolve {
			get { return dissolve; }
			set {
				if (value < 0)
					dissolve = 0;
				else if (value > 100)
					dissolve = 100;
				else
					dissolve = value;
			}
		}

		public MarkerGravity gravity;

		public WaterMarker (int dissolve = 50, MarkerGravity gravity = MarkerGravity.SouthEast, int dx = 10, int dy = 10)
		{
			Dissolve = dissolve;
			this.dissolve = dissolve;
			this.dx = dx;
			this.dy = dy;
			this.gravity = gravity;
		}

		public virtual string MakeRequest (string url)
		{
			return null;
		}

		public static ExifRet Call (string url)
		{
			CallRet callRet = FileOpClient.Get (url);
			return new ExifRet (callRet);
		}
	}
}
