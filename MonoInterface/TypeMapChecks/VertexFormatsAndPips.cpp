#include "stdafx.h"

#include "CheckingBasics.h"

struct VF_P3F_C4B_T2F
{
	Vec3 xyz;
	UCol color;
	Vec2 st;

	explicit VF_P3F_C4B_T2F(SVF_P3F_C4B_T2F &other)
	{
		CHECK_TYPE_SIZE(VF_P3F_C4B_T2F);

		ASSIGN_FIELD(xyz);
		ASSIGN_FIELD(color);
		ASSIGN_FIELD(st);

		CHECK_TYPE(xyz);
		CHECK_TYPE(color);
		CHECK_TYPE(st);
	}
};
struct VF_TP3F_C4B_T2F
{
	Vec4 pos;
	UCol color;
	Vec2 st;

	explicit VF_TP3F_C4B_T2F(SVF_TP3F_C4B_T2F &other)
	{
		CHECK_TYPE_SIZE(VF_TP3F_C4B_T2F);

		ASSIGN_FIELD(pos);
		ASSIGN_FIELD(color);
		ASSIGN_FIELD(st);

		CHECK_TYPE(pos);
		CHECK_TYPE(color);
		CHECK_TYPE(st);
	}
};
struct VF_P3S_C4B_T2S
{
	Vec3f16 xyz;
	UCol color;
	Vec2f16 st;

	explicit VF_P3S_C4B_T2S(SVF_P3S_C4B_T2S &other)
	{
		CHECK_TYPE_SIZE(VF_P3S_C4B_T2S);

		ASSIGN_FIELD(xyz);
		ASSIGN_FIELD(color);
		ASSIGN_FIELD(st);

		CHECK_TYPE(xyz);
		CHECK_TYPE(color);
		CHECK_TYPE(st);
	}
};

struct VF_P3F_C4B_T2S
{
	Vec3 xyz;
	UCol color;
	Vec2f16 st;

	explicit VF_P3F_C4B_T2S(SVF_P3F_C4B_T2S &other)
	{
		CHECK_TYPE_SIZE(VF_P3F_C4B_T2S);

		ASSIGN_FIELD(xyz);
		ASSIGN_FIELD(color);
		ASSIGN_FIELD(st);

		CHECK_TYPE(xyz);
		CHECK_TYPE(color);
		CHECK_TYPE(st);
	}
};

struct VF_P3S_N4B_C4B_T2S
{
	Vec3f16 xyz;
	UCol normal;
	UCol color;
	Vec2f16 st;

	explicit VF_P3S_N4B_C4B_T2S(SVF_P3S_N4B_C4B_T2S &other)
	{
		CHECK_TYPE_SIZE(VF_P3S_N4B_C4B_T2S);

		ASSIGN_FIELD(xyz);
		ASSIGN_FIELD(normal);
		ASSIGN_FIELD(color);
		ASSIGN_FIELD(st);

		CHECK_TYPE(xyz);
		CHECK_TYPE(normal);
		CHECK_TYPE(color);
		CHECK_TYPE(st);
	}
};

struct VF_P2S_N4B_C4B_T1F
{
	CryHalf2 xy;
	UCol normal;
	UCol color;
	float z;

	explicit VF_P2S_N4B_C4B_T1F(SVF_P2S_N4B_C4B_T1F &other)
	{
		CHECK_TYPE_SIZE(VF_P2S_N4B_C4B_T1F);

		ASSIGN_FIELD(xy);
		ASSIGN_FIELD(normal);
		ASSIGN_FIELD(color);
		ASSIGN_FIELD(z);

		CHECK_TYPE(xy);
		CHECK_TYPE(normal);
		CHECK_TYPE(color);
		CHECK_TYPE(z);
	}
};

struct VF_T2F
{
	Vec2 st;

	explicit VF_T2F(SVF_T2F &other)
	{
		CHECK_TYPE_SIZE(VF_T2F);

		ASSIGN_FIELD(st);

		CHECK_TYPE(st);
	}
};
struct VF_W4B_I4S
{
	UCol weights;
	uint16 indices[4];

	explicit VF_W4B_I4S(SVF_W4B_I4S &other)
	{
		CHECK_TYPE_SIZE(VF_W4B_I4S);

		ASSIGN_FIELD(weights);
		ASSIGN_FIELD(indices[0]);

		CHECK_TYPE(weights);
		CHECK_TYPE(indices);
	}
};
struct VF_C4B_C4B
{
	UCol coef0;
	UCol coef1;

	explicit VF_C4B_C4B(SVF_C4B_C4B &other)
	{
		CHECK_TYPE_SIZE(VF_C4B_C4B);

		ASSIGN_FIELD(coef0);
		ASSIGN_FIELD(coef1);

		CHECK_TYPE(coef0);
		CHECK_TYPE(coef1);
	}
};
struct VF_P3F_P3F_I4B
{
	Vec3 thin;
	Vec3 fat;
	UCol index;

