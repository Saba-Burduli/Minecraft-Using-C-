using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MineDirt.Src;
public struct QuantizedVertex : IVertexType
{
    Vector3 Position; 
    Vector2 UV; 
    float Light;

    public QuantizedVertex(Vector3 position, Vector2 uv, float light)
    {
        this.Position = position;
        this.UV = uv;
        this.Light = light;
    }

    public static readonly VertexDeclaration VertexDeclaration = new(
        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
        new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
        new VertexElement(20, VertexElementFormat.Single, VertexElementUsage.Color, 0)
    );

    VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
}