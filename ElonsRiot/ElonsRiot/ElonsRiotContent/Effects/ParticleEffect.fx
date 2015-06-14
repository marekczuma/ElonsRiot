float4x4 View;
float4x4 Projection;
float2 ViewportScale;

float CurrentTime;

float Duration;
float DurationRandomness;
float3 Gravity;
float EndVelocity;
float4 MinColor;
float4 MaxColor;

float2 RotateSpeed;
float2 StartSize;
float2 EndSize;

texture Texture;

sampler Sampler = sampler_state
{
	Texture = (Texture);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Point;

	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float2 Corner : POSITION0;
	float3 Position : POSITION1;
	float3 Velocity : NORMAL0;
	float4 Random : COLOR0;
	float Time : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 TextureCoordinate : COLOR1;
};

float4 ComputeParticlePosition(float3 position, float3 velocity, float age, float normalizedAge)
{
	float startVelocity = length(velocity);
	float endVelocity = startVelocity * EndVelocity;

	float velocityIntegral = startVelocity * normalizedAge + (endVelocity - 
		startVelocity) * normalizedAge * normalizedAge / 2;

	position += normalize(velocity) * velocityIntegral * Duration;
	position += Gravity * age * normalizedAge;

	return mul(mul(float4(position, 1), View), Projection);
}

float ComputeParticleSize(float randomValue, float normalizedAge)
{
	float startSize = lerp(StartSize.x, StartSize.y, randomValue);
	float endSize = lerp(EndSize.x, EndSize.y, randomValue);
	float size = lerp(startSize, endSize, normalizedAge);

	return size * Projection._m11;
}

float4 ComputeParticleColor(float4 projectedPosition, float randomValue, float normalizedAge)
{
	float4 color = lerp(MinColor, MaxColor, randomValue);
	//color.a *= normalizedAge * (1 - normalizedAge) * (1 - normalizedAge) * 6.7;

	return color;
}

float2x2 ComputeParticleRotation(float randomValue, float age)
{
	float rotateSpeed = lerp(RotateSpeed.x, RotateSpeed.y, randomValue);
	float rotation = rotateSpeed * age;

	float c = cos(rotation);
	float s = sin(rotation);

	return float2x2(c, -s, s, c);
}

VertexShaderOutput ParticleVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output;

	float age = CurrentTime - input.Time;
	age *= 1 + input.Random.x * DurationRandomness;

	float normalizedAge = saturate(age / Duration);

	output.Position = ComputeParticlePosition(input.Position, input.Velocity, age, normalizedAge);

	float size = ComputeParticleSize(input.Random.y, normalizedAge);
	float2x2 rotation = ComputeParticleRotation(input.Random.w, age);

	output.Position.xy += mul(input.Corner, rotation) * size * ViewportScale;

	output.Color = ComputeParticleColor(output.Position, input.Random.z, normalizedAge);
	output.TextureCoordinate = (input.Corner + 1) / 2;

	return output;
}

float4 ParticlePixelShader(VertexShaderOutput input) : COLOR0
{
	return tex2D(Sampler, input.TextureCoordinate) * input.Color;
}

technique Particles
{
	pass P0
	{
		VertexShader = compile vs_2_0 ParticleVertexShader();
		PixelShader = compile ps_2_0 ParticlePixelShader();
	}
}