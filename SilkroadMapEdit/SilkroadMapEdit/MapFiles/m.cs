using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace SilkroadMapEditor
{
    class m
    {
        public m(byte[] file)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(file));
            reader.BaseStream.Position += 12; //Skip Header
            for (int i = 0; i < 36; i++)
            {
                reader.ReadBytes(6);
                for (int x = 0; x < 17 * 17; x++)
                {
                    MCell cell = new MCell();
                    cell.height = reader.ReadSingle();
                    byte[] tmp = reader.ReadBytes(2);
                    cell.surfaceID = ReadCustomInt(tmp, 10);
                    cell.flag_6 = (byte)ReadCustomInt(tmp, 1, 10);
                    cell.flag_5 = (byte)ReadCustomInt(tmp, 1, 10);
                    cell.flag_4 = (byte)ReadCustomInt(tmp, 1, 10);
                    cell.flag_3 = (byte)ReadCustomInt(tmp, 1, 10);
                    cell.flag_2 = (byte)ReadCustomInt(tmp, 1, 10);
                    cell.flag_1 = (byte)ReadCustomInt(tmp, 1, 10);
                    cell.light = reader.ReadByte();

                }
                reader.ReadBytes(546);
            }
        }

        public int ReadCustomInt(byte[] data, int size, int offset = 0)
        {
            int tmp = 0;
            BitArray test = new BitArray(data);
            for (int y = 0; y < size - offset; y++)
            {
                if (test[y + offset])
                {
                    tmp += (int)Math.Pow(2, y);
                }
            }
            return tmp;
        }

        struct MCell
        {
            public float height;
            public int surfaceID;
            public byte flag_6;
            public byte flag_5;
            public byte flag_4;
            public byte flag_3;
            public byte flag_2;
            public byte flag_1;
            public byte light;
        }
    }
}
