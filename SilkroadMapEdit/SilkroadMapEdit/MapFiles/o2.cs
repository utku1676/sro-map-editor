using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SilkroadMapEditor
{
    class o2
    {
        BinaryReader reader;
        public List<mapObject> mapObjects;
        public o2(byte[] file)
        {
            reader = new BinaryReader(new MemoryStream(file));
            ParseO2File(reader);
        }

        private void ParseO2File(BinaryReader reader)
        {
            reader.BaseStream.Position += 16; //Skip Header
            mapObjects = new List<mapObject>();

            for (int i = 0; i < 142; i++)
            {
                short count = reader.ReadInt16();
                for (int x = 0; x < count; x++)
                {
                    mapObject obj = new mapObject();
                    obj.group = 1;
                    obj.uID = reader.ReadInt32();
                    obj.x = reader.ReadSingle();
                    obj.y = reader.ReadSingle();
                    obj.z = reader.ReadSingle();
                    reader.ReadBytes(2);
                    obj.angle = reader.ReadSingle();
                    obj.ID = reader.ReadInt32();
                    reader.ReadBytes(2);
                    obj.xsec = reader.ReadByte();
                    obj.ysec = reader.ReadByte();
                    mapObjects.Add(obj);
                }
            }
            
        }

        public struct mapObject
        {
            public int group;
            public int uID;
            public float x;
            public float y;
            public float z;
            public float angle;
            public int ID;
            public byte xsec;
            public byte ysec;
        }
    }
}
