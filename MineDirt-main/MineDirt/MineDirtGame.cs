using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineDirt.Src;

namespace MineDirt;

public class MineDirtGame : Game
{
#if DEBUG
    MineDirt.Src.Debug debug = new();
#endif

    public static MineDirtGame Instance;
    public static GraphicsDeviceManager Graphics;
    public static bool IsMouseCursorVisible = false;

    public static Camera3D Camera;

    public static Texture2D TextureAtlas;
    public static Texture2D Crosshair;
    public static Vector2 CrosshairPosition;

    private SpriteBatch _spriteBatch;

    public static FastNoiseLite Noise = new(1234);

    BasicEffect effect;
    Effect blockShader;

    Chunk chunk;

    bool mouseLeftWasDown = false;
    bool mouseRightWasDown = false;

    public MineDirtGame()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = IsMouseCursorVisible;

        // Uncap FPS
        Graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = false;

        // Set updates per second to 30
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 64.0);

        // Set to fullscreen
        Graphics.IsFullScreen = true;

        // Set the resolution for fullscreen mode
        Graphics.PreferredBackBufferWidth = 1920; // Set your preferred width
        Graphics.PreferredBackBufferHeight = 1080; // Set your preferred height
        Graphics.ApplyChanges();

        Instance = this;
    }

    protected override void Initialize()
    {
        Camera = new Camera3D(new Vector3(0, 100, 0), GraphicsDevice.Viewport.AspectRatio);
        World.Initialize();

        // Set noise parameters
        Noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        Noise.SetFrequency(0.02f);
        Noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        Noise.SetFractalOctaves(5);
        Noise.SetFractalLacunarity(2.0f);
        Noise.SetFractalGain(0.1f);
        Noise.SetFractalWeightedStrength(0.2f);

#if DEBUG
        debug.Initialize();
#endif
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        blockShader = Content.Load<Effect>("Shaders/BlockShader");
        TextureAtlas = Content.Load<Texture2D>("Textures/Blocks");
        Crosshair = Content.Load<Texture2D>("Textures/Crosshair");
        CrosshairPosition = new Vector2(
            GraphicsDevice.Viewport.Width / 2 - Crosshair.Width / 2,
            GraphicsDevice.Viewport.Height / 2 - Crosshair.Height / 2
        );

        // Initialize the BasicEffect
        effect = new BasicEffect(GraphicsDevice)
        {
            VertexColorEnabled = false, // Disable color shading
            TextureEnabled = true, // Enable texture mapping
            Texture = TextureAtlas, // Set the loaded texture
            View = Matrix.CreateLookAt(new Vector3(0, 0, 0), Vector3.Zero, Vector3.Up),
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                100f
            ),
        };

        blockShader
            .Parameters["WorldViewProjection"]
            .SetValue(
                Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.PiOver4,
                    GraphicsDevice.Viewport.AspectRatio,
                    0.1f,
                    100f
                )
            );
        blockShader.Parameters["TextureAtlas"].SetValue(TextureAtlas);

        // Load the block textures
        BlockRendering.Load(BlockType.Dirt, [2]);
        BlockRendering.Load(BlockType.Grass, [1, 0, 2]);
        BlockRendering.Load(BlockType.Cobblestone, [3]);
        BlockRendering.Load(BlockType.Bedrock, [4]);
        BlockRendering.Load(BlockType.Stone, [5]);
        BlockRendering.Load(BlockType.Glass, [34]);
        BlockRendering.Load(BlockType.Water, [35]);

#if DEBUG
        debug.LoadContent();
#endif
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        KeyboardState keyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();

        // Update the camera with the current input and GraphicsDevice for mouse centering
        Camera.Update(gameTime, keyboardState, mouseState, GraphicsDevice);
        IsMouseVisible = IsMouseCursorVisible;

        // Check if player is in menu mode
        if(Camera.IsMouseControlEnabled)
        {
            if(mouseState.LeftButton == ButtonState.Pressed && !mouseLeftWasDown)
            {
                mouseLeftWasDown = true; 
                if(Camera.PointedBlock.Type != BlockType.Air)
                {
                    World.BreakBlock(Camera.PointedBlockPosition);
                }
            }
            else if (mouseState.RightButton == ButtonState.Pressed && !mouseRightWasDown)
            {
                mouseRightWasDown = true;
                Block block = new(BlockType.Glass);

                World.PlaceBlock(Camera.PointedBlockPosition + Camera.PointedBlockFace, block);
            }
        }

        if(mouseState.LeftButton == ButtonState.Released)
            mouseLeftWasDown = false;

        if (mouseState.RightButton == ButtonState.Released)
            mouseRightWasDown = false;

        World.UpdateChunks();

#if DEBUG
        debug.Update(gameTime);
#endif
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
#if DEBUG
        debug.BeginDraw(gameTime);
#endif

        GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        GraphicsDevice.BlendState = BlendState.AlphaBlend;

        effect.View = Camera.View;
        effect.Projection = Camera.Projection;

        blockShader.Parameters["WorldViewProjection"].SetValue(Camera.View * Camera.Projection);

        World.DrawChunksOpaque(blockShader);
        World.DrawChunksTransparent(blockShader);

        _spriteBatch.Begin();
        _spriteBatch.Draw(Crosshair, CrosshairPosition, Color.White);
        _spriteBatch.End();

        if(Camera.PointedBlock.Type != BlockType.Air)
        {
            BoundingBox box = new(
                Camera.PointedBlockPosition,
                Camera.PointedBlockPosition + Vector3.One
            );

            Camera.DrawBoundingBox(box, GraphicsDevice, effect);
        }

        base.Draw(gameTime);

#if DEBUG
        debug.EndDraw(gameTime);
#endif
    }
}
