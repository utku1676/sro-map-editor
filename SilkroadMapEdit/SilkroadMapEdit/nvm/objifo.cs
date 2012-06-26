using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SilkroadMapEditor
{
    class objifo
    {
        private SilkroadMapEditor m_game;
        private List<objinfo> objinfos = new List<objinfo>();
        public objifo(SilkroadMapEditor game)
        {
            this.m_game = game;
            Load();
        }
        public void Load()
        {
            byte[] file = m_game.datapk2.getFile("object.ifo");
            StreamReader reader = new StreamReader(new MemoryStream(file));
            reader.ReadLine();
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                objinfo info = new objinfo();
                info.id = int.Parse(line.Substring(0, 5));
                info.param = line.Substring(6, 10);
                info.path = line.Substring(18, line.Length - 19);
                objinfos.Add(info);
            }
        }
        public string GetPathByID(int id)
        {
            objinfo info = objinfos.Find(t => t.id == id);
            if (info != null)
            {
                return info.path;
            }
            else
            {
                throw new Exception("Unknown objID");
            }
        }
        public class objinfo
        {
            public int id;
            public string param;
            public string path;
        }
    }
}
