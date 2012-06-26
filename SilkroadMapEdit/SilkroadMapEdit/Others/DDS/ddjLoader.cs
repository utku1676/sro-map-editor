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
using System.Drawing;
using System.Drawing.Imaging;


namespace SilkroadMapEditor
{
    class ddjLoader
    {
        public static Texture2D GetTexture(byte[] file,GraphicsDevice device)
        {
            Texture2D tmpTex;
            DDSLib.DDSFromStream(new MemoryStream(file), device, 20, false, out tmpTex);
            return tmpTex;
        }
    }
}
