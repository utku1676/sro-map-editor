using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SilkroadMapEditor
{
    class bsr
    {
        public bsr(byte[] file)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(file));
            ParseBSR(reader);
        }
        string[] bmtArray;
        string[] bmsArray;
        string[] banArray;
        private void ParseBSR(BinaryReader reader)
        {
            reader.ReadBytes(12); //Skip Header
            reader.ReadBytes(13 * 4);
            uint bsrType = reader.ReadUInt32();
            int titleLen = reader.ReadInt32();
            string title = new string(reader.ReadChars(titleLen));
            if (bsrType == 0x20003)
            {
                int unk = reader.ReadInt32();
                int unk2 = reader.ReadInt32();
                reader.BaseStream.Position += 0x28;
                string unk123 = new string(reader.ReadChars(reader.ReadInt32()));
                for (int x = 0; x < 2; x++)
                {
                    float[] tmp = new float[6];
                    for (int i = 0; i < 6; i++)
                    {
                        tmp[i] = reader.ReadSingle();
                    }
                }
                int unk3 = reader.ReadInt32();
                int bmtCount = reader.ReadInt32();
                int unk4 = reader.ReadInt32();
                bmtArray = new string[bmtCount];
                for (int x = 0; x < bmtCount; x++)
                {
                    bmtArray[x] = new string(reader.ReadChars(reader.ReadInt32()));
                    if (x + 1 != bmtCount)
                    {
                        uint tt = reader.ReadUInt32();
                    }
                }
                uint bmsCount = reader.ReadUInt32();
                bmsArray = new string[bmsCount];
                for (int t = 0; t < bmsCount; t++)
                {
                    bmsArray[t] = new string(reader.ReadChars(reader.ReadInt32()));
                }
                uint unkSize = reader.ReadUInt32();
                uint unk8 = reader.ReadUInt32();
                int banCount = reader.ReadInt32();
                banArray = new string[banCount];
                for (int x = 0; x < banCount; x++)
                {
                    banArray[x] = new string(reader.ReadChars(reader.ReadInt32()));
                }
                //float width = tmp[3] - tmp[0]; Console.WriteLine(width);
                //float height = tmp[4] - tmp[1]; Console.WriteLine(height);
                //float lenght = tmp[5] - tmp[2]; Console.WriteLine(lenght);
            }
            if (bsrType == 0x20004)
            {
                int aunk = reader.ReadInt32();
                int aunk2 = reader.ReadInt32();
                int aunk3 = reader.ReadInt32();
                int aunk4 = reader.ReadInt32();
                int aunk5 = reader.ReadInt32();
                int aunk6 = reader.ReadInt32();
                int aunk7 = reader.ReadInt32();
                int aunk8 = reader.ReadInt32();
                int aunk9 = reader.ReadInt32();
                int aunk10 = reader.ReadInt32();
                int aunk11 = reader.ReadInt32();
                int aunk12 = reader.ReadInt32();
                //int aunk13 = reader.ReadInt32();
                string unk = new string(reader.ReadChars(reader.ReadInt32()));
                //reader.BaseStream.Position += 0x2C;
                for (int x = 0; x < 2; x++)
                {
                    float[] tmp = new float[6];
                    for (int i = 0; i < 6; i++)
                    {
                        tmp[i] = reader.ReadSingle();
                    }
                }
                int unk3 = reader.ReadInt32();
                int bmtCount = reader.ReadInt32();
                int unk4 = reader.ReadInt32();
                bmtArray = new string[bmtCount];
                for (int x = 0; x < bmtCount; x++)
                {
                    bmtArray[x] = new string(reader.ReadChars(reader.ReadInt32()));
                    if (x + 1 != bmtCount)
                    {
                        uint tt = reader.ReadUInt32();
                    }
                }
                uint bmsCount = reader.ReadUInt32();
                bmsArray = new string[bmsCount];
                for (int t = 0; t < bmsCount; t++)
                {
                    bmsArray[t] = new string(reader.ReadChars(reader.ReadInt32()));
                    if (aunk == 3 && aunk2 == 2)
                    {
                        int unk55 = reader.ReadInt32();
                    }
                }
                //int bskLen = reader.ReadInt32();
                //string bsk = new string(reader.ReadChars(bskLen));
                //uint unk8 = reader.ReadUInt32();
                //int banCount = reader.ReadInt32();
                //banArray = new string[banCount];
                //for (int x = 0; x < banCount; x++)
                //{
                //    banArray[x] = new string(reader.ReadChars(reader.ReadInt32()));
                //}
            }
            if (bsrType == 0x20002)
            {
                int unk = reader.ReadInt32();
                int unk2 = reader.ReadInt32();
                reader.BaseStream.Position += 0x24;
                int unkunk = reader.ReadInt32();
                string test = new string(reader.ReadChars(reader.ReadInt32()));
                for (int x = 0; x < 2; x++)
                {
                    float[] tmp = new float[6];
                    for (int i = 0; i < 6; i++)
                    {
                        tmp[i] = reader.ReadSingle();
                    }
                }
                int unk3 = reader.ReadInt32();
                int bmtCount = reader.ReadInt32();
                int unk4 = reader.ReadInt32();
                bmtArray = new string[bmtCount];
                for (int x = 0; x < bmtCount; x++)
                {
                    bmtArray[x] = new string(reader.ReadChars(reader.ReadInt32()));
                    if (x + 1 != bmtCount)
                    {
                        uint tt = reader.ReadUInt32();
                    }
                }
                uint bmsCount = reader.ReadUInt32();
                bmsArray = new string[bmsCount];
                for (int t = 0; t < bmsCount; t++)
                {
                    bmsArray[t] = new string(reader.ReadChars(reader.ReadInt32()));
                }
                uint unkSize = reader.ReadUInt32();
                uint unk8 = reader.ReadUInt32();
                int banCount = reader.ReadInt32();
                banArray = new string[banCount];
                for (int x = 0; x < banCount; x++)
                {
                    banArray[x] = new string(reader.ReadChars(reader.ReadInt32()));
                }
            }
            if (bsrType == 0x20001)
            {
                uint unk = reader.ReadUInt32();
                uint unkCount = reader.ReadUInt32();
                reader.BaseStream.Position += 0x2C;
                for (int x = 0; x < 2; x++)
                {
                    float[] tmp = new float[6];
                    for (int i = 0; i < 6; i++)
                    {
                        tmp[i] = reader.ReadSingle();
                    }
                }
                int unko1 = reader.ReadInt32();
                for (int x = 0; x < unko1; x++)
                {
                    reader.BaseStream.Position += 64;
                }
                int bmtCount = reader.ReadInt32();
                int unko2 = reader.ReadInt32();
                bmtArray = new string[bmtCount];
                for (int x = 0; x < bmtCount; x++)
                {
                    bmtArray[x] = new string(reader.ReadChars(reader.ReadInt32()));
                    if (x + 1 != bmtCount)
                    {
                        uint tt = reader.ReadUInt32();
                    }
                }

                uint bmsCount = reader.ReadUInt32();
                bmsArray = new string[bmsCount];
                for (int t = 0; t < bmsCount; t++)
                {
                    bmsArray[t] = new string(reader.ReadChars(reader.ReadInt32()));
                }
                uint unkSize = reader.ReadUInt32();
                uint unk8 = reader.ReadUInt32();
                int banCount = reader.ReadInt32();
                banArray = new string[banCount];
                for (int x = 0; x < banCount; x++)
                {
                    banArray[x] = new string(reader.ReadChars(reader.ReadInt32()));
                }

            }
        }

        public List<string> GetBMSPaths()
        {
            return bmsArray.ToList<string>();
        }

        public List<string> GetBMTPaths()
        {
            return bmtArray.ToList<string>();
        }
    }
}
