namespace MineDirt.Src;
public enum BlockType : byte
{
    Air,
    Dirt,
    Grass,
    Cobblestone,
    Bedrock,
    Stone,
    Glass,
    Water
}

public struct Block
{
    public BlockType Type;

    // 0b00000000
    // IsBitmaskBuilt, Opacity, Front, Back, Left, Right, Top, Bottom
    public byte AdjacentFacesVisibility;

    private const byte OpacityMask = 0b01000000;
    private const byte BitmaskBuiltMask = 0b10000000;

    public enum AdjentFaceMask : byte
    {
        Front = 0b00000001,
        Back = 0b00000010,
        Left = 0b00000100,
        Right = 0b00001000,
        Top = 0b00010000,
        Bottom = 0b00100000,
    }

    public static readonly short[] Faces =
    [
        -1, // Front
        1, // Back
        (short)(-1 * Subchunk.Size * Subchunk.Size), // Left
        (short)(1 * Subchunk.Size * Subchunk.Size), // Right
        (short)(1 * Subchunk.Size), // Top
        (short)(-1 * Subchunk.Size), // Bottom
    ];

    public Block(BlockType type = BlockType.Air)
    {
        Type = type;
        AdjacentFacesVisibility = 0b01000000;

        if (Type == BlockType.Air)
            AdjacentFacesVisibility = 0;

        if(Type == BlockType.Glass)
            AdjacentFacesVisibility = 0;

        if (Type == BlockType.Water)
            AdjacentFacesVisibility = 0;
    }

    public readonly bool IsOpaque => (AdjacentFacesVisibility & OpacityMask) != 0;

    public readonly bool IsBitmaskBuilt => (AdjacentFacesVisibility & BitmaskBuiltMask) != 0;

    public readonly bool GetAdjacentFaceVisibility(AdjentFaceMask face) =>
        (AdjacentFacesVisibility & (byte)face) != 0;

    public void SetBlockOpacity(bool isOpaque)
    {
        if (isOpaque)
            AdjacentFacesVisibility |= OpacityMask;
        else
            AdjacentFacesVisibility &= ~OpacityMask & 0xFF;
    }

    public void SetIsBitmaskBuilt(bool isBitmaskBuilt)
    {
        if (isBitmaskBuilt)
            AdjacentFacesVisibility |= BitmaskBuiltMask;
        else
            AdjacentFacesVisibility &= ~BitmaskBuiltMask & 0xFF;
    }

    public void SetAdjacentFaceVisibility(AdjentFaceMask face, bool isVisible)
    {
        byte faceMask = (byte)face;

        if (isVisible)
            AdjacentFacesVisibility |= faceMask;
        else
            AdjacentFacesVisibility &= (byte)((~faceMask) & 0xFF);
    }
}
