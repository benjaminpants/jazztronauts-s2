using Sandbox;


namespace Jazztronauts;


public class CollectedProp : Prop
{

	public float DestroyTimer = 5f;

	public CollectedProp()
	{
		
	}

	public CollectedProp( ModelEntity tocopy ) : this()
	{
		Model = tocopy.Model;
		Position = tocopy.Position;
		Rotation = tocopy.Rotation;
		RenderColor = Color.Magenta;
		PhysicsBody.GravityEnabled = false;
		ApplyLocalAngularImpulse(Vector3.Random * 200);
		Velocity = Vector3.Up * 100f;
		Scale = tocopy.Scale;
		Tags.Clear();
		Tags.Add("debris");
	}

	[Event.Tick.Server]
	protected void ServerUpdate()
	{
		Velocity *= 0.98f;
		DestroyTimer -= Time.Delta;
		if ( LocalScale > 0.5f)
		{
			LocalScale *= 0.99f;
		}
		if (DestroyTimer <= 0f)
		{
			Delete();
		}
	}


}

