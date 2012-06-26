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
    class Terrain : DrawableGameComponent
    {
        SilkroadMapEditor m_game;
        VertexPositionColor[] vertices;
        int[] indicies;
        BasicEffect effect;
        List<SilkroadEntity> entities = new List<SilkroadEntity>();
        objifo objectInfo;
        public Terrain(SilkroadMapEditor game)
            : base(game)
        {
            effect = new BasicEffect(game.GraphicsDevice);
            this.m_game = game;
            this.objectInfo = new objifo(game);
        }
        public bool LoadTerrain(int XSector, int YSector)
        {

            string file = string.Format("nv_{0}{1}.nvm",YSector.ToString("x2"),XSector.ToString("x2"));
            vertices = nvm.GetVertices(m_game.datapk2.getFile(file));
            indicies = nvm.GetIndicies();
            file = string.Format(@"\{0}\{1}.m", YSector,XSector);
            m m_file = new m(m_game.mappk2.getFilebyPath(file));
            //foreach (tNVMEntity t in nvm.entities)
            //{
            //    SilkroadEntity ent = new SilkroadEntity(m_game);
            //    ent.CreateFromBSR(objectInfo.GetPathByID(t.id),new Vector3(t.x,t.y,t.z));
            //    entities.Add(ent);
            //}
            file = string.Format(@"\{0}\{1}.o2", YSector,XSector);
            o2 o2 = new o2(m_game.mappk2.getFilebyPath(file));
            foreach (o2.mapObject obj in o2.mapObjects)
            {
                SilkroadEntity ent = new SilkroadEntity(m_game);
                ent.CreateFromBSR(objectInfo.GetPathByID(obj.uID),new Vector3(obj.x,obj.y,obj.z),obj.angle);
                entities.Add(ent);
            }
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
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, vertices.Length,indicies,0,indicies.Length / 3);
            }
            DrawEntites();
            base.Draw(gameTime);
        }

        private void DrawEntites()
        {
            foreach(SilkroadEntity ent in entities)
            {
                ent.Draw();
            }
        }
    }
}
