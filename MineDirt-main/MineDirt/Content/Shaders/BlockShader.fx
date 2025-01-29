// Transformation matrices
float4x4 WorldViewProjection;
texture2D TextureAtlas : register(t0);
sampler2D TextureSampler = sampler_state
{
    texture = <TextureAtlas>;
};

// // Scaling factors for decompression
// float3 PositionScale;
// float2 UVScale;

// Vertex input structure
struct VertexInput
{
    //uint X : POSITION0; // Position X (byte)
    //uint Y : POSITION1; // Position Y (byte)
    //uint Z : POSITION2; // Position Z (byte)
    //uint U : TEXCOORD0; // UV U (byte)
    //uint V : TEXCOORD1; // UV V (byte)
    float3 Position : POSITION0; // Position (byte)
    float2 TexCoord : TEXCOORD0; // UV (byte)
    float Light : COLOR0; // Light value
};

// Vertex output structure
struct VertexOutput
{
    float4 Position : POSITION; // Transformed position
    float2 TexCoord : TEXCOORD0; // Passed-through texture coordinates
    float Light : COLOR0; // Light value
};

// Vertex shader
//VertexOutput VS_Main(VertexInput input)
//{
//    VertexOutput output;

//    // Scale the byte values to the proper float range (0 to 1)
//    float3 position = float3(input.X, input.Y, input.Z) / 255.0f; // Scale from 0-255 to 0.0 - 1.0
//    position -= 0.5f; // Center around 0 (optional based on your block coordinates)

//    // Apply the WorldViewProjection matrix to the position
//    output.Position = mul(float4(position, 1.0f), WorldViewProjection); // Transform position by WorldViewProjection matrix

//    // UV coordinates are scaled to range [0, 1] using the same technique
//    output.TexCoord = float2(input.U / 255.0f, input.V / 255.0f); // Scale UVs

//    return output;
//}

// Vertex shader
VertexOutput VS_Main(VertexInput input)
{
    VertexOutput output;
    
    //int X = (int) ((input.Position / (32 * 32)) % 32);
    //int Y = (int) ((input.Position / 32) % 32);
    //int Z = (int) (input.Position % 32);
    
    // Transform and decompress position
    output.Position = mul(float4(input.Position, 1.0f), WorldViewProjection);

    // Decompress UVs
    // output.TexCoord = input.TexCoord * UVScale;
    output.TexCoord = input.TexCoord;
    output.Light = input.Light;
    
    return output;
}

// Pixel shader
float4 PS_Main(VertexOutput input) : COLOR
{
    // Sample the texture
    float4 color = tex2D(TextureSampler, input.TexCoord);

    // Apply the light value to the color
    color.rgb *= input.Light; // Modulate RGB channels by the light value

    return color;
}
// Techniques
technique BasicTechnique
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VS_Main();
        PixelShader = compile ps_3_0 PS_Main();
    }
}