//desaturated.fx
//Written by Kirk Roerig
//to be used to create a depressing feel
sampler samplerState;

float light;

struct PS_INPUT
{
float2 TexCoord : TEXCOORD0;
};
float4 Pause(PS_INPUT Input) : COLOR0
{
	float4 col = 0;

	col = tex2D(samplerState, Input.TexCoord);

col -=light;

return col;
}
technique PauseTechnique
{
pass P0
{
PixelShader = compile ps_2_0 Pause();
}
}