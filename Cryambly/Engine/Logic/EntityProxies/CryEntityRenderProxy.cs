using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Rendering;
using CryCil.Engine.Rendering.Nodes;
using CryCil.Geometry;
using CryCil.Graphics;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents a part of the entity that is responsible for getting this entity rendered on screen.
	/// </summary>
	public struct CryEntityRenderProxy
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets the bounds of this entity in world-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox WorldBounds
		{
			get
			{
				this.AssertInstance();

				BoundingBox box;
				GetWorldBounds(this.handle, out box);
				return box;
			}
		}
		/// <summary>
		/// Gets or sets the bounds of this entity in entity-space.
		/// </summary>
		/// <remarks>
		/// When set, the bounds will be recalculated after next change is done to the entity.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox LocalBounds
		{
			get
			{
				this.AssertInstance();

				BoundingBox box;
				GetLocalBounds(this.handle, out box);
				return box;
			}
			set
			{
				this.AssertInstance();

				SetLocalBounds(this.handle, ref value, false);
			}
		}
		/// <summary>
		/// Gets the material that is used to render the first renderable slot of this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material RenderMaterial
		{
			get
			{
				this.AssertInstance();

				return GetRenderMaterialInternal(this.handle, -1);
			}
		}
		/// <summary>
		/// Gets the render node that is hosted by this proxy.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryRenderNode RenderNode
		{
			get
			{
				this.AssertInstance();

				return GetRenderNode(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that specify which material layers must be active.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public MaterialLayerActivityFlags ActiveMaterialLayers
		{
			get
			{
				this.AssertInstance();

				return GetMaterialLayersMask(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetMaterialLayersMask(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets flags that specify which material layer should be blended(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public MaterialLayerBlendFlags BlendedMaterialLayers
		{
			get
			{
				this.AssertInstance();

				return GetMaterialLayersBlend(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetMaterialLayersBlend(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates this entity is in the state of cloak interference(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool CloakInterferenceState
		{
			get
			{
				this.AssertInstance();

				return GetCloakInterferenceState(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetCloakInterferenceState(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the strength of the cloak highlight(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float CloakHighlightStrength
		{
			set
			{
				this.AssertInstance();

				SetCloakHighlightStrength(this.handle, value);
			}
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public byte CloakColorChannel
		{
			get
			{
				this.AssertInstance();

				return GetCloakColorChannel(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetCloakColorChannel(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether cloak render should fade out of the picture as
		/// player moves further and further away(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool CloakFadeByDistance
		{
			get
			{
				this.AssertInstance();

				return DoesCloakFadeByDistance(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetCloakFadeByDistance(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the scaling factor for a time it takes to transition between cloaked and uncloaked
		/// states.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float CloakBlendTimeScale
		{
			get
			{
				this.AssertInstance();

				return GetCloakBlendTimeScale(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetCloakBlendTimeScale(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether cloak render should ignore the cloak refraction
		/// color(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IgnoreCloakRefractionColor
		{
			get
			{
				this.AssertInstance();

				return DoesIgnoreCloakRefractionColor(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetIgnoreCloakRefractionColor(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the name of the post effect to apply to this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string CustomPostEffect
		{
			set
			{
				this.AssertInstance();

				SetCustomPostEffect(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this entity's rendering should be affected by the
		/// interference filter that is applied to HUD, when this entity is rendered in Post 3D Render mode.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IgnoreHudInterferenceFilter
		{
			set
			{
				this.AssertInstance();

				SetIgnoreHudInterferenceFilter(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this entity's rendering should be done with depth test,
		/// when this entity is rendered in Post 3D Render mode.
		/// </summary>
		/// <remarks>
		/// Setting this to <c>true</c> makes this entity render behind entities that are rendered with a
		/// flag <see cref="EntitySlotFlags.RenderNearest"/> set.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool RequireDepthTestOnHud
		{
			set
			{
				this.AssertInstance();

				SetHUDRequireDepthTest(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this entity's rendering should be done without bloom, when
		/// this entity is rendered in Post 3D Render mode.
		/// </summary>
		/// <remarks>Set this to <c>true</c> when it causes overglow and weird visual artifacts.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool DisableBloomOnHud
		{
			set
			{
				this.AssertInstance();

				SetHUDDisableBloom(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this entity's rendering should be done without heat
		/// effects.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IgnoreHeat
		{
			set
			{
				this.AssertInstance();

				SetIgnoreHeatAmount(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a set of 4 numbers from 0 to 1 that represent the scale of vision effect that is
		/// applied to this entity.
		/// </summary>
		/// <remarks>
		/// Used by CryTek's games to designate the heat signature of this entity for thermal vision.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ColorSingle VisionScaleSingle
		{
			get
			{
				this.AssertInstance();

				return new ColorByte(GetVisionParams(this.handle));
			}
			set
			{
				this.AssertInstance();

				SetVisionParams(this.handle, value.R, value.G, value.B, value.A);
			}
		}
		/// <summary>
		/// Gets or sets a set of 4 numbers from 0 to 255 that represent the scale of vision effect that is
		/// applied to this entity.
		/// </summary>
		/// <remarks>
		/// Used by CryTek's games to designate the heat signature of this entity for thermal vision.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ColorByte VisionScaleByte
		{
			get
			{
				this.AssertInstance();

				return new ColorByte(GetVisionParams(this.handle));
			}
			set
			{
				this.AssertInstance();

				SetVisionParams(this.handle,
								value.Red / 255.0f, value.Green / 255.0f,
								value.Blue / 255.0f, value.Alpha / 255.0f);
			}
		}
		/// <summary>
		/// Gets or sets a set of 4 numbers from 0 to 1 that represent the color of the silhouette that is
		/// render around this entity on HUD.
		/// </summary>
		/// <remarks>Can be used to highlight the interactive items in the world.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ColorSingle SilhouetteColorSingle
		{
			get
			{
				this.AssertInstance();

				return new ColorByte(GetHUDSilhouettesParams(this.handle));
			}
			set
			{
				this.AssertInstance();

				SetHUDSilhouettesParams(this.handle, value.R, value.G, value.B, value.A);
			}
		}
		/// <summary>
		/// Gets or sets a set of 4 numbers from 0 to 255 that represent the color of the silhouette that is
		/// render around this entity on HUD.
		/// </summary>
		/// <remarks>Can be used to highlight the interactive items in the world.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ColorByte SilhouetteColorByte
		{
			get
			{
				this.AssertInstance();

				return new ColorByte(GetHUDSilhouettesParams(this.handle));
			}
			set
			{
				this.AssertInstance();

				SetHUDSilhouettesParams(this.handle,
										value.Red / 255.0f, value.Green / 255.0f,
										value.Blue / 255.0f, value.Alpha / 255.0f);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity should look like it dissolves in the
		/// shadow.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool DissolveInShadows
		{
			get
			{
				this.AssertInstance();

				return GetShadowDissolve(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetShadowDissolve(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a set of 4 numbers from 0 to 1 that represent the scale of material effect layer
		/// that is applied to this entity.
		/// </summary>
		/// <remarks>Used by CryTek's games for nanosuit effects.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ColorSingle EffectLayerSingle
		{
			get
			{
				this.AssertInstance();

				return new ColorByte(GetEffectLayerParams(this.handle));
			}
			set
			{
				this.AssertInstance();

				Vector4 v = new Vector4(value.R, value.G, value.B, value.A);
				SetEffectLayerParams(this.handle, ref v);
			}
		}
		/// <summary>
		/// Gets or sets a set of 4 numbers from 0 to 255 that represent the scale of material effect layer
		/// that is applied to this entity.
		/// </summary>
		/// <remarks>Used by CryTek's games for nanosuit effects.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ColorByte EffectLayerByte
		{
			get
			{
				this.AssertInstance();

				return new ColorByte(GetEffectLayerParams(this.handle));
			}
			set
			{
				this.AssertInstance();

				SetEffectLayerParamsEnc(this.handle, value.Bytes.UnsignedInt);
			}
		}
		/// <summary>
		/// Gets or sets the number between 0 and 1 that represents opacity of this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Opacity
		{
			get
			{
				this.AssertInstance();

				return GetOpacity(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetOpacity(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the last time this entity was seen according to the system timer.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float LastSeenTime
		{
			get
			{
				this.AssertInstance();

				return GetLastSeenTime(this.handle);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether motion blur can be used when rendering this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool MotionBlur
		{
			set
			{
				this.AssertInstance();

				SetMotionBlur(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that specifies how far can this entity be from the camera relative to its size
		/// before disappearing from the view.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int ViewDistanceRatio
		{
			set
			{
				this.AssertInstance();

				SetViewDistRatio(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that specifies how far can this entity be from the camera relative to its size
		/// before switching to lower LOD model.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int LodRatio
		{
			set
			{
				this.AssertInstance();

				SetLodRatio(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal CryEntityRenderProxy(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Changes the local-space bounding box of this entity.
		/// </summary>
		/// <param name="bounds">         Reference to the bounding box to force upon this entity.</param>
		/// <param name="dontRecalculate">
		/// Indicates whether entity must be prevented from attempting to recalculate the bounding box after
		/// this change.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ChangeBounds(ref BoundingBox bounds, bool dontRecalculate = true)
		{
			this.AssertInstance();

			SetLocalBounds(this.handle, ref bounds, dontRecalculate);
		}
		/// <summary>
		/// Invalidates the bounding box of this entity, making it attempt to recalculate its bounding box.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void InvalidateBounds()
		{
			this.AssertInstance();

			InvalidateLocalBounds(this.handle);
		}
		/// <summary>
		/// Gets the material that is used to render the object in specified slot.
		/// </summary>
		/// <param name="slot">
		/// Zero-based index of the slot that contains the object which render material we need.
		/// </param>
		/// <returns>
		/// Valid object that represents the material that is used to render the object in the slot, or an
		/// invalid object, if slot is not renderable.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material GetRenderMaterial(int slot)
		{
			this.AssertInstance();

			return GetRenderMaterialInternal(this.handle, slot);
		}
		/// <summary>
		/// Gets the custom material that was assigned to the slot using <see cref="SetMaterial"/>.
		/// </summary>
		/// <param name="slot">Zero-based index of the slot to get the custom material from.</param>
		/// <returns>
		/// A valid <see cref="Material"/>, if a custom material was assigned to this slot before, otherwise
		/// an invalid object is returned.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material GetMaterial(int slot)
		{
			this.AssertInstance();

			return GetSlotMaterial(this.handle, slot);
		}
		/// <summary>
		/// Assigns a custom material to the slot.
		/// </summary>
		/// <param name="slot">    Zero-based index of the slot to assign the custom material to.</param>
		/// <param name="material">
		/// A custom material to assign to the slot. An invalid object can be used to remove the custom
		/// material to make the slot use default one instead.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetMaterial(int slot, Material material)
		{
			this.AssertInstance();

			SetSlotMaterial(this.handle, slot, material);
		}
		/// <summary>
		/// Assigns/Removes this entity to/from being rendered at post render stage.
		/// </summary>
		/// <remarks>
		/// Entities that are rendered at post render stage will look like they are in front of everything
		/// on the screen.
		/// <para>
		/// Post rendering is needed to display objects as part of the UI (e.g. animated character model on
		/// character selection screen).
		/// </para>
		/// </remarks>
		/// <param name="enable"> 
		/// Indicates whether this entity should be added to the post render stage, rather then removed from
		/// it.
		/// </param>
		/// <param name="groupId">Identifier of the group to render this entity in.</param>
		/// <param name="p1">     
		/// Normalized <see cref="Vector2"/> object that contains coordinates of one of the points of the
		/// rectangle n the screen within which this entity will be rendered.
		/// </param>
		/// <param name="p2">     
		/// Normalized <see cref="Vector2"/> object that contains coordinates of one of the points of the
		/// rectangle n the screen within which this entity will be rendered.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAsPost3DRenderObject(bool enable, byte groupId, Vector2 p1, Vector2 p2)
		{
			this.AssertInstance();

			Vector4 rect = new Vector4(p1.X, p1.Y, p2.X, p2.Y);
			SetAsPost3dRenderObjectInternal(this.handle, enable, groupId, ref rect);
		}
		/// <summary>
		/// Removes all renderable objects from this proxy.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ClearSlots()
		{
			this.AssertInstance();

			ClearSlotsInternal(this.handle);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetWorldBounds(IntPtr handle, out BoundingBox bounds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalBounds(IntPtr handle, out BoundingBox bounds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalBounds(IntPtr handle, ref BoundingBox bounds, bool bDoNotRecalculate);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InvalidateLocalBounds(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetRenderMaterialInternal(IntPtr handle, int nSlot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSlotMaterial(IntPtr handle, int nSlot, Material pMaterial);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetSlotMaterial(IntPtr handle, int nSlot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryRenderNode GetRenderNode(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMaterialLayersMask(IntPtr handle,
														 MaterialLayerActivityFlags nMtlLayersMask);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MaterialLayerActivityFlags GetMaterialLayersMask(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMaterialLayersBlend(IntPtr handle, MaterialLayerBlendFlags nMtlLayersBlend);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MaterialLayerBlendFlags GetMaterialLayersBlend(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCloakInterferenceState(IntPtr handle, bool bHasCloakInterference);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetCloakInterferenceState(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCloakHighlightStrength(IntPtr handle, float highlightStrength);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCloakColorChannel(IntPtr handle, byte nCloakColorChannel);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte GetCloakColorChannel(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCloakFadeByDistance(IntPtr handle, bool bCloakFadeByDistance);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool DoesCloakFadeByDistance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCloakBlendTimeScale(IntPtr handle, float fCloakBlendTimeScale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetCloakBlendTimeScale(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIgnoreCloakRefractionColor(IntPtr handle, bool bIgnoreCloakRefractionColor);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool DoesIgnoreCloakRefractionColor(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCustomPostEffect(IntPtr handle, string pPostEffectName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAsPost3dRenderObjectInternal(IntPtr handle, bool bPost3dRenderObject,
																   byte groupId, ref Vector4 groupScreenRect);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIgnoreHudInterferenceFilter(IntPtr handle, bool bIgnoreFiler);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetHUDRequireDepthTest(IntPtr handle, bool bRequire);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetHUDDisableBloom(IntPtr handle, bool bDisableBloom);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIgnoreHeatAmount(IntPtr handle, bool bIgnoreHeat);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetVisionParams(IntPtr handle, float r, float g, float b, float a);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetVisionParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetHUDSilhouettesParams(IntPtr handle, float r, float g, float b, float a);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetHUDSilhouettesParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetShadowDissolve(IntPtr handle, bool enable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetShadowDissolve(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEffectLayerParams(IntPtr handle, ref Vector4 pParams);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEffectLayerParamsEnc(IntPtr handle, uint nEncodedParams);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetEffectLayerParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetOpacity(IntPtr handle, float fAmount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetOpacity(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetLastSeenTime(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearSlotsInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMotionBlur(IntPtr handle, bool enable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetViewDistRatio(IntPtr handle, int nViewDistRatio);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLodRatio(IntPtr handle, int nLodRatio);
		#endregion
	}
}