using System;
using Sandbox;
using System.Collections.Generic;
using System.Linq;
using Jazztronauts.Data;
namespace Jazztronauts.Entities
{
	[Spawnable]
	[EditorModel("models/arrow.vmdl")]
	[Library("point_propdropper", Title = nameof(PropDropper))]
	public partial class PropDropper : Entity
	{

		public const float SpawnDelay = 0.1f;

		public Random RNG = new Random();

		public float SpawnTimer = SpawnDelay;

		private JazzPlayer CurrentPlayer;


		/// <summary>
		/// When the dropper starts dropping props. The player whose props it is dropping is the activator.
		/// </summary>
		protected Output OnDropStart { get; set; }

		/// <summary>
		/// When the dropper stops dropping props. The player whose props it stopped dropping is the activator.
		/// </summary>
		protected Output OnDropStop { get; set; }

		/// <summary>
		/// When the prop dropper can't start dropping props. (Either invalid entity or player has no props to drop)
		/// </summary>
		protected Output OnDropFail { get; set; }


		[Event.Tick.Server]
		protected void ServerUpdate()
		{
			if (CurrentPlayer == null) return;
			Jazztronauts.Data.Player CurrentPlayerData = CurrentPlayer.Data;
			SpawnTimer -= Time.Delta;
			if (SpawnTimer <= 0f)
			{
				SpawnTimer = SpawnDelay;
				if (CurrentPlayerData.StolenMapProps.Count == 0)
				{
					CurrentPlayer = null;
					OnDropStop.Fire(CurrentPlayer);
					return;
				}
				int CurrentIndex = RNG.Next(0, CurrentPlayerData.StolenMapProps.Count);
				if (CurrentPlayerData.StolenMapProps[CurrentIndex].Count > 0)
				{
					DropProp(CurrentPlayerData.StolenMapProps[CurrentIndex]);
					CurrentPlayerData.StolenMapProps[CurrentIndex].Count--;
				}
				if (CurrentPlayerData.StolenMapProps[CurrentIndex].Count == 0)
				{
					CurrentPlayerData.StolenMapProps.RemoveAt(CurrentIndex); //remove the first value and move on to the next.
				}
			}
		}

		public void DropProp(StolenProps prop)
		{
			foreach (JazzPlayer ply in JazztronautsGame.Instance.JazzPlayers)
			{
				ply.Data.Earned += prop.Worth;
				ply.UpdateClientDataEasy();
			}

			DroppedProp model = new DroppedProp();
			model.SetModel(prop.ModelPath);
			model.Position = this.Position;
		}


		/// <summary>
		/// Start the prop dropping process for the following entity. Any further inputs will be ignored until prop dropping is complete.
		/// </summary>
		[Input]
		public void DropProps(Entity ent)
		{
			if (CurrentPlayer != null) return;
			JazzPlayer player = (JazzPlayer)ent;
			if (player != null)
			{
				if (player.Data == null)
				{
					throw new Exception("Player Data is null! How did this happen??");
				}
				bool HasPropsToDrop = player.Data.StolenMapProps.Count != 0;
				if (HasPropsToDrop)
				{
					CurrentPlayer = player;
					SpawnTimer = SpawnDelay;
					OnDropStart.Fire(player);
				}
				else
				{
					OnDropFail.Fire(player);
				}
			}
			else
			{
				OnDropFail.Fire(player);
			}
		}

	}
}
