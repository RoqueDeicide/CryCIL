﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Encapsulates information about a slot.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct EntitySlotInfo
	{
		#region Fields
		[UsedImplicitly]
		private EntitySlotFlags nFlags;
		[UsedImplicitly]
		private int nParentSlot;
		// Hide mask used by breakable object to indicate what index of the CStatObj sub-object is hidden.
		[UsedImplicitly]
		private ulong nSubObjHideMask;
		[UsedImplicitly]
		private Matrix34* pLocalTM;
		[UsedImplicitly]
		private Matrix34* pWorldTM;
		[UsedImplicitly]
		private EntityId entityId;
		[UsedImplicitly]
		private IntPtr pStatObj;
		[UsedImplicitly]
		private IntPtr pCharacter;
		[UsedImplicitly]
		private IntPtr pParticleEmitter;
		[UsedImplicitly]
		private IntPtr pLight;
		[UsedImplicitly]
		private IntPtr pChildRenderNode;
		[UsedImplicitly]
		private IntPtr pGeomCacheRenderNode;
		[UsedImplicitly]
		private Material pMaterial;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a set of flags that describe this slot.
		/// </summary>
		public EntitySlotFlags Flags
		{
			get { return this.nFlags; }
		}
		/// <summary>
		/// Gets a zero-based index of the entity slot that is set as a parent for this one. Returns -1 if
		/// no parent is set.
		/// </summary>
		public int ParentSlot
		{
			get { return this.nParentSlot; }
		}
		/// <summary>
		/// Gets a 3x4 matrix that represents local transformation of this slot.
		/// </summary>
		public Matrix34 LocalTransformationMatrix
		{
			get { return *this.pLocalTM; }
		}
		/// <summary>
		/// Gets a 3x4 matrix that represents world transformation of this slot.
		/// </summary>
		public Matrix34 WorldTransformationMatrix
		{
			get { return *this.pWorldTM; }
		}
		/// <summary>
		/// Gets identifier of the entity that is bound to this slot (Returns invalid identifier, if no
		/// entity is bound.)
		/// </summary>
		public EntityId BoundEntity
		{
			get { return this.entityId; }
		}
		/// <summary>
		/// Gets valid object that represents the particle emitter that is bound to this slot. Returned
		/// object is not valid if the emitter is not bound to this slot.
		/// </summary>
		public ParticleEmitter BoundEmitter
		{
			get { return new ParticleEmitter(this.pParticleEmitter); }
		}

		/// <summary>
		/// Gets material that is used to render this slot.
		/// </summary>
		public Material Material
		{
			get { return this.pMaterial; }
		}
		#endregion
	}
}