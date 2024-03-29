﻿using System;
using Sandbox;

//had to copy this entire class JUST so i could increase the jumpheight. That's stupid

namespace Jazztronauts
{

	[Library]
	public partial class JazzWalkController : WalkController
	{
		[Net] public float JumpMultiplier { get; set; } = 1.0f;

		public override void CheckJumpButton()
		{
			//if ( !player->CanJump() )
			//    return false;


			/*
            if ( player->m_flWaterJumpTime )
            {
                player->m_flWaterJumpTime -= gpGlobals->frametime();
                if ( player->m_flWaterJumpTime < 0 )
                    player->m_flWaterJumpTime = 0;

                return false;
            }*/



			// If we are in the water most of the way...
			if (Swimming)
			{
				// swimming, not jumping
				ClearGroundEntity();

				Velocity = Velocity.WithZ(100);

				// play swimming sound
				//  if ( player->m_flSwimSoundTime <= 0 )
				{
					// Don't play sound again for 1 second
					//   player->m_flSwimSoundTime = 1000;
					//   PlaySwimSound();
				}

				return;
			}

			if (GroundEntity == null)
				return;

			/*
            if ( player->m_Local.m_bDucking && (player->GetFlags() & FL_DUCKING) )
                return false;
            */

			/*
            // Still updating the eye position.
            if ( player->m_Local.m_nDuckJumpTimeMsecs > 0u )
                return false;
            */

			ClearGroundEntity();

			// player->PlayStepSound( (Vector &)mv->GetAbsOrigin(), player->m_pSurfaceData, 1.0, true );

			// MoveHelper()->PlayerSetAnimation( PLAYER_JUMP );

			float flGroundFactor = 1.0f;
			//if ( player->m_pSurfaceData )
			{
				//   flGroundFactor = g_pPhysicsQuery->GetGameSurfaceproperties( player->m_pSurfaceData )->m_flJumpFactor;
			}

			float flMul = 268.3281572999747f * 1.2f;

			float startz = Velocity.z;

			if (Duck.IsActive)
				flMul *= 0.8f;

			flMul *= JumpMultiplier;

			Velocity = Velocity.WithZ(startz + flMul * flGroundFactor);

			Velocity -= new Vector3(0, 0, Gravity * 0.5f) * Time.Delta;

			// mv->m_outJumpVel.z += mv->m_vecVelocity[2] - startz;
			// mv->m_outStepHeight += 0.15f;

			// don't jump again until released
			//mv->m_nOldButtons |= IN_JUMP;

			AddEvent("jump");

		}
	}
}
