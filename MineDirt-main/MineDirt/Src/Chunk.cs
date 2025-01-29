using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MineDirt;
using System.Collections.Generic;
using System.Linq;
public class Chunk
{
    public static int Height { get; private set; } = 8 * Subchunk.Size; // Max chunk size
    public Vector3 Position { get; private set; }

    public long VertexCount => Subchunks.Values.ToList().Sum(subchunk => subchunk.VertexCount);
    public long TransparentVertexCount => Subchunks.Values.ToList().Sum(subchunk => subchunk.TransparentVertexCount);
    public long IndexCount => Subchunks.Values.ToList().Sum(subchunk => subchunk.IndexCount);
    public long TransparentIndexCount => Subchunks.Values.ToList().Sum(subchunk => subchunk.TransparentIndexCount);

    public bool HasUpdatedBuffers = false; 
    public bool IsUpdatingBuffers = false;
    public int UpdateCount = 0;

    // List to store all subchunks
    public Dictionary<Vector3, Subchunk> Subchunks { get; private set; }

    private IOrderedEnumerable<KeyValuePair<Vector3, Subchunk>> sortedSubchunks;
    private static readonly Vector3 subchunkCenterModifier = new(Subchunk.Size / 2, Subchunk.Size / 2, Subchunk.Size / 2);

    public Chunk(Vector3 position)
    {
        Position = position;
        Subchunks = [];

        // Generate subchunks
        GenerateSubchunkBlocks();
    }

    private void GenerateSubchunkBlocks()
    {
        int subchunkCount = Height / Subchunk.Size;

        for (int y = 0; y < subchunkCount; y++)
        {
            Vector3 subchunkPosition = new(Position.X, y * Subchunk.Size, Position.Z);

            // Create and add the subchunk
            Subchunks.Add(subchunkPosition, new Subchunk(this, subchunkPosition));
        }
    }

    public void GenerateSubchunkBuffers()
    {
        HasUpdatedBuffers = true;

        foreach (KeyValuePair<Vector3, Subchunk> subchunk in Subchunks)
        {
            subchunk.Value.GenerateBuffers();
        }

        IsUpdatingBuffers = false;
        UpdateCount++;
    }   

    public void DrawOpaque(Effect effect)
    {
        sortedSubchunks = Subchunks
            .OrderByDescending(item => Vector3.DistanceSquared(MineDirtGame.Camera.Position, item.Key + subchunkCenterModifier));

        foreach (var item in sortedSubchunks)
            item.Value.DrawOpaque(effect);
    }

    public void DrawTransparent(Effect effect)
    {
        // Subchunks are already sorted from the DrawOpaque method
        foreach (var item in sortedSubchunks)
            item.Value.DrawTransparent(effect);
    }
}
