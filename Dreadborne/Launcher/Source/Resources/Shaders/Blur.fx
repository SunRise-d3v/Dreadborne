Texture2D SpriteTexture : register(t0);
SamplerState SpriteTextureSampler : register(s0);

float2 Direction;

float Weights[3];
float Offsets[3];

float4 MainPS(float4 pos : SV_POSITION, float4 color : COLOR0, float2 uv : TEXCOORD0) : SV_Target
{
    float4 c = SpriteTexture.Sample(SpriteTextureSampler, uv) * Weights[0];
    c += SpriteTexture.Sample(SpriteTextureSampler, uv + Direction * Offsets[1]) * Weights[1];
    c += SpriteTexture.Sample(SpriteTextureSampler, uv - Direction * Offsets[1]) * Weights[1];
    c += SpriteTexture.Sample(SpriteTextureSampler, uv + Direction * Offsets[2]) * Weights[2];
    c += SpriteTexture.Sample(SpriteTextureSampler, uv - Direction * Offsets[2]) * Weights[2];
    return c * color;
}

technique Blur
{
    pass P0
    {
        PixelShader = compile ps_6_0 MainPS();
    }
}