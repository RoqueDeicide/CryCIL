using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents an object that provides information about a surface of physical body: bounciness,
	/// friction and ricochet-related parameters.
	/// </summary>
	public struct PhysicalSurface
	{
		#region Fields
		private readonly int index;
		#endregion
		#region Properties
		internal int Index => this.index;
		/// <summary>
		/// Gets or sets restitution modifier of this surface.
		/// </summary>
		public float Bounciness
		{
			get
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				this.GetParameters(out bounciness, out friction, out flags);
				return bounciness;
			}
			set
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				this.GetParameters(out bounciness, out friction, out flags);
				this.SetParameters(value, friction, flags);
			}
		}
		/// <summary>
		/// Gets or sets friction modifier of this surface.
		/// </summary>
		public float Friction
		{
			get
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				this.GetParameters(out bounciness, out friction, out flags);
				return friction;
			}
			set
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				this.GetParameters(out bounciness, out friction, out flags);
				this.SetParameters(bounciness, value, flags);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that specify this surface.
		/// </summary>
		public SurfaceFlags Flags
		{
			get
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				this.GetParameters(out bounciness, out friction, out flags);
				return flags;
			}
			set
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				this.GetParameters(out bounciness, out friction, out flags);
				this.SetParameters(bounciness, friction, value);
			}
		}
		/// <summary>
		/// Gets or sets a modifier that is used when reducing the 'damage' value that is inflicted by
		/// bullets that pierce this surface.
		/// </summary>
		public float DamageReduction
		{
			get
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				float damageReduction;
				float ricochetAngle;
				float ricochetDamageReduction;
				float ricochetVelocityReduction;
				this.GetParameters(out bounciness, out friction, out damageReduction, out ricochetAngle,
								   out ricochetDamageReduction, out ricochetVelocityReduction, out flags);
				return damageReduction;
			}
			set
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				float damageReduction;
				float ricochetAngle;
				float ricochetDamageReduction;
				float ricochetVelocityReduction;
				this.GetParameters(out bounciness, out friction, out damageReduction, out ricochetAngle,
								   out ricochetDamageReduction, out ricochetVelocityReduction, out flags);
				this.SetParameters(bounciness, friction, value, ricochetAngle, ricochetDamageReduction,
								   ricochetVelocityReduction, flags);
			}
		}
		/// <summary>
		/// Gets or sets the modifier that is used when calculating the ricochet angle.
		/// </summary>
		public float RicochetAngle
		{
			get
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				float damageReduction;
				float ricochetAngle;
				float ricochetDamageReduction;
				float ricochetVelocityReduction;
				this.GetParameters(out bounciness, out friction, out damageReduction, out ricochetAngle,
								   out ricochetDamageReduction, out ricochetVelocityReduction, out flags);
				return ricochetAngle;
			}
			set
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				float damageReduction;
				float ricochetAngle;
				float ricochetDamageReduction;
				float ricochetVelocityReduction;
				this.GetParameters(out bounciness, out friction, out damageReduction, out ricochetAngle,
								   out ricochetDamageReduction, out ricochetVelocityReduction, out flags);
				this.SetParameters(bounciness, friction, damageReduction, value, ricochetDamageReduction,
								   ricochetVelocityReduction, flags);
			}
		}
		/// <summary>
		/// Gets or sets a modifier that is used when reducing the 'damage' value that is inflicted by
		/// bullets that ricocheted off this surface.
		/// </summary>
		public float RicochetDamageReduction
		{
			get
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				float damageReduction;
				float ricochetAngle;
				float ricochetDamageReduction;
				float ricochetVelocityReduction;
				this.GetParameters(out bounciness, out friction, out damageReduction, out ricochetAngle,
								   out ricochetDamageReduction, out ricochetVelocityReduction, out flags);
				return ricochetDamageReduction;
			}
			set
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				float damageReduction;
				float ricochetAngle;
				float ricochetDamageReduction;
				float ricochetVelocityReduction;
				this.GetParameters(out bounciness, out friction, out damageReduction, out ricochetAngle,
								   out ricochetDamageReduction, out ricochetVelocityReduction, out flags);
				this.SetParameters(bounciness, friction, damageReduction, ricochetAngle, value,
								   ricochetVelocityReduction, flags);
			}
		}
		/// <summary>
		/// Gets or sets a modifier that is used when reducing the velocity of bullets that ricocheted off
		/// this surface.
		/// </summary>
		public float RicochetVelocityReduction
		{
			get
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				float damageReduction;
				float ricochetAngle;
				float ricochetDamageReduction;
				float ricochetVelocityReduction;
				this.GetParameters(out bounciness, out friction, out damageReduction, out ricochetAngle,
								   out ricochetDamageReduction, out ricochetVelocityReduction, out flags);
				return ricochetVelocityReduction;
			}
			set
			{
				float bounciness;
				float friction;
				SurfaceFlags flags;
				float damageReduction;
				float ricochetAngle;
				float ricochetDamageReduction;
				float ricochetVelocityReduction;
				this.GetParameters(out bounciness, out friction, out damageReduction, out ricochetAngle,
								   out ricochetDamageReduction, out ricochetVelocityReduction, out flags);
				this.SetParameters(bounciness, friction, damageReduction, ricochetAngle, ricochetDamageReduction,
								   value, flags);
			}
		}
		#endregion
		#region Construction
		internal PhysicalSurface(int index)
		{
			this.index = index;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets basic parameters that specify this physical surface.
		/// </summary>
		/// <param name="bounciness">Restitution modifier of the surface.</param>
		/// <param name="friction">  Friction modifier of the surface.</param>
		/// <param name="flags">     A set of flags that are assigned to this surface.</param>
		public void GetParameters(out float bounciness, out float friction, out SurfaceFlags flags)
		{
			PhysicalWorld.GetSurfaceParameters(this.index, out bounciness, out friction, out flags);
		}
		/// <summary>
		/// Sets basic parameters that specify this physical surface.
		/// </summary>
		/// <param name="bounciness">Restitution modifier of the surface.</param>
		/// <param name="friction">  Friction modifier of the surface.</param>
		/// <param name="flags">     A set of flags to assign to this surface.</param>
		public void SetParameters(float bounciness, float friction, SurfaceFlags flags)
		{
			PhysicalWorld.SetSurfaceParameters(this.index, bounciness, friction, flags);
		}
		/// <summary>
		/// Gets an extended set of parameters that specify this physical surface.
		/// </summary>
		/// <param name="bounciness">               Restitution modifier of the surface.</param>
		/// <param name="friction">                 Friction modifier of the surface.</param>
		/// <param name="damageReduction">          
		/// A modifier that is used when reducing the 'damage' value that is inflicted by bullets that
		/// pierce this surface.
		/// </param>
		/// <param name="ricochetAngleModifier">    
		/// A modifier that is used when calculating the ricochet angle.
		/// </param>
		/// <param name="ricochetDamageReduction">  
		/// A modifier that is used when reducing the 'damage' value that is inflicted by bullets that
		/// ricocheted off this surface.
		/// </param>
		/// <param name="ricochetVelocityReduction">
		/// A modifier that is used when reducing the velocity of bullets that ricocheted off this surface.
		/// </param>
		/// <param name="flags">                    
		/// A set of flags that are assigned to this surface.
		/// </param>
		public void GetParameters(out float bounciness, out float friction, out float damageReduction,
								  out float ricochetAngleModifier, out float ricochetDamageReduction,
								  out float ricochetVelocityReduction, out SurfaceFlags flags)
		{
			PhysicalWorld.GetSurfaceParametersExt(this.index, out bounciness, out friction, out damageReduction,
												  out ricochetAngleModifier, out ricochetDamageReduction,
												  out ricochetVelocityReduction, out flags);
		}
		/// <summary>
		/// Sets an extended set of parameters that specify this physical surface.
		/// </summary>
		/// <param name="bounciness">               Restitution modifier of the surface.</param>
		/// <param name="friction">                 Friction modifier of the surface.</param>
		/// <param name="damageReduction">          
		/// A modifier that is used when reducing the 'damage' value that is inflicted by bullets that
		/// pierce this surface.
		/// </param>
		/// <param name="ricochetAngleModifier">    
		/// A modifier that is used when calculating the ricochet angle.
		/// </param>
		/// <param name="ricochetDamageReduction">  
		/// A modifier that is used when reducing the 'damage' value that is inflicted by bullets that
		/// ricocheted off this surface.
		/// </param>
		/// <param name="ricochetVelocityReduction">
		/// A modifier that is used when reducing the velocity of bullets that ricocheted off this surface.
		/// </param>
		/// <param name="flags">                    
		/// A set of flags that are assigned to this surface.
		/// </param>
		public void SetParameters(float bounciness, float friction, float damageReduction, float ricochetAngleModifier,
								  float ricochetDamageReduction, float ricochetVelocityReduction, SurfaceFlags flags)
		{
			PhysicalWorld.SetSurfaceParametersExt(this.index, bounciness, friction, damageReduction, ricochetAngleModifier,
												  ricochetDamageReduction, ricochetVelocityReduction, flags);
		}
		#endregion
		#region Utilities
		#endregion
	}
}