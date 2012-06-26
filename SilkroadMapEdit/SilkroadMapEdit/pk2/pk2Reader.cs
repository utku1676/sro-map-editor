using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using pk2;

namespace pk2
{
    public class pk2Reader
    {
        Blowfish blowfish = new Blowfish(); //Credits as always to pushedx aka Drew_Benton
        byte[] bKey = new byte[] { 0x32, 0xCE, 0xDD, 0x7C, 0xBC, 0xA8 };
        Pk2 pk2File = new Pk2();

        Pk2.pFolder currentFolder;
        Pk2.pFolder mainFolder;
        FileStream fileStream;


        public pk2Reader(string silkroadPath)
        {
            if (!File.Exists(silkroadPath))
            {
                throw new Exception("pk2 not found. Please set the correct Path to your Silkroad directory");
            }
            fileStream = new FileStream(silkroadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            blowfish.Initialize(bKey);
            BinaryReader reader = new BinaryReader(fileStream);
            pk2File.header = (Pk2.pk2Header)BufferToStruct(reader.ReadBytes(256), typeof(Pk2.pk2Header));
            Console.WriteLine(pk2File.header.Name);
            currentFolder = new Pk2.pFolder();
            currentFolder.name = silkroadPath;
            currentFolder.files = new List<Pk2.pFile>();
            currentFolder.subfolders = new List<Pk2.pFolder>();

            mainFolder = currentFolder;
            Read(reader.BaseStream.Position);
            Console.WriteLine("Done. Found {0} files.", pk2File.Files.Count);
        }
        /// <summary>
        /// Not Well tested.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] getFilebyPath(string path)
        {
            string[] tmp = path.Split('\\');
            Pk2.pFolder folder;
            if (tmp[0] != "")
            {
                folder = pk2File.Folders.Find(t => t.name == tmp[0]);
                for (int i = 1; i < tmp.Length - 1; i++)
                {
                    folder = folder.subfolders.Find(t => t.name == tmp[i]);
                }
                Pk2.pFile file = folder.files.Find(t => t.name == tmp[tmp.Length - 1]);
                fileStream.Position = file.position;
                byte[] tmp_data = new byte[file.size];
                fileStream.Read(tmp_data, 0, tmp_data.Length);
                return tmp_data;
            }
            else
            {
                folder = pk2File.Folders.Find(t => t.name == tmp[1]);
                if (tmp.Length > 3)
                {
                    for (int i = 2; i < tmp.Length - 1; i++)
                    {
                        folder = folder.subfolders.Find(t => t.name == tmp[i]);
                    }
                    Pk2.pFile file = folder.files.Find(t => t.name == tmp[tmp.Length - 1]);
                    fileStream.Position = file.position;
                    byte[] tmp_data = new byte[file.size];
                    fileStream.Read(tmp_data, 0, tmp_data.Length);
                    return tmp_data;
                }
                else
                {
                    Pk2.pFile file = folder.files.Find(t => t.name ==tmp[2]);
                    fileStream.Position = file.position;
                    byte[] tmp_data = new byte[file.size];
                    fileStream.Read(tmp_data, 0, tmp_data.Length);
                    return tmp_data;
                }
            }          
        }

        private pk2.Pk2.pFolder FindFolder(Pk2.pFolder mainFolder, string p)
        {
            Pk2.pFolder rfolder = new Pk2.pFolder();
            foreach (Pk2.pFolder folder in mainFolder.subfolders)
            {
                if (folder.name == p)
                {
                    rfolder = folder;
                }
                if (rfolder == null)
                {
                    rfolder = CheckSubfolders(folder, p);
                }
            }
            return rfolder;
        }

        private Pk2.pFolder CheckSubfolders(Pk2.pFolder folder, string p)
        {
            Pk2.pFolder tmp = folder.subfolders.Find(t => t.name == p);
            if (tmp == null)
            {
                foreach (Pk2.pFolder fl in folder.subfolders)
                {
                    tmp =  CheckSubfolders(fl, p);
                }
            }
            return tmp;
            
        }

        public bool FileExists(string name)
        {
            Pk2.pFile file = pk2File.Files.Find(item => item.name == name);
            if (file.position != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte[] getFile(string name)
        {
            if (FileExists(name))
            {
                BinaryReader reader = new BinaryReader(fileStream);
                Pk2.pFile file = pk2File.Files.Find(item => item.name == name);
                reader.BaseStream.Position = file.position;
                return reader.ReadBytes((int)file.size);
            }
            else
            {
                throw new Exception(string.Format("pk2Reader: File not found: {0}", name));
            }
        }

        public List<string> GetFileNames()
        {
            List<string> tmpList = new List<string>();
            foreach(Pk2.pFile file in pk2File.Files)
            {
                tmpList.Add(file.name);
            }
            return tmpList;
        }

        void Read(long position)
        {
            BinaryReader reader = new BinaryReader(fileStream);
            reader.BaseStream.Position = position;
            List<Pk2.pFolder> folders = new List<Pk2.pFolder>();
            Pk2.Pk2EntryBlock entryBlock = (Pk2.Pk2EntryBlock)BufferToStruct(blowfish.Decode(reader.ReadBytes(Marshal.SizeOf(typeof(Pk2.Pk2EntryBlock)))), typeof(Pk2.Pk2EntryBlock));

         

            for (int i = 0; i < 20; i++)
            {
                Pk2.pk2Entry entry = entryBlock.entries[i]; //.....
                switch (entry.type)
                {
                    case 0: //Null Entry

                        break;
                    case 1: //Folder 
                        if (entry.name != "." && entry.name != "..")
                        {
                            Pk2.pFolder folder = new Pk2.pFolder();
                            folder.name = entry.name;
                            folder.position = BitConverter.ToInt64(entry.position, 0);
                            folders.Add(folder);
                            pk2File.Folders.Add(folder);
                            currentFolder.subfolders.Add(folder);
                        }
                        break;
                    case 2: //File
                        Pk2.pFile file = new Pk2.pFile();
                        file.position = entry.Position;
                        file.name = entry.name;
                        file.size = entry.Size;
                        file.parentFolder = currentFolder;
                        pk2File.Files.Add(file);
                        currentFolder.files.Add(file);
                        break;
                }

            }
            if (entryBlock.entries[19].nChain != 0)
            {
                Read(entryBlock.entries[19].nChain);
            }
           

            foreach (Pk2.pFolder folder in folders)
            {
                currentFolder = folder;
                if (folder.files == null)
                {
                    folder.files = new List<Pk2.pFile>();
                }
                if (folder.subfolders == null)
                {
                    folder.subfolders = new List<Pk2.pFolder>();
                }
                Console.WriteLine(folder.name);
                Read(folder.position);
            }

        }
        object BufferToStruct(byte[] buffer, Type returnStruct)
        {
            IntPtr pointer = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, pointer, buffer.Length);
            return Marshal.PtrToStructure(pointer, returnStruct);
        }
    }
}
