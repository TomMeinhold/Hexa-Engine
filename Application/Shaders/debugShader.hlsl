cbuffer MatrixBuffer : register(b0)
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projectionMatrix;
	float4 cameraPosition;
};

struct VertexInput
{
	float4 Position : POSITION;
	float4 Color : COLOR;
};
struct VertexOutput
{
	float4 Position : SV_POSITION;
	float4 Color : TEXCOORD0;
};

VertexOutput RenderSceneVS(VertexInput input)
{
	VertexOutput output;

	output.Position = mul(input.Position, transpose(worldMatrix));
	output.Position = mul(output.Position, transpose(viewMatrix));
	output.Position = mul(output.Position, transpose(projectionMatrix));

	output.Color = input.Color;
	return output;
}

float4 RenderScenePS(VertexOutput input) : SV_TARGET
{
	return input.Color;
}