sampler inputTexture;

float4 MainPS(float2 textureCoordinates: TEXCOORD0): COLOR0
{
	float4 color = tex2D(inputTexture, textureCoordinates);
		
	// does this look weird? more on this later:
	color.rgb = color.r * 0.2126 + color.g * 0.7152 + color.b * 0.0722;
		
	return color;
}

technique Techninque1
{
	pass Pass1
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		PixelShader = compile ps_3_0 MainPS();
	}
};