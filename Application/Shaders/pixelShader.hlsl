Texture2D shaderTextures[4];
SamplerState SampleType;

cbuffer LightBuffer : register(b0)
{
	float4 ambientColor;
	float4 diffuseColor;
	float3 lightDirection;
	float specularPower;
	float4 specularColor;
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

float4 main(PixelInputType input) : SV_Target
{
	float4 textureColor;
	float4 bumpMap;
	float3 bumpNormal;
	float3 lightDir;
	float lightIntensity;
	float4 color;
	float3 reflection;
	float4 specular;
	float4 specularIntensity;

	// Sample the texture pixel at this location.
	textureColor = shaderTextures[0].Sample(SampleType, input.tex);

	// Sample the pixel in the bump map.
	bumpMap = shaderTextures[1].Sample(SampleType, input.tex);

	// Expand the range of the normal value from (0, +1) to (-1, +1).
	bumpMap = (bumpMap * 2.0f) - 1.0f;

	// Calculate the normal from the data in the bump map.
	bumpNormal = input.normal + bumpMap.x * input.tangent + bumpMap.y * input.binormal;

	// Normalize the resulting bump normal.
	bumpNormal = normalize(bumpNormal);

	// Invert the light direction for calculations.
	lightDir = -lightDirection;

	// Calculate the amount of the light on this pixel based on the bump normal value.
	lightIntensity = saturate(dot(bumpNormal, lightDir));

	// Determine the final diffuse color based on the diffuse color and the amount of the light intensity.
	color = saturate(diffuseColor * lightIntensity);

	// Combine the final bump light color with texture color.
	color = color * textureColor;

	if (lightIntensity > 0.0f)
	{
		specularIntensity = shaderTextures[2].Sample(SampleType, input.tex);

		float3 halfDir = normalize(lightDir + input.viewDirection);

		float specAngle = max(dot(halfDir, input.normal), 0.0);

		specular = pow(specAngle, specularPower);

		specular = specular * specularIntensity;

		// Add the specular component last to the output color.
		color = saturate(color + specular);
	}
	return color;
}