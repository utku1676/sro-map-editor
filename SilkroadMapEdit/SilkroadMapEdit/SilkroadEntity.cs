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
    public class SilkroadEntity
    {
        public class silkModel
        {
            public List<Mesh> meshs;
            public float roation;
        }
        public class Mesh
        {
            public VertexPositionNormalTexture[] verticies;
            public int[] indcicies;
            public Texture2D texture;
            public string name;
        }
        private SilkroadMapEditor m_game;
        bsr m_bsr;
        public silkModel model;
        Texture2D errorTexture;
        BasicEffect effect;
        Vector3 m_position;
        public SilkroadEntity(SilkroadMapEditor game)
        {
            m_game = game;
            model = new silkModel();
            errorTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.Gray;
            errorTexture.SetData<Color>(data);
            effect = new BasicEffect(m_game.GraphicsDevice);
        }
        public void CreateFromBSR(string path, Vector3 position, float rotation = 0.0f)
        {
            m_position = position;
            model.roation = rotation;
            m_bsr = new bsr(m_game.datapk2.getFilebyPath(path));
            bms[] models = new bms[m_bsr.GetBMSPaths().Count];
            List<string> paths = m_bsr.GetBMSPaths();
            for (int i = 0; i < models.Length; i++)
            {
                models[i] = new bms(paths[i],m_game.datapk2);
            }
            paths = m_bsr.GetBMTPaths();
            bmt[] materials = new bmt[m_bsr.GetBMTPaths().Count];

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = new bmt(paths[i],m_game.datapk2);
            }
            List<string> textures = new List<string>();
            foreach (bmt mat in materials)
            {
                List<string> tmp = mat.GetTextureList();
                textures.AddRange(tmp);
            }

            List<SilkroadTexture> textList = new List<SilkroadTexture>();
            Texture2D[] texs = new Texture2D[textures.Count];
            string ddspath = paths[0].Substring(0, paths[0].LastIndexOf('\\'));
            for (int i = 0; i < textures.Count; i++)
            {
                //textures[i] = textures[i].Replace(".ddj", ".dds");
                if (textures[i] != "")
                {
                    string tmppath = ddspath + @"\" + textures[i];
                    texs[i] = ddjLoader.GetTexture(m_game.datapk2.getFilebyPath(tmppath), m_game.GraphicsDevice);
                    SilkroadTexture tmp = new SilkroadTexture();
                    tmp.texture = texs[i];
                    tmp.name = textures[i];
                    textList.Add(tmp);
                }
            }

            //model.texture = DDSLib.GetTexture(File.ReadAllBytes(@"F:\EliteSRO\PK2Tools_0_1\Data\" + path), GraphicsDevice);
            SetupVerticies(models);
            ApplyTextures(model, textList);
        }
        private void SetupVerticies(bms[] models)
        {
            model.meshs = new List<Mesh>();
            foreach (bms bm in models)
            {
                Mesh mesh = new Mesh();
                mesh.verticies = bm.GetVerticies();
                mesh.indcicies = bm.GetIndicies();
                mesh.name = bm.mesh;
                model.meshs.Add(mesh);
            }

        }
        private void ApplyTextures(silkModel model, List<SilkroadTexture> textures)
        {

            for (int i = 0; i < model.meshs.Count; i++)
            {
                //SilkroadTexture tmp = textures.Find(t => t.name.StartsWith(model.meshs[i].name.ToLower()) == true);
                //if (tmp != null)
                //{
                //    model.meshs[i].texture = tmp.texture;
                //}
                //else
                //{
                    model.meshs[i].texture = errorTexture;
                //}
            }
        }

        public void Draw()
        {
            foreach (SilkroadEntity.Mesh mesh in model.meshs)
            {
                effect.View = m_game.m_camera.View;
                effect.Projection = m_game.m_camera.Projection;
                effect.World = Matrix.CreateRotationY(MathHelper.ToRadians((float)(Math.Cos(model.roation) + Math.Sin(model.roation)))) * Matrix.CreateTranslation(m_position);
                effect.VertexColorEnabled = false;
                effect.TextureEnabled = true;
                effect.Texture = mesh.texture;
                effect.EnableDefaultLighting();
                
                //RasterizerState rs = new RasterizerState();
                //rs.CullMode = CullMode.None;
                //rs.FillMode = FillMode.Solid;
                m_game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DepthStencilState d = new DepthStencilState();
                d.DepthBufferEnable = true;
                m_game.GraphicsDevice.DepthStencilState = d;
                m_game.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    m_game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, mesh.verticies, 0, mesh.verticies.Length, mesh.indcicies, 0, mesh.indcicies.Length / 3);
                }
            }
        }
    }
    class SilkroadTexture
    {
        public Texture2D texture;
        public string name;
    }
}
