using System;
using System.Runtime.CompilerServices;
using CryCil.Engine.Logic;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Rendering;
using CryCil.Engine.Rendering.Lighting;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Attachments
{
	internal static class AttachedObjectsCommons
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ProcessAttachment(IntPtr handle, AttachmentSocket pIAttachment);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern BoundingBox GetAabb(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetRadiusSqr(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern StaticObject GetIStatObj(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetIStatObj(IntPtr handle, StaticObject staticObject);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Character GetICharacterInstance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetICharacterInstance(IntPtr handle, Character character);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentSkin GetIAttachmentSkin(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetIAttachmentSkin(IntPtr handle, AttachmentSkin skin);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetObjectFilePath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Material GetBaseMaterial(IntPtr handle, uint nLod);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetReplacementMaterial(IntPtr handle, Material pMaterial, uint nLod);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Material GetReplacementMaterial(IntPtr handle, uint nLod);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetEntityId(IntPtr handle, EntityId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern EntityId GetEntityId(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void LoadLight(IntPtr handle, ref LightProperties light);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern LightSource GetLightSource(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ParticleEmitter GetEmitter(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr LoadEffectAttachment(string effectName, ref Vector3 offset, ref Vector3 dir, float scale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreateEffectAttachment(ParticleEffect pParticleEffect, ref Vector3 offset, ref Vector3 dir, float scale);
	}
}