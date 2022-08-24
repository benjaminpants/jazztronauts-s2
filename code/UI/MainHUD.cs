using Sandbox.UI;

namespace Jazztronauts.UI;

public class MainHUD : RootPanel
{
	public MainHUD()
	{
		AddChild<CoinDisplay>();
		AddChild<ChatBox>();
	}
}