using Sandbox;

namespace Jazztronauts.Entities;

public class DroppedProp : Prop
{

	public DroppedProp()
	{
		if (!IsServer) return;
		Rotation = Rotation.Random;
		ApplyLocalAngularImpulse(Vector3.Random * 80);
		Velocity = Vector3.Up * 100f;
		Scale = 0.6f;
		Tags.Clear();
		Tags.Add("stolen"); //Stolen Tag works as placeholder solution until I figure out how to properly detect whether or not a prop supports physics. Because Source 2 is STUPID.
		DeleteAsync(5f);
	}

	public override void TakeDamage(DamageInfo info)
	{
		//no.
	}

}