using System;
using Sandbox;

namespace Jazztronauts;

public class ViewModel : BaseViewModel
{
	protected float SwingInfluence => 0.05f;
	protected float ReturnSpeed => 5.0f;
	protected float MaxOffsetLength => 10.0f;
	protected float BobCycleTime => 7;
	protected Vector3 BobDirection => new(0.0f, 1.0f, 0.5f);

	private Vector3 _swingOffset;
	private float _lastPitch;
	private float _lastYaw;
	private float _bobAnim;

	private bool _activated;

	public bool EnableSwingAndBob = true;

	public float YawInertia { get; private set; }
	public float PitchInertia { get; private set; }

	public override void PostCameraSetup(ref CameraSetup camSetup)
	{
		base.PostCameraSetup(ref camSetup);

		if (!Local.Pawn.IsValid())
			return;

		if (!_activated)
		{
			_lastPitch = camSetup.Rotation.Pitch();
			_lastYaw = camSetup.Rotation.Yaw();

			YawInertia = 0;
			PitchInertia = 0;

			_activated = true;
		}

		Position = camSetup.Position;
		Rotation = camSetup.Rotation;

		int cameraBoneIndex = GetBoneIndex("camera");
		if (cameraBoneIndex != -1)
		{
			camSetup.Rotation *= Rotation.Inverse * GetBoneTransform(cameraBoneIndex).Rotation;
		}

		float newPitch = Rotation.Pitch();
		float newYaw = Rotation.Yaw();

		PitchInertia = Angles.NormalizeAngle(newPitch - _lastPitch);
		YawInertia = Angles.NormalizeAngle(_lastYaw - newYaw);

		if (EnableSwingAndBob)
		{
			Vector3 playerVelocity = Local.Pawn.Velocity;

			if (Local.Pawn is Player player)
			{
				PawnController controller = player.GetActiveController();
				if (controller != null && controller.HasTag("noclip"))
				{
					playerVelocity = Vector3.Zero;
				}
			}

			float verticalDelta = playerVelocity.z * Time.Delta;
			Vector3 viewDown = Rotation.FromPitch(newPitch).Up * -1.0f;
			verticalDelta *= 1.0f - MathF.Abs(viewDown.Cross(Vector3.Down).y);
			float pitchDelta = PitchInertia - verticalDelta * 1;
			float yawDelta = YawInertia;

			Vector3 offset = CalcSwingOffset(pitchDelta, yawDelta);
			offset += CalcBobbingOffset(playerVelocity);

			Position += Rotation * offset;
		}
		else
		{
			SetAnimParameter("aim_yaw_inertia", YawInertia);
			SetAnimParameter("aim_pitch_inertia", PitchInertia);
		}

		_lastPitch = newPitch;
		_lastYaw = newYaw;
	}

	protected Vector3 CalcSwingOffset(float pitchDelta, float yawDelta)
	{
		Vector3 swingVelocity = new(0, yawDelta, pitchDelta);

		_swingOffset -= _swingOffset * ReturnSpeed * Time.Delta;
		_swingOffset += swingVelocity * SwingInfluence;

		if (_swingOffset.Length > MaxOffsetLength)
		{
			_swingOffset = _swingOffset.Normal * MaxOffsetLength;
		}

		return _swingOffset;
	}

	protected Vector3 CalcBobbingOffset(Vector3 velocity)
	{
		_bobAnim += Time.Delta * BobCycleTime;

		const float twoPi = MathF.PI * 2.0f;

		if (_bobAnim > twoPi)
		{
			_bobAnim -= twoPi;
		}

		float speed = new Vector2(velocity.x, velocity.y).Length;
		speed = speed > 10.0 ? speed : 0.0f;
		Vector3 offset = BobDirection * (speed * 0.005f) * MathF.Cos(_bobAnim);
		offset = offset.WithZ(-MathF.Abs(offset.z));

		return offset;
	}
}