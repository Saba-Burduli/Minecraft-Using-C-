#if DEBUG
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGuiNet;
using System;
using System.Diagnostics;
namespace MineDirt.Src;

public class Debug
{
    public ImGuiRenderer GuiRenderer;

    private bool hasFirstDrawFinished = false;
    
    private TimeSpan timeToShowDebugWindow = new(0, 0, 1);

    // Get the current process
    Process currentProcess = Process.GetCurrentProcess();

    // Retrieve memory usage information
    long workingSet = 0;
    long privateMemory = 0; 
    long virtualMemory = 0; 

    private float fps = 0f;
    private float ups = 0f;
    private float fpsTimer = 0f;
    private float upsTimer = 0f;
    private int frameCount = 0;
    private int updateCount = 0;

    private long vertexCount = 0; 
    private long indexCount = 0;    

    private int chunkUpdateCount = 0;
    private int chunkCount = 0;

    private Vector3 TeleportPos = new(0, 0, 0);

    private Vector3 cameraChunkPosition = Vector3.Zero;

    private float cameraSpeed = Camera3D.MovementSpeed;

    private bool RenderWireframes = false;

    private RasterizerState wireFrameRasterizeState = new()
    {
        FillMode = FillMode.WireFrame,
        CullMode = CullMode.None,
    };

    private RasterizerState defaultRasterizeState = new()
    {
        FillMode = FillMode.Solid,
        CullMode = CullMode.CullCounterClockwiseFace,
    };

    public void Initialize()
    {
        GuiRenderer = new ImGuiRenderer(MineDirtGame.Instance);
    }

    public void LoadContent()
    {
        GuiRenderer.RebuildFontAtlas();
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        cameraChunkPosition = MineDirtGame.Camera.Position.ToChunkPosition();

        // Update UPS (Updates Per Second)
        upsTimer += deltaTime;
        updateCount++;
        if (upsTimer >= 1f) // Update every second
        {
            ups = updateCount;
            upsTimer -= 1f;  // Reset the timer
            updateCount = 0;

            chunkUpdateCount = 0;
            foreach (Chunk item in World.Chunks.Values)
            {
                chunkUpdateCount += item.UpdateCount;
            }

            chunkCount = World.Chunks.Count;

            // Update the number of Vertices and Indices
            vertexCount = World.VertexCount;
            indexCount = World.IndexCount;

            currentProcess = Process.GetCurrentProcess();

            workingSet = currentProcess.WorkingSet64; // Physical memory usage in bytes
            privateMemory = currentProcess.PrivateMemorySize64; // Private memory in bytes
            virtualMemory = currentProcess.VirtualMemorySize64; // Virtual memory usage in bytes
        }
    }

    public void EndDraw(GameTime gameTime)
    {
        if(gameTime.TotalGameTime < timeToShowDebugWindow)
            return; 

        GuiRenderer.BeginLayout(gameTime);

        // Calculate FPS (Frames Per Second)
        fpsTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        frameCount++;
        if (fpsTimer >= 1f) // Update every second
        {
            fps = frameCount;
            fpsTimer -= 1f; // Reset the timer
            frameCount = 0;
        }

        // Create an ImGui window for camera coordinates
        if (ImGui.Begin("Debug", ImGuiWindowFlags.NoFocusOnAppearing))
        {
            // Create an ImGui window for camera coordinates
            if (!hasFirstDrawFinished)
            {
                ImGui.SetWindowSize(new System.Numerics.Vector2(400, 320));
            }

            // Display the pointed block 
            ImGui.Text($"Pointed Block: {MineDirtGame.Camera.PointedBlock.Type}");
            ImGui.Text($"Pointed Block Coords: {MineDirtGame.Camera.PointedBlockPosition}");
            ImGui.Text($"Pointed Block Face: {MineDirtGame.Camera.PointedBlockFace}");

            // Display the camera's position in the window
            ImGui.Text($"X: {MineDirtGame.Camera.Position.X}, Y: {MineDirtGame.Camera.Position.Y}, Z: {MineDirtGame.Camera.Position.Z}");

            // Display the camera's chunk position in the window
            ImGui.Text($"Abs Chunk X: {cameraChunkPosition.X}, Y: {cameraChunkPosition.Y}, Z: {cameraChunkPosition.Z}");

            // Display the camera's chunk normalized position in the window
            ImGui.Text($"Nor Chunk X: {cameraChunkPosition.X / Subchunk.Size}, Y: {cameraChunkPosition.Y / Subchunk.Size}, Z: {cameraChunkPosition.Z / Subchunk.Size}");

            // Display FPS and UPS
            ImGui.Text($"FPS: {fps}");
            ImGui.Text($"UPS: {ups}");

            // Display chunk count
            ImGui.Text($"Chunks Count: {chunkCount}");

            // Chunks total update count
            ImGui.Text($"Chunks Update Count: {chunkUpdateCount}");

            // Display number of Vertices and Indices
            ImGui.Text($"Vertices: {vertexCount}");
            ImGui.Text($"Indices: {indexCount}");

            // Display memory usage information in GB
            ImGui.Text($"Working Set: {(double)workingSet / 1024 / 1024 / 1024:0.###} GB");
            ImGui.Text($"Private Memory: {(double)privateMemory / 1024 / 1024 / 1024:0.###} GB");
            ImGui.Text($"Virtual Memory: {(double)virtualMemory / 1024 / 1024 / 1024:0.###} GB");

            // Checkbox 
            ImGui.Checkbox("Render Wireframes", ref RenderWireframes);

            // Add button to reload chunks
            if (ImGui.Button("Reload Chunks"))
            {
                World.ReloadChunks();
            }

            // Slider for movement speed
            ImGui.SliderFloat("Movement Speed", ref cameraSpeed, 0.1f, 50f);

            Camera3D.MovementSpeed = cameraSpeed;
        }

        ImGui.End();

        if(ImGui.Begin("Teleport"))
        {
            // Create an ImGui window for camera coordinates
            if (!hasFirstDrawFinished)
            {
                ImGui.SetWindowSize(new System.Numerics.Vector2(400, 320));
            }

            ImGui.InputFloat("X", ref TeleportPos.X);
            ImGui.InputFloat("Y", ref TeleportPos.Y);
            ImGui.InputFloat("Z", ref TeleportPos.Z);
            if (ImGui.Button("Teleport"))
            {
                MineDirtGame.Camera.Position = TeleportPos;
            }
        }

        GuiRenderer.EndLayout();

        hasFirstDrawFinished = true;
    }

    public void BeginDraw(GameTime gameTime)
    {
        if (RenderWireframes)
            MineDirtGame.Graphics.GraphicsDevice.RasterizerState = wireFrameRasterizeState;
        else
            MineDirtGame.Graphics.GraphicsDevice.RasterizerState = defaultRasterizeState;
    }
}

#endif