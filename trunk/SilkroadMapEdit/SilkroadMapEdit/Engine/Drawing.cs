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
    public class Drawing : DrawableGameComponent
    {
        private SilkroadMapEditor m_game;
        public Drawing(SilkroadMapEditor game)
            : base(game)
        {
            m_game = game;
        }
        public bool ShowFPS {
            get
            {
            return b_showfps;   
            }
            set
            {
                b_showfps = value;
            }
        }
        private bool b_showfps = false;
        public void DrawString(string msg, Vector2 position, Color color)
        {
            m_game.spriteBatch.Begin();
            m_game.spriteBatch.DrawString(m_game.gameFont, msg, position, color);
            m_game.spriteBatch.End();
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.ShowFPS)
            {
                DrawFPS(gameTime);
            }
            base.Draw(gameTime);
        }

        private void DrawFPS(GameTime gameTime)
        {
            string fps = string.Format("FPS: {0}", (1.0 / gameTime.ElapsedGameTime.TotalSeconds).ToString("0.00"));
            DrawString(fps, new Vector2(10, 10), Color.Black);
        }
    }
}
