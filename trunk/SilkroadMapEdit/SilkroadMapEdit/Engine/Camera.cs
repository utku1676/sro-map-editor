using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SilkroadMapEditor
{
    public class Camera : DrawableGameComponent
    {
        private static Camera activeCamera = null;

        // View and projection
        private Matrix projection = Matrix.Identity;
        private Matrix view = Matrix.Identity;

        //
        private Vector3 position = new Vector3(0, 700, 0);
        private Vector3 angle = new Vector3(MathHelper.ToRadians(25.0f),MathHelper.ToRadians(120.0f), 0);
        private float speed = 500f;
        private float turnSpeed = 10f;

        public static Camera ActiveCamera
        {
            get { return activeCamera; }
            set { activeCamera = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public Camera(Game game)
            : base(game)
        {
            if (ActiveCamera == null)
                ActiveCamera = this;
        }

        public override void Initialize()
        {
            int centerX = Game.Window.ClientBounds.Width / 2;
            int centerY = Game.Window.ClientBounds.Width / 2;

            Mouse.SetPosition(centerX, centerY);
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float ratio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, ratio, 10, 10000);

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (keyboard.IsKeyDown(Keys.Space))
            {
                Game.IsMouseVisible = false;
                int centerX = Game.Window.ClientBounds.Width / 2;
                int centerY = Game.Window.ClientBounds.Width / 2;

                Mouse.SetPosition(centerX, centerY);


                angle.X += MathHelper.ToRadians((mouse.Y - centerY) * turnSpeed * 0.01f); // pitch
                angle.Y += MathHelper.ToRadians((mouse.X - centerX) * turnSpeed * 0.01f); // yaw

                angle.X = MathHelper.Clamp(angle.X, -MathHelper.ToRadians(180f), MathHelper.ToRadians(180f));
            }
            else
            {
                Game.IsMouseVisible = true;
            }
            Vector3 forward = Vector3.Normalize(new Vector3((float)Math.Sin(-angle.Y), (float)Math.Sin(angle.X), (float)Math.Cos(-angle.Y)));
            Vector3 left = Vector3.Normalize(new Vector3((float)Math.Cos(angle.Y), 0f, (float)Math.Sin(angle.Y)));

            if (keyboard.IsKeyDown(Keys.W))
                position -= forward * speed * delta;

            if (keyboard.IsKeyDown(Keys.S))
                position += forward * speed * delta;

            if (keyboard.IsKeyDown(Keys.A))
                position -= left * speed * delta;

            if (keyboard.IsKeyDown(Keys.D))
                position += left * speed * delta;

            if (keyboard.IsKeyDown(Keys.E))
                position += Vector3.Up * speed * delta;

            if (keyboard.IsKeyDown(Keys.C))
                position += Vector3.Down * speed * delta;

            if (keyboard.IsKeyDown(Keys.Escape))
                Game.Exit();

            view = Matrix.Identity;
            view *= Matrix.CreateTranslation(-position);
            view *= Matrix.CreateRotationZ(angle.Z);
            view *= Matrix.CreateRotationY(angle.Y);
            view *= Matrix.CreateRotationX(angle.X);

            base.Update(gameTime);
        }
    }
}
