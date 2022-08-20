using Sandbox;

namespace Jazztronauts;

public static class JazzHelpers
{
	public static bool CheckIfEntityIsValidStealable(Entity ent)
	{
		if ( ent.IsValid() )
		{
			return ent.ClassName != "worldent" && ent.ClassName != "JazzPlayer";
		}
		return false;
	}

	public static int CalculateModelWorth(Model mdl)
	{
		int base_value = ((int)mdl.Bounds.Size.Length / 5) + 1;
		return base_value;
	}
	
	
}
