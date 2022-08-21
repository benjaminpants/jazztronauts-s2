using Sandbox.UI;
using Sandbox;

namespace Jazztronauts.UI
{
	[UseTemplate("/Code/Ui/Data/CoinDisplay.html")]
	internal class CoinDisplay : Panel
	{
		public int Coins { get; set; } = 69420;

		public override void Tick()
		{
			JazzPlayer ply = (JazzPlayer)Local.Pawn;

			Coins = ply.Money;
		}
	}
}