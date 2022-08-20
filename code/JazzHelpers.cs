using Sandbox;
using System;
using System.Linq;

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

	public static int CalculateModelWorth(Model mdl)
	{
		int baseValue = (int)mdl.Bounds.Size.Length / 5 + 1;
		return baseValue;
	}

	public static Vector3? GetRandomSpot(Vector3 offset)
	{
		if (!NavMesh.IsLoaded)
		{
			throw new Exception("No proper navmesh was found. TODO: Make a shard spawning system for maps that dont have navmeshes");
		}

		//NavMesh.GetPointWithinRadius();

		Vector3? val = NavMesh.GetPointWithinRadius(Entity.All.OfType<SpawnPoint>().OrderBy(x => Guid.NewGuid()).FirstOrDefault().Position, 200f, 99999f);
		if (val.HasValue)
		{
			return val.Value + offset;
		}
		else
		{
			return val;
		}
		//(Vector3.Up * 64f);
	}

}