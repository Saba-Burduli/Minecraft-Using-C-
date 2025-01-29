using System;
using Microsoft.Xna.Framework;

namespace MineDirt.Src;

public static class Extensions
{
    public static Vector3 ToChunkPosition(this Vector3 position) =>
        new(
            (int)Math.Floor(position.X / Subchunk.Size) * Subchunk.Size,
            0,
            (int)Math.Floor(position.Z / Subchunk.Size) * Subchunk.Size
        );

    public static Vector3 ToSubchunkPosition(this Vector3 position) =>
        new(
            (int)Math.Floor(position.X / Subchunk.Size) * Subchunk.Size,
            (int)Math.Floor(position.Y / Subchunk.Size) * Subchunk.Size,
            (int)Math.Floor(position.Z / Subchunk.Size) * Subchunk.Size
        );

    public static Vector3 ToSubchunkRelativePosition(this Vector3 position) =>
        new(
            ((position.X % Subchunk.Size) + Subchunk.Size) % Subchunk.Size,
            ((position.Y % Subchunk.Size) + Subchunk.Size) % Subchunk.Size,
            ((position.Z % Subchunk.Size) + Subchunk.Size) % Subchunk.Size
        );

    public static int ToIndex(this Vector3 position) =>
        Subchunk.GetIndexFromX((int)position.X)
        + Subchunk.GetIndexFromY((int)position.Y)
        + Subchunk.GetIndexFromZ((int)position.Z);
}
