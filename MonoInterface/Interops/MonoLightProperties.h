#pragma once

#include "IMonoInterface.h"

struct EnvironmentProbeProperties
{
	ITexture *DiffuseCubemap;
	ITexture *SpecularCubemap;
	Vec3 Extents;
	float AttenuationFallOffMax;
	uint8 SortPriority;
	void ToCDLight(CDLight &light) const
	{
		light.SetDiffuseCubemap(this->DiffuseCubemap);
		light.SetSpecularCubemap(this->DiffuseCubemap);
		light.m_ProbeExtents  = this->Extents;
		light.SetFalloffMax(this->AttenuationFallOffMax);
		light.m_nSortPriority = this->SortPriority;
	}
	void FromCDLight(const CDLight &light)
	{
		this->AttenuationFallOffMax = light.GetFalloffMax();
		this->DiffuseCubemap        = light.GetDiffuseCubemap();
		this->SpecularCubemap       = light.GetSpecularCubemap();
		this->Extents               = light.m_ProbeExtents;
		this->SortPriority          = light.m_nSortPriority;
	}
};
struct LightProjectionBox
{
	float Width;
	float Height;
	float Length;
	void ToCDLight(CDLight &light) const
	{
		light.m_fBoxWidth  = this->Width;
		light.m_fBoxHeight = this->Height;
		light.m_fBoxLength = this->Length;
	}
	void FromCDLight(const CDLight &light)
	{
		this->Width  = light.m_fBoxWidth;
		this->Height = light.m_fBoxHeight;
		this->Length = light.m_fBoxLength;
	}
};
struct LightProjectorProperties
{
	ITexture *AttenuationMap;
	ITexture *Image;
	Matrix34 ObjectMatrix;
	void ToCDLight(CDLight &light) const
	{
		light.m_pLightAttenMap = this->AttenuationMap;
		light.m_pLightImage    = this->Image;
		light.SetMatrix(this->ObjectMatrix, true);
	}
	void FromCDLight(const CDLight &light)
	{
		this->AttenuationMap = light.m_pLightAttenMap;
		this->Image          = light.m_pLightImage;
		this->ObjectMatrix   = light.m_ObjMatrix;
	}
};
struct AreaLightDimensions
{
	float Width;
	float Height;
	void ToCDLight(CDLight &light) const
	{
		light.m_fAreaWidth  = this->Width;
		light.m_fAreaHeight = this->Height;
	}
	void FromCDLight(const CDLight &light)
	{
		this->Width = light.m_fAreaWidth;
		this->Height = light.m_fAreaHeight;
	}
};

struct LightProperties
{
	eDynamicLightFlags Flags;
	Vec3 Origin;
	float Radius;
	ColorF Color;
	float SpecularMultiplier;
	EnvironmentProbeProperties EnvProbeProperties;
	LightProjectionBox ProjectionBox;
	float AttenuationBulbSize;
	mono::string Name;
	AreaLightDimensions AreaDimensions;

	void ToCDLight(CDLight &light) const
	{
		light.m_sName = ToNativeString(this->Name);
		light.m_Flags = this->Flags;
		light.SetPosition(this->Origin);
		light.m_fRadius = this->Radius;
		light.SetLightColor(this->Color);
		light.SetSpecularMult(this->SpecularMultiplier);
		this->EnvProbeProperties.ToCDLight(light);
		this->ProjectionBox.ToCDLight(light);
		light.m_fAttenuationBulbSize = this->AttenuationBulbSize;
		this->AreaDimensions.ToCDLight(light);
	}
	void FromCDLight(const CDLight &light)
	{
		this->Flags              = eDynamicLightFlags(light.m_Flags);
		this->Origin             = light.GetPosition();
		this->Radius             = light.m_fRadius;
		this->Color              = light.GetFinalColor(ColorF());
		this->SpecularMultiplier = light.GetSpecularMult();
		this->EnvProbeProperties.FromCDLight(light);
		this->ProjectionBox     .FromCDLight(light);
		this->AttenuationBulbSize   = light.m_fAttenuationBulbSize;
		this->AreaDimensions    .FromCDLight(light);
	}
};