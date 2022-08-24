using Jazztronauts.Data.Interfaces;
using Sandbox;

namespace Jazztronauts.Data;

public class StolenProps : IGameData
{
	public StolenProps() { }

	public StolenProps(Model mdl)
	{
		ModelPath = mdl.ResourcePath;
		Count = 1;
		Worth = JazzHelpers.CalculateModelWorth(mdl);
	}

	public string ModelPath { get; set; }

	public int Count { get; set; }

	public long Worth { get; set; }

}