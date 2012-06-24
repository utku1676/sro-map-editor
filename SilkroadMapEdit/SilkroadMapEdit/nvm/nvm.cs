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
    public class nvm
    {
        public static tNVMZone1[] zone1s;
        public static tNVMZone2[] zone2s;
        public static tNVMZone3[] zone3s;
        public static tTextureMapEntry[,] textureMap;
        public static tNVMEntity[] entities;
        private static float[] hightmap;

        public static VertexPositionColor[] GetVertices(byte[] file)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(file));
            ParseNVM(reader);
            VertexPositionColor[] vertices = new VertexPositionColor[97 * 97];
            for (int x = 0; x < 97; x++)
            {
                for (int y = 0; y < 97; y++)
                {
                    vertices[x + y * 97].Position = new Vector3(x * 20, GetHightAt(x, y), y * 20);
                    vertices[x + y * 97].Color = Color.Gray;

                }
            }
            return vertices;          
        }

        public static int[] GetIndicies()
        {
            int[] indices = new int[96 * 96 * 6];
            int counter = 0;
            for (int y = 0; y < 96; y++)
            {
                for (int x = 0; x < 96; x++)
                {
                    int lowerLeft = x + y * 97;
                    int lowerRight = (x + 1) + y * 97;
                    int topLeft = x + (y + 1) * 97;
                    int topRight = (x + 1) + (y + 1) * 97;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
            return indices;
        }

        public static float GetHightAt(int x, int y)
        {
            return hightmap[y * 97 + x];
        }
        public static ushort GetTextureIndexAt(int x, int y)
        {
            return textureMap[y, x].w4;
        }

        private static void ParseNVM(BinaryReader reader)
        {
            string company = new string(reader.ReadChars(4));
            string format = new string(reader.ReadChars(4));
            string version = new string(reader.ReadChars(4));
            ushort entryCount = reader.ReadUInt16();
            entities = new tNVMEntity[entryCount];
            for (int x = 0; x < entryCount; x++)
            {
                tNVMEntity obj = new tNVMEntity();
                obj.id = reader.ReadInt32();


                obj.x = reader.ReadSingle();
                obj.y = reader.ReadSingle();
                obj.z = reader.ReadSingle();
                obj.uk2 = reader.ReadUInt16();
                obj.uk3 = reader.ReadSingle();
                obj.uk4 = reader.ReadUInt16();
                obj.uk5 = reader.ReadUInt16();
                obj.uk6 = reader.ReadUInt16();
                obj.grid = reader.ReadUInt16();
                obj.extraCount = reader.ReadUInt16();
                if (obj.extraCount > 0)
                {
                    obj.extraArray = new tNVMEntityExtra[obj.extraCount];
                    for (int y = 0; y < obj.extraCount; y++)
                    {
                        obj.extraArray[y].field1 = reader.ReadUInt32();
                        obj.extraArray[y].field2 = reader.ReadUInt16();
                    }
                }
                entities[x] = obj;
            }
            uint zone1Count = reader.ReadUInt32();
            uint zone1Extra = reader.ReadUInt32();

            zone1s = new tNVMZone1[zone1Count];
            for (uint x = 0; x < zone1Count; x++)
            {
                tNVMZone1 obj = new tNVMZone1();
                obj.fX1 = reader.ReadSingle();
                obj.fY1 = reader.ReadSingle();
                obj.fX2 = reader.ReadSingle();
                obj.fY2 = reader.ReadSingle();
                obj.extraCount = reader.ReadByte();
                if (obj.extraCount > 0)
                {
                    obj.extraArray = new ushort[obj.extraCount];
                    for (byte y = 0; y < obj.extraCount; y++)
                    {
                        obj.extraArray[y] = reader.ReadUInt16();
                    }
                }
                zone1s[x] = obj;
            }

            uint zone2Count = reader.ReadUInt32();
            zone2s = new tNVMZone2[zone2Count];
            for (uint x = 0; x < zone2Count; x++)
            {
                tNVMZone2 obj = new tNVMZone2();
                obj.fX1 = reader.ReadSingle();
                obj.fY1 = reader.ReadSingle();
                obj.fX2 = reader.ReadSingle();
                obj.fY2 = reader.ReadSingle();
                obj.s3_b1 = reader.ReadByte();
                obj.s3_b2 = reader.ReadByte();
                obj.s3_b3 = reader.ReadByte();
                obj.s3_w2 = reader.ReadUInt16();
                obj.s3_w3 = reader.ReadUInt16();
                obj.s3_w4 = reader.ReadUInt16();
                obj.s3_w5 = reader.ReadUInt16();
                zone2s[x] = obj;
            }

            uint zone3Count = reader.ReadUInt32();
            zone3s = new tNVMZone3[zone3Count];
            for (uint x = 0; x < zone3Count; x++)
            {
                tNVMZone3 obj = new tNVMZone3();
                obj.fX1 = reader.ReadSingle();
                obj.fY1 = reader.ReadSingle();
                obj.fX2 = reader.ReadSingle();
                obj.fY2 = reader.ReadSingle();
                obj.s3_b1 = reader.ReadByte();
                obj.s3_b2 = reader.ReadByte();
                obj.s3_b3 = reader.ReadByte();
                obj.s3_w2 = reader.ReadUInt16();
                obj.s3_w3 = reader.ReadUInt16();
                zone3s[x] = obj;
            }
            textureMap = new tTextureMapEntry[96, 96];
            for (int x = 0; x < 96; x++)
            {
                for (int y = 0; y < 96; y++)
                {
                    textureMap[x, y].w1 = reader.ReadUInt16();
                    textureMap[x, y].w2 = reader.ReadUInt16();
                    textureMap[x, y].w3 = reader.ReadUInt16();
                    textureMap[x, y].w4 = reader.ReadUInt16();
                }
            }
            hightmap = new float[9409];
            for (int x = 0; x < 9409; x++)
            {
                hightmap[x] = reader.ReadSingle();
            }
        }
    }

    public struct tNVMEntity
    {
        public int id;
        public float x;
        public float y;
        public float z;
        public ushort uk2;
        public float uk3;
        public ushort uk4;
        public ushort uk5;
        public ushort uk6;
        public ushort grid;
        public ushort extraCount;
        public tNVMEntityExtra[] extraArray;
    }
    public struct tNVMEntityExtra
    {
        public uint field1;
        public ushort field2;
    }
    public struct tNVMZone1
    {
        public float fX1;
        public float fY1;
        public float fX2;
        public float fY2;
        public byte extraCount;
        public ushort[] extraArray;
    }
    public struct tNVMZone2
    {
        public float fX1;
        public float fY1;
        public float fX2;
        public float fY2;
        public byte s3_b1;
        public byte s3_b2;
        public byte s3_b3;
        public ushort s3_w2;
        public ushort s3_w3;
        public ushort s3_w4;
        public ushort s3_w5;
    }
    public struct tNVMZone3
    {
        public float fX1;
        public float fY1;
        public float fX2;
        public float fY2;
        public byte s3_b1;
        public byte s3_b2;
        public byte s3_b3;
        public ushort s3_w2;
        public ushort s3_w3;
    }
    public struct tTextureMapEntry
    {
        public ushort w1;
        public ushort w2;
        public ushort w3;
        public ushort w4;
    }
}
