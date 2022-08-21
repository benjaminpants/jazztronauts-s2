using System;
using System.Linq;
using Sandbox;

namespace Jazztronauts;

internal class Inventory : BaseInventory
{
	public Inventory(Player player) : base(player)
	{
	}

	public override bool CanAdd(Entity entity)
	{
		if (!entity.IsValid())
			return false;

		if (!base.CanAdd(entity))
			return false;

		return !IsCarryingType(entity.GetType());
	}

	public override bool Add(Entity entity, bool makeActive = false)
	{
		if (!entity.IsValid())
			return false;

		if (IsCarryingType(entity.GetType()))
			return false;

		return base.Add(entity, makeActive);
	}

	public bool IsCarryingType(Type t)
	{
		return List.Any(x => x?.GetType() == t);
	}

	public override bool SetActive(Entity ent)
	{
		Entity active_before = Active;
		bool toreturn = base.SetActive(ent);
		if (active_before == null) return toreturn;
		if (active_before != Active)
		{
			((Weapon)active_before).OnUnequipt();
			((Weapon)Active).OnEquipt();
		}
		return toreturn;
	}

	public override bool SetActiveSlot(int i, bool evenIfEmpty = false)
	{
		Entity active_before = Active;
		bool toreturn = base.SetActiveSlot(i, evenIfEmpty);
		if (active_before == null) return toreturn;
		if (active_before != Active)
		{
			((Weapon)active_before).OnUnequipt();
			((Weapon)Active).OnEquipt();
		}
		return toreturn;
	}

	public override bool Drop(Entity ent)
	{
		if (!Host.IsServer)
			return false;

		if (!Contains(ent))
			return false;

		if (ent is BaseCarriable bc)
		{
			bc.OnCarryDrop(Owner);
		}

		return ent.Parent == null;
	}
}