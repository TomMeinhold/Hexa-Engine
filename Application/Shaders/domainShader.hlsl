Texture2D shaderTextures[4];
SamplerState SampleType;

cbuffer MatrixBuffer : register(b0)
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projectionMatrix;
	float4 cameraPosition;
};

cbuffer ModelConstantBuffer : register(b1)
{
	matrix model;
};

//////////////
// TYPEDEFS //
//////////////
struct ConstantOutputType
{
	float edges[3] : SV_TessFactor;
	float inside : SV_InsideTessFactor;
};

struct HullOutputType
{
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
	float3 viewDirection : TEXCOORD1;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
	float3 viewDirection : TEXCOORD1;
};

////////////////////////////////////////////////////////////////////////////////
// Domain Shader
////////////////////////////////////////////////////////////////////////////////
[domain("tri")]

PixelInputType ColorDomainShader(ConstantOutputType input, float3 uvwCoord : SV_DomainLocation, const OutputPatch<HullOutputType, 3> patch)
{
	PixelInputType output;
	float4 worldPosition;
	float displacement;
	float4 displacementPosition;

	// Determine the position of the new vertex.
	output.position = uvwCoord.x * patch[0].position + uvwCoord.y * patch[1].position + uvwCoord.z * patch[2].position;
	output.tex = uvwCoord.x * patch[0].tex + uvwCoord.y * patch[1].tex + uvwCoord.z * patch[2].tex;
	output.normal = uvwCoord.x * patch[0].normal + uvwCoord.y * patch[1].normal + uvwCoord.z * patch[2].normal;
	output.tangent = uvwCoord.x * patch[0].tangent + uvwCoord.y * patch[1].tangent + uvwCoord.z * patch[2].tangent;
	output.binormal = uvwCoord.x * patch[0].binormal + uvwCoord.y * patch[1].binormal + uvwCoord.z * patch[2].binormal;
	output.viewDirection = uvwCoord.x * patch[0].viewDirection + uvwCoord.y * patch[1].viewDirection + uvwCoord.z * patch[2].viewDirection;

	output.normal = mul(output.normal, (float3x3)transpose(model));
	output.normal = mul(output.normal, (float3x3)transpose(worldMatrix));
	output.normal = normalize(output.normal);

	displacement = shaderTextures[3].SampleLevel(SampleType, output.tex, 0).r;
	displacementPosition = float4(((0.05f * displacement) * output.normal), 0);

	output.position = mul(output.position, transpose(model));
	output.position = mul(output.position, transpose(worldMatrix));
	output.position += displacementPosition;
	output.position = mul(output.position, transpose(viewMatrix));
	output.position = mul(output.position, transpose(projectionMatrix));

	worldPosition = mul(output.position, transpose(model));
	worldPosition = mul(output.position, transpose(worldMatrix));
	output.viewDirection = normalize(cameraPosition.xyz - worldPosition.xyz);

	output.tangent = mul(output.tangent, (float3x3)transpose(model));
	output.tangent = mul(output.tangent, (float3x3)transpose(worldMatrix));
	output.tangent = normalize(output.tangent);

	output.binormal = mul(output.binormal, (float3x3)transpose(model));
	output.binormal = mul(output.binormal, (float3x3)transpose(worldMatrix));
	output.binormal = normalize(output.binormal);

	return output;
}