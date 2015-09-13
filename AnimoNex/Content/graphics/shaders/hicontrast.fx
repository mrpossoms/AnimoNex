
//desaturated.fx
//Written by Kirk Roerig
//to be used to create a depressing feel
sampler samplerState;

struct PS_INPUT
{
float2 TexCoord : TEXCOORD0;
};
float4 Pause(PS_INPUT Input) : COLOR0
{
	float4 col = 0;

	col = tex2D(samplerState, Input.TexCoord);
for(int i =0; i!= 2; i++)

float a = (col.r + col.g + col.b) / 3;

//a /= 12.0f;
	col.rgb = (col.rgb + (a))/2.0f;
	col.r +=(col.r-0.5f)/4.0f;
	col.b +=(col.b-0.5f)/4.0f;
	col.g +=(col.g-0.5f)/4.0f;

return col;
}
technique PauseTechnique
{
pass P0
{
PixelShader = compile ps_2_0 Pause();
}
}