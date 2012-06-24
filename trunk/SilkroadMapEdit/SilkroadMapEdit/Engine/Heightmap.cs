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
using System.IO;


namespace SilkroadMapEditor
{
    class Heightmap : DrawableGameComponent
    {
        SilkroadMapEditor m_game;
        VertexPositionColor[] vertices;
        int[] indicies;
        BasicEffect effect;

        public Heightmap(SilkroadMapEditor game) : base(game)
        {
            effect = new BasicEffect(game.GraphicsDevice);
            this.m_game = game;
        }
        public bool LoadTerrain(int XSector, int YSector)
        {
            string file = string.Format("nv_{0}{1}.nvm",YSector.ToString("x2"),XSector.ToString("x2"));
            vertices = nvm.GetVertices(m_game.datapk2.getFile(file));
            indicies = nvm.GetIndicies();
            return true;
        }
        public override void Draw(GameTime gameTime)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullClockwiseFace;
            rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;
            effect.View = m_game.m_camera.View;
            effect.Projection = m_game.m_camera.Projection;
            effect.World = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            effect.VertexColorEnabled = false;
            //effect.EnableDefaultLighting();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, vertices.Length,indicies,0,indicies.Length / 3);
            }
            base.Draw(gameTime);
        }
    }
}
