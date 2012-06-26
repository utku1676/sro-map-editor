using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SilkroadMapEditor
{
    class bmt
    {
        List<string> textures = new List<string>();
        public bmt(string path,pk2.pk2Reader pk2reader)
        {
            byte[] file = pk2reader.getFilebyPath(path);
            BinaryReader reader = new BinaryReader(new MemoryStream(file));
            ParseBMT(reader);
        }

        private void ParseBMT(BinaryReader reader)
        {
            string header = new string(reader.ReadChars(12));
            int textureCount = reader.ReadInt32();
            for (int i = 0; i < textureCount; i++)
            {
                string name = new string(reader.ReadChars(reader.ReadInt32()));
                reader.BaseStream.Position += 0x48;
                string textureName = new string(reader.ReadChars(reader.ReadInt32()));
                if(textureName.Contains('\\'))
                {
                    textureName = textureName.Split('\\')[textureName.Split('\\').Length - 1];
                }
                reader.BaseStream.Position += 7;
                textures.Add(textureName);
            }
        }

        public List<string> GetTextureList()
        {
            return textures;
        }
    }
}
