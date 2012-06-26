using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SilkroadMapEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SilkroadMapEditor : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        private System.Windows.Forms.Control m_drawSurfaceControl;
        public SpriteFont gameFont;
        public Drawing m_drawing;
        public Camera m_camera;
        public pk2.pk2Reader datapk2;
        public pk2.pk2Reader mediapk2;
        public pk2.pk2Reader mappk2;
        int xsec = 168, ysec = 98;
        KeyboardState lastkeyboard;
        Terrain map;

        public SilkroadMapEditor(System.Windows.Forms.Control renderControl)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_drawSurfaceControl = renderControl;
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(Game1_VisibleChanged); 
      
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = m_drawSurfaceControl.Handle;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = m_drawSurfaceControl.Width;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = m_drawSurfaceControl.Height;
        }
        private void Game1_VisibleChanged(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
            {
                System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            string silkroadPath = @"F:\Silkroad80erCap\"; //Muss noch geändert werden.
            datapk2 = new pk2.pk2Reader(silkroadPath + @"\Data.pk2");
            mediapk2 = new pk2.pk2Reader(silkroadPath + @"\Media.pk2");
            mappk2 = new pk2.pk2Reader(silkroadPath + @"\Map.pk2");
            Program.window.LoadObjectList();
            m_camera = new Camera(this);
            m_drawing = new Drawing(this);
            m_drawing.ShowFPS = true;
            map = new Terrain(this);
            this.Components.Add(m_drawing);
            this.Components.Add(map);
            this.Components.Add(m_camera);
            map.LoadTerrain(xsec, ysec);
            lastkeyboard = Keyboard.GetState();
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameFont = Content.Load<SpriteFont>("Font");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Down)&& lastkeyboard.IsKeyUp(Keys.Down))
            {
                ysec--;
                map.LoadTerrain(xsec, ysec);
            }

            // TODO: Add your update logic here
            lastkeyboard = keyboard;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
