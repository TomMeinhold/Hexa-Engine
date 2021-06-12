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

struct VertexInputType
{
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
};

struct HullInputType
{
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float3 tangent : TANGENT;
	float3 binormal : BINORMAL;
	float3 viewDirection : TEXCOORD1;
};

HullInputType main(VertexInputType input)
{
	HullInputType output;
	float4 worldPosition;

	output.position = input.position;
	output.tex = input.tex;
	output.normal = input.normal;
	output.tangent = input.tangent;
	output.binormal = input.binormal;

	worldPosition = mul(input.position, transpose(model));
	worldPosition = mul(output.position, transpose(worldMatrix));
	output.viewDirection = normalize(cameraPosition.xyz - worldPosition.xyz);
	return output;
}