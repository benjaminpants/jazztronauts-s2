using System;
using System.Collections.Generic;
using System.Linq;
using Jazztronauts.Data;
using Sandbox;

namespace Jazztronauts;

public static class JazzHelpers
{
	public static bool CheckIfEntityIsValidStealable(Entity ent)
	{
		if (ent.IsValid())
		{
			return ent.ClassName != "worldent" && ent.ClassName != "JazzPlayer" && ent.ClassName != "CollectedProp";
		}
		return false;
	}

	public static long CalculateWorth(this List<StolenProps> me)
	{
		long final_worth = 0;
		foreach (StolenProps prop in me)
		{
			final_worth += (prop.Worth * prop.Count);
		}
		return final_worth;
	}

	public static int CalculateModelWorth(Model mdl)
	{
		int baseValue = (int)mdl.Bounds.Size.Length / 5 + 1;
		return baseValue;
	}

	public static Vector3? GetRandomSpot(Vector3 offset)
	{
		if (!NavMesh.IsLoaded)
		{
			return Entity.All.OrderBy(x => Guid.NewGuid()).ToList().FirstOrDefault().Position + ((Vector3.Random * 128f) * new Vector3(1,1,0));
		}

		//NavMesh.GetPointWithinRadius();

		Vector3? val = NavMesh.GetPointWithinRadius(Entity.All.OfType<SpawnPoint>().OrderBy(x => Guid.NewGuid()).FirstOrDefault().Position, 200f, 99999f);
		if (val.HasValue)
		{
			return val.Value + offset;
		}

		return val;
		//(Vector3.Up * 64f);
	}

}