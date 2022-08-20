using Sandbox;
using Sandbox.UI;

namespace Jazztronauts.UI;

public class MainHUD : HudEntity<RootPanel>
{
	public MainHUD()
	{
		if (!IsClient) return;

		RootPanel.AddChild<CoinDisplay>();
	}
}