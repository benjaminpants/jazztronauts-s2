using System;
using Sandbox;
using System.Collections.Generic;
using System.Linq;
using Jazztronauts.Data;
using SandboxEditor;

namespace Jazztronauts.Entities
{
	[Spawnable]
	[HammerEntity]
	[EditorModel("models/arrow.vmdl")]
	[Library("point_propdropper", Title = nameof(PropDropper))]
	public class PropDropper : Entity
	{

		public const float SpawnDelay = 0.1f;

		public Random RNG = new Random();

		public float SpawnTimer = SpawnDelay;

		private JazzPlayer CurrentPlayer;


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
				}
				else
				{
				}
			}
			else
			{
				Log.Warning("Tried to call DropProps without a valid player!");
			}
		}

	}
}
