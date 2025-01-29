using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MineDirt.Src;

public static class BlockRendering
{
    public static Dictionary<BlockType, Vector2[][]> textures = [];
    public static byte[] Indices = [0, 1, 2, 2, 1, 3];

    public static QuantizedVertex[] Vertices(Vector3 pos, BlockType blockType)
    {
        float light1 = 1f;
        float light2 = 0.9f;
        float light3 = 0.8f;
        float light4 = 0.7f;

        Vector3[] positions = [
                new Vector3(pos.X - 0f, pos.Y + 1f, pos.Z - 0f),
                new Vector3(pos.X + 1f, pos.Y + 1f, pos.Z - 0f),
                new Vector3(pos.X - 0f, pos.Y - 0f, pos.Z - 0f),
                new Vector3(pos.X + 1f, pos.Y - 0f, pos.Z - 0f),
                new Vector3(pos.X - 0f, pos.Y + 1f, pos.Z + 1f),
                new Vector3(pos.X + 1f, pos.Y + 1f, pos.Z + 1f),
                new Vector3(pos.X - 0f, pos.Y - 0f, pos.Z + 1f),
                new Vector3(pos.X + 1f, pos.Y - 0f, pos.Z + 1f)
            ];

        if(blockType == BlockType.Water)
        {
            Vector3 waterLevel = new(0, -0.1f, 0);
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] += waterLevel;
            }
        }

        if (textures[blockType].Length == 1)
            return [
            // Front face (using the side texture)
            new QuantizedVertex(positions[0], textures[blockType][0][0], light2), // top-left
            new QuantizedVertex(positions[2], textures[blockType][0][2], light2), // bottom-left
            new QuantizedVertex(positions[1], textures[blockType][0][1], light2),  // top-right
            new QuantizedVertex(positions[3], textures[blockType][0][3], light2),  // bottom-right

            // Back face (using the side texture)
            new QuantizedVertex(positions[4], textures[blockType][0][0], light1),  // top-left
            new QuantizedVertex(positions[5], textures[blockType][0][1], light1),   // top-right
            new QuantizedVertex(positions[6], textures[blockType][0][2], light1),  // bottom-left
            new QuantizedVertex(positions[7], textures[blockType][0][3], light1),   // bottom-right

            // Left face (using the side texture)
            new QuantizedVertex(positions[0], textures[blockType][0][0], light3),  // top-left
            new QuantizedVertex(positions[4], textures[blockType][0][1], light3),   // top-right
            new QuantizedVertex(positions[2], textures[blockType][0][2], light3),  // bottom-left
            new QuantizedVertex(positions[6], textures[blockType][0][3], light3),   // bottom-right

            // Right face (using the side texture)
            new QuantizedVertex(positions[1], textures[blockType][0][0], light1),  // top-left
            new QuantizedVertex(positions[3], textures[blockType][0][2], light1),  // bottom-left
            new QuantizedVertex(positions[5], textures[blockType][0][1], light1),   // top-right
            new QuantizedVertex(positions[7], textures[blockType][0][3], light1),   // bottom-right

            // Top face (using the top texture)
            new QuantizedVertex(positions[0], textures[blockType][0][0], light1),  // top-left
            new QuantizedVertex(positions[1], textures[blockType][0][1], light1),   // top-right
            new QuantizedVertex(positions[4], textures[blockType][0][2], light1),   // bottom-left
            new QuantizedVertex(positions[5], textures[blockType][0][3], light1),    // bottom-right

            // Bottom face (using the bottom texture)
            new QuantizedVertex(positions[2], textures[blockType][0][0], light4), // top-left
            new QuantizedVertex(positions[6], textures[blockType][0][2], light4),  // bottom-left
            new QuantizedVertex(positions[3], textures[blockType][0][1], light4),  // top-right
            new QuantizedVertex(positions[7], textures[blockType][0][3], light4)    // bottom-right
        ];

        if (textures[blockType].Length == 2)
            return [
            // Front face (using the side texture)
            new QuantizedVertex(positions[0], textures[blockType][0][0], light2), // top-left
            new QuantizedVertex(positions[2], textures[blockType][0][2], light2), // bottom-left
            new QuantizedVertex(positions[1], textures[blockType][0][1], light2),  // top-right
            new QuantizedVertex(positions[3], textures[blockType][0][3], light2),  // bottom-right

            // Back face (using the side texture)
            new QuantizedVertex(positions[4], textures[blockType][0][0], light1),  // top-left
            new QuantizedVertex(positions[5], textures[blockType][0][1], light1),   // top-right
            new QuantizedVertex(positions[6], textures[blockType][0][2], light1),  // bottom-left
            new QuantizedVertex(positions[7], textures[blockType][0][3], light1),   // bottom-right

            // Left face (using the side texture)
            new QuantizedVertex(positions[0], textures[blockType][0][0], light3),  // top-left
            new QuantizedVertex(positions[4], textures[blockType][0][1], light3),   // top-right
            new QuantizedVertex(positions[2], textures[blockType][0][2], light3),  // bottom-left
            new QuantizedVertex(positions[6], textures[blockType][0][3], light3),   // bottom-right

            // Right face (using the side texture)
            new QuantizedVertex(positions[1], textures[blockType][0][0], light1),  // top-left
            new QuantizedVertex(positions[3], textures[blockType][0][2], light1),  // bottom-left
            new QuantizedVertex(positions[5], textures[blockType][0][1], light1),   // top-right
            new QuantizedVertex(positions[7], textures[blockType][0][3], light1),   // bottom-right

            // Top face (using the top texture)
            new QuantizedVertex(positions[0], textures[blockType][1][0], light1),  // top-left
            new QuantizedVertex(positions[1], textures[blockType][1][1], light1),   // top-right
            new QuantizedVertex(positions[4], textures[blockType][1][2], light1),   // bottom-left
            new QuantizedVertex(positions[5], textures[blockType][1][3], light1),    // bottom-right

            // Bottom face (using the bottom texture)
            new QuantizedVertex(positions[2], textures[blockType][1][0], light4), // top-left
            new QuantizedVertex(positions[6], textures[blockType][1][2], light4),  // bottom-left
            new QuantizedVertex(positions[3], textures[blockType][1][1], light4),  // top-right
            new QuantizedVertex(positions[7], textures[blockType][1][3], light4)    // bottom-right
        ];

        if (textures[blockType].Length == 3)
            return [
            // Front face (using the side texture)
            new QuantizedVertex(positions[0], textures[blockType][0][0], light2), // top-left
            new QuantizedVertex(positions[2], textures[blockType][0][2], light2), // bottom-left
            new QuantizedVertex(positions[1], textures[blockType][0][1], light2),  // top-right
            new QuantizedVertex(positions[3], textures[blockType][0][3], light2),  // bottom-right

            // Back face (using the side texture)
            new QuantizedVertex(positions[4], textures[blockType][0][0], light1),  // top-left
            new QuantizedVertex(positions[5], textures[blockType][0][1], light1),   // top-right
            new QuantizedVertex(positions[6], textures[blockType][0][2], light1),  // bottom-left
            new QuantizedVertex(positions[7], textures[blockType][0][3], light1),   // bottom-right

            // Left face (using the side texture)
            new QuantizedVertex(positions[0], textures[blockType][0][0], light3),  // top-left
            new QuantizedVertex(positions[4], textures[blockType][0][1], light3),   // top-right
            new QuantizedVertex(positions[2], textures[blockType][0][2], light3),  // bottom-left
            new QuantizedVertex(positions[6], textures[blockType][0][3], light3),   // bottom-right

            // Right face (using the side texture)
            new QuantizedVertex(positions[1], textures[blockType][0][0], light1),  // top-left
            new QuantizedVertex(positions[3], textures[blockType][0][2], light1),  // bottom-left
            new QuantizedVertex(positions[5], textures[blockType][0][1], light1),   // top-right
            new QuantizedVertex(positions[7], textures[blockType][0][3], light1),   // bottom-right

            // Top face (using the top texture)
            new QuantizedVertex(positions[0], textures[blockType][1][0], light1),  // top-left
            new QuantizedVertex(positions[1], textures[blockType][1][1], light1),   // top-right
            new QuantizedVertex(positions[4], textures[blockType][1][2], light1),   // bottom-left
            new QuantizedVertex(positions[5], textures[blockType][1][3], light1),    // bottom-right

            // Bottom face (using the bottom texture)
            new QuantizedVertex(positions[2], textures[blockType][2][0], light4), // top-left
            new QuantizedVertex(positions[6], textures[blockType][2][2], light4),  // bottom-left
            new QuantizedVertex(positions[3], textures[blockType][2][1], light4),  // top-right
            new QuantizedVertex(positions[7], textures[blockType][2][3], light4)    // bottom-right
        ];

        return [
            // Front face (using the side texture)
            new QuantizedVertex(positions[0], textures[blockType][0][0], light2), // top-left
            new QuantizedVertex(positions[2], textures[blockType][0][2], light2), // bottom-left
            new QuantizedVertex(positions[1], textures[blockType][0][1], light2),  // top-right
            new QuantizedVertex(positions[3], textures[blockType][0][3], light2),  // bottom-right

            // Back face (using the side texture)
            new QuantizedVertex(positions[4], textures[blockType][1][0], light1),  // top-left
            new QuantizedVertex(positions[5], textures[blockType][1][1], light1),   // top-right
            new QuantizedVertex(positions[6], textures[blockType][1][2], light1),  // bottom-left
            new QuantizedVertex(positions[7], textures[blockType][1][3], light1),   // bottom-right

            // Left face (using the side texture)
            new QuantizedVertex(positions[0], textures[blockType][2][0], light3),  // top-left
            new QuantizedVertex(positions[4], textures[blockType][2][1], light3),   // top-right
            new QuantizedVertex(positions[2], textures[blockType][2][2], light3),  // bottom-left
            new QuantizedVertex(positions[6], textures[blockType][2][3], light3),   // bottom-right

            // Right face (using the side texture)
            new QuantizedVertex(positions[1], textures[blockType][3][0], light1),  // top-left
            new QuantizedVertex(positions[3], textures[blockType][3][2], light1),  // bottom-left
            new QuantizedVertex(positions[5], textures[blockType][3][1], light1),   // top-right
            new QuantizedVertex(positions[7], textures[blockType][3][3], light1),   // bottom-right

            // Top face (using the top texture)
            new QuantizedVertex(positions[0], textures[blockType][4][0], light1),  // top-left
            new QuantizedVertex(positions[1], textures[blockType][4][1], light1),   // top-right
            new QuantizedVertex(positions[4], textures[blockType][4][2], light1),   // bottom-left
            new QuantizedVertex(positions[5], textures[blockType][4][3], light1),    // bottom-right

            // Bottom face (using the bottom texture)
            new QuantizedVertex(positions[2], textures[blockType][5][0], light4), // top-left
            new QuantizedVertex(positions[6], textures[blockType][5][2], light4),  // bottom-left
            new QuantizedVertex(positions[3], textures[blockType][5][1], light4),  // top-right
            new QuantizedVertex(positions[7], textures[blockType][5][3], light4)    // bottom-right
        ];
    }

    // [0] = Front, Back, Left, Right, Top, Bottom
    // [0] = Front, Back, Left, Right, [1] = Top, Bottom
    // [0] = Front, Back, Left, Right, [1] = Top, [2] = Bottom
    // [0] = Front, [1] = Back, [2] = Left, [3] = Right, [4] = Top, [5] = Bottom]
    public static void Load(BlockType blockType, byte[] textureAtlasIndices)
    {
        if (!textures.ContainsKey(blockType))
        {
            // Get texture coordinates for each face (front, back, left, right, top, bottom)
            if (textureAtlasIndices.Length == 1)
                textures.Add(blockType,
                [
                    GetTextureCoordinates(textureAtlasIndices[0]), // Front face
                ]);

            if (textureAtlasIndices.Length == 2)
                textures.Add(blockType,
                [
                    GetTextureCoordinates(textureAtlasIndices[0]), // Front face
                    GetTextureCoordinates(textureAtlasIndices[1])  // Bottom face
                ]);

            if (textureAtlasIndices.Length == 3)
                textures.Add(blockType,
                [
                    GetTextureCoordinates(textureAtlasIndices[0]), // Front face
                    GetTextureCoordinates(textureAtlasIndices[1]), // Top face
                    GetTextureCoordinates(textureAtlasIndices[2])  // Bottom face
                ]);

            if (textureAtlasIndices.Length == 6)
                textures.Add(blockType,
                [
                    GetTextureCoordinates(textureAtlasIndices[0]), // Front face
                    GetTextureCoordinates(textureAtlasIndices[1]), // Back face
                    GetTextureCoordinates(textureAtlasIndices[2]), // Left face
                    GetTextureCoordinates(textureAtlasIndices[3]), // Right face
                    GetTextureCoordinates(textureAtlasIndices[4]), // Top face
                    GetTextureCoordinates(textureAtlasIndices[5])  // Bottom face
                ]);
        }
    }

    public static Vector2[] GetTextureCoordinates(int textureIndex, int textureWidth = 16, int textureHeight = 16, int atlasWidth = 256)
    {
        // Calculate the texture coordinates based on the index and size
        int row = textureIndex / (atlasWidth / textureWidth);
        int col = textureIndex % (atlasWidth / textureWidth);

        float uMin = col * textureWidth / (float)atlasWidth;
        float vMin = row * textureHeight / (float)atlasWidth;
        float uMax = uMin + textureWidth / (float)atlasWidth;
        float vMax = vMin + textureHeight / (float)atlasWidth;

        return
        [
            new Vector2(uMin, vMin), // Top-left
            new Vector2(uMax, vMin), // Top-right
            new Vector2(uMin, vMax), // Bottom-left
            new Vector2(uMax, vMax), // Bottom-right
        ];
    }

    public static QuantizedVertex[] GetFaceVertices(BlockType blockType, byte faceIndex, Vector3 pos)
    {
        // Each face has 4 vertices in the CustomVertex array
        // Face indices: 0 = Front, 1 = Back, 2 = Left, 3 = Right, 4 = Top, 5 = Bottom
        return Vertices(pos, blockType).Skip(faceIndex * 4).Take(4).ToArray();
    }
}
