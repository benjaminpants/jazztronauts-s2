using Sandbox;


namespace Jazztronauts;

partial class JazzPlayer : Player
{

	[Net]
	public int Money { get; set; } = 0;


	public ClothingContainer Clothing = new();

	public JazzPlayer()
	{
		Inventory = new Inventory(this);
	}
	

	public JazzPlayer( Client cl ) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient( cl );
	}

	public override void Respawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController
		{
			WalkSpeed = 120,
			SprintSpeed = 240,
			DefaultSpeed = 120,
			AirAcceleration = 100,
			AutoJump = true

		};

		Animator = new StandardPlayerAnimator();

		CameraMode = new FirstPersonCamera();

		Clothing.DressEntity( this );

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		//Clothing.DressEntity( this );

		var ps = new PropSnatcher();

		Inventory.Add( ps, true );


		//Inventory.SetActiveSlot(0,false);

		base.Respawn();
	}

	public Entity SearchForStealables( )
	{
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 120 )
			.Ignore( this )
			.Run();

		// See if any of the parent entities are usable if we ain't.
		var ent = tr.Entity;
		while ( ent.IsValid() && !JazzHelpers.CheckIfEntityIsValidStealable( ent ) )
		{
			ent = ent.Parent;
		}

		// Nothing found, try a wider search
		if ( !JazzHelpers.CheckIfEntityIsValidStealable( ent ) )
		{
			tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 85 )
			.Radius( 2 )
			.Ignore( this )
			.Run();

			// See if any of the parent entities are usable if we ain't.
			ent = tr.Entity;
			while ( ent.IsValid() && !JazzHelpers.CheckIfEntityIsValidStealable( ent ) )
			{
				ent = ent.Parent;
			}
		}

		if ( JazzHelpers.CheckIfEntityIsValidStealable( ent ) ) return ent;

		return null;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		SimulateActiveChild(cl, Inventory.Active);

		

		if ( Input.Pressed( InputButton.View ) )
		{
			if ( CameraMode is ThirdPersonCamera )
			{
				CameraMode = new FirstPersonCamera();
			}
			else
			{
				CameraMode = new ThirdPersonCamera();
			}
		}
	}

}
