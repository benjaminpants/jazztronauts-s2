using Jazztronauts.Entities;
using Sandbox;
using Sandbox.UI;

namespace Jazztronauts.UI
{
	[UseTemplate("/Code/Ui/Data/CoinDisplay.html")]
	internal class CoinDisplay : Panel
	{
		public long Coins { get; set; } = 69420;

		public override void Tick()
		{
			JazzPlayer ply = (JazzPlayer)Game.LocalPawn;
			Coins = ply.Money;
		}
	}
}