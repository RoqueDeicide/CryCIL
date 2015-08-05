using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using CryCil.Engine.Rendering;
using CryCil.Engine.StaticObjects;

namespace CryCil.Engine.Logic
{
	internal static class EntitySlotOps
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsSlotValid(IntPtr entityHandle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void FreeSlot(IntPtr entityHandle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetSlotInfo(IntPtr entityHandle, int nIndex, out EntitySlotInfo slotInfo);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetSlotWorldTM(IntPtr entityHandle, int slot, out Matrix34 matrix);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetSlotLocalTM(IntPtr entityHandle, int slot, bool bRelativeToParent, out Matrix34 matrix);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSlotLocalTM(IntPtr entityHandle, int slot, ref Matrix34 localTM);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSlotCameraSpacePos(IntPtr entityHandle, int slot, ref Vector3 cameraSpacePos);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetSlotCameraSpacePos(IntPtr entityHandle, int slot, out Vector3 cameraSpacePos);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SetParentSlot(IntPtr entityHandle, int nParentIndex, int nChildIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSlotMaterial(IntPtr entityHandle, int slot, Material pMaterial);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSlotFlags(IntPtr entityHandle, int slot, EntitySlotFlags nFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern EntitySlotFlags GetSlotFlags(IntPtr entityHandle, int slot);


		// Description:
		//     Returns true if character is to be updated.
		// Arguments:
		//     slot - Index of the slot.
		// Return Value:
		//     Returns true if character is to be updated.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool ShouldUpdateCharacter(IntPtr entityHandle, int slot);

		// Description:
		//     Fast method to get the character at the specified slot.
		// Arguments:
		//     slot - Index of the slot.
		// Return Value:
		//     Character pointer or NULL if character with this slot does not exist.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetCharacter(IntPtr entityHandle, int slot);

		// Description:
		//     Sets character instance of a slot, and creates slot if necessary.
		// Arguments:
		//     slot		- Index of a slot, or -1 if a new slot need to be allocated.
		//	   pCharacter	- A pointer to character instance.
		// Return Value:
		//    An integer which refers to the slot index which used.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int SetCharacter(IntPtr entityHandle, IntPtr pCharacter, int slot);

		// Description:
		//     Fast method to get the static object at the specified slot.
		// Arguments:
		//     slot - Index of the slot; | with ENTITY_SLOT_ACTUAL to disable compound statobj handling.
		// Return Value:
		//     StatObj pointer or NULL if stat object with this slot does not exist.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetStatObj(IntPtr entityHandle, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ParticleEmitter GetParticleEmitter(IntPtr entityHandle, int slot);

		// Description:
		//     Fast method to get the geom cache render cache at the specified slot.
		// Arguments:
		//     slot - Index of the slot.
		// Return Value:
		//     IGeomCacheRenderNode pointer or NULL if stat object with this slot does not exist.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetGeomCacheRenderNode(IntPtr entityHandle, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void MoveSlot(IntPtr entityHandle, CryEntity targetIEnt, int slot);

		// Description:
		//     Sets static object of a slot, creates slot if necessary
		// Arguments:
		//     slot - Index of a slot, or -1 if a new slot need to be allocated; 
		//						 | with ENTITY_SLOT_ACTUAL to disable compound statobj handling
		//		 pStatObj - pointer to the new static object
		//     mass - new mass of the slot, negative value to keep the current
		// Return:
		//    An integer which refer to the slot index which used
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int SetStatObj(IntPtr entityHandle, IntPtr pStatObj, int slot, bool bUpdatePhysics, float mass = -1.0f);

		// Description:
		//     Loads static geometry to the specified slot, or to next available slot.
		//     If same object is already loaded in this slot, operation is ignored.
		//     If this slot number is occupied by different kind of object it is overwritten with static object.
		//     nLoadFlags - @see ELoadFlags
		// Return:
		//     Slot id where the object was loaded, or -1 if loading failed.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int LoadGeometry(IntPtr entityHandle, int slot, string sFilename, string sGeomName = null, int nLoadFlags = 0);

		// Description:
		//     Loads character to the specified slot, or to next available slot.
		//     If same character is already loaded in this slot, operation is ignored.
		//     If this slot number is occupied by different kind of object it is overwritten.
		// Return:
		//     Slot id where the object was loaded, or -1 if loading failed.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int LoadCharacter(IntPtr entityHandle, int slot, string sFilename, int nLoadFlags = 0);

		// Description:
		//     Loads geometry cache to the specified slot, or to next available slot.
		//     If same geometry cache is already loaded in this slot, operation is ignored.
		//     If this slot number is occupied by different kind of object it is overwritten.
		// Return:
		//     Slot id where the object was loaded, or -1 if loading failed.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int LoadGeomCache(IntPtr entityHandle, int slot, string sFilename);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int LoadParticleEmitterDefault(IntPtr entityHandle, int slot, ParticleEffect pEffect, bool bPrime = false, bool bSerialize = false);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int LoadParticleEmitter(IntPtr entityHandle, int slot, ParticleEffect pEffect, ref ParticleSpawnParameters parameters, bool bPrime = false, bool bSerialize = false);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int SetParticleEmitter(IntPtr entityHandle, int slot, ParticleEmitter pEmitter, bool bSerialize = false);

		// Description:
		//     Loads a light source to the specified slot, or to next available slot.
		// Return:
		//     Slot id where the light source was loaded, or -1 if loading failed.
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int LoadLight(IntPtr entityHandle, int slot, IntPtr pLight);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSlotCount(IntPtr entityHandle);
	}
}