	explicit VF_P3F_P3F_I4B(SVF_P3F_P3F_I4B &other)
	{
		CHECK_TYPE_SIZE(VF_P3F_P3F_I4B);

		ASSIGN_FIELD(thin);
		ASSIGN_FIELD(fat);
		ASSIGN_FIELD(index);

		CHECK_TYPE(thin);
		CHECK_TYPE(fat);
		CHECK_TYPE(index);
	}
};
struct VF_P3F
{
	Vec3 xyz;

	explicit VF_P3F(SVF_P3F &other)
	{
		CHECK_TYPE_SIZE(VF_P3F);

		ASSIGN_FIELD(xyz);

		CHECK_TYPE(xyz);
	}
};
struct VF_P3F_T3F
{
	Vec3 p;
	Vec3 st;

	explicit VF_P3F_T3F(SVF_P3F_T3F &other)
	{
		CHECK_TYPE_SIZE(VF_P3F_T3F);

		ASSIGN_FIELD(p);
		ASSIGN_FIELD(st);

		CHECK_TYPE(p);
		CHECK_TYPE(st);
	}
};
struct VF_P3F_T2F_T3F
{
	Vec3 p;
	Vec2 st0;
	Vec3 st1;

	explicit VF_P3F_T2F_T3F(SVF_P3F_T2F_T3F &other)
	{
		CHECK_TYPE_SIZE(VF_P3F_T2F_T3F);

		ASSIGN_FIELD(p);
		ASSIGN_FIELD(st0);
		ASSIGN_FIELD(st1);

		CHECK_TYPE(p);
		CHECK_TYPE(st0);
		CHECK_TYPE(st1);
	}
};
struct VF_TP3F_T2F_T3F
{
	Vec4 p;
	Vec2 st0;
	Vec3 st1;

	explicit VF_TP3F_T2F_T3F(SVF_TP3F_T2F_T3F &other)
	{
		CHECK_TYPE_SIZE(VF_TP3F_T2F_T3F);

		ASSIGN_FIELD(p);
		ASSIGN_FIELD(st0);
		ASSIGN_FIELD(st1);

		CHECK_TYPE(p);
		CHECK_TYPE(st0);
		CHECK_TYPE(st1);
	}
};
struct VF_P2F_T4F_C4F
{
	Vec2 p;
	Vec4 st;
	Vec4 color;

	explicit VF_P2F_T4F_C4F(SVF_P2F_T4F_C4F &other)
	{
		CHECK_TYPE_SIZE(VF_P2F_T4F_C4F);

		ASSIGN_FIELD(p);
		ASSIGN_FIELD(color);
		ASSIGN_FIELD(st);

		CHECK_TYPE(p);
		CHECK_TYPE(color);
		CHECK_TYPE(st);
	}
};

struct VF_P3F_C4B_T4B_N3F2
{
	Vec3 xyz;
	UCol color;
	UCol st;
	Vec3 xaxis;
	Vec3 yaxis;

	explicit VF_P3F_C4B_T4B_N3F2(SVF_P3F_C4B_T4B_N3F2 &other)
	{
		CHECK_TYPE_SIZE(VF_P3F_C4B_T4B_N3F2);

		ASSIGN_FIELD(xyz);
		ASSIGN_FIELD(color);
		ASSIGN_FIELD(st);
		ASSIGN_FIELD(xaxis);
		ASSIGN_FIELD(yaxis);

		CHECK_TYPE(xyz);
		CHECK_TYPE(color);
		CHECK_TYPE(st);
		CHECK_TYPE(xaxis);
		CHECK_TYPE(yaxis);
	}
};

struct VF_C4B_T2S
{
	UCol color;
	Vec2f16 st;

	explicit VF_C4B_T2S(SVF_C4B_T2S &other)
	{
		CHECK_TYPE_SIZE(VF_C4B_T2S);

		ASSIGN_FIELD(color);
		ASSIGN_FIELD(st);

		CHECK_TYPE(color);
		CHECK_TYPE(st);
	}
};

struct PipTangents
{
	Vec4sf Tangent;
	Vec4sf Bitangent;

	explicit PipTangents(SPipTangents &other)
	{
		CHECK_TYPE_SIZE(PipTangents);

		ASSIGN_FIELD(Tangent);
		ASSIGN_FIELD(Bitangent);

		CHECK_TYPE(Tangent);
		CHECK_TYPE(Bitangent);
	}
};

struct PipQTangents
{
	Vec4sf QTangent;

	explicit PipQTangents(SPipQTangents &other)
	{
		CHECK_TYPE_SIZE(PipQTangents);

		ASSIGN_FIELD(QTangent);

		CHECK_TYPE(QTangent);
	}
};

struct PipNormal
{
	float x;
	float y;
	float z;

	explicit PipNormal(SPipNormal &other)
	{
		CHECK_TYPE_SIZE(PipNormal);

		ASSIGN_FIELD(x);
		ASSIGN_FIELD(y);
		ASSIGN_FIELD(z);

		CHECK_TYPE(x);
		CHECK_TYPE(y);
		CHECK_TYPE(z);
	}
};