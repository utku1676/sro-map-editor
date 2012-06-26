using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SilkroadMapEditor
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }
        pk2.pk2Reader mediapk2;
        pk2.pk2Reader datapk2;
        pk2.pk2Reader mappk2;
        private void Window_Load(object sender, EventArgs e)
        {

        }
        List<mapObject_info> mapobjectList = new List<mapObject_info>();

        struct mapObject_info
        {
            public int id;
            public string unk;
            public string path;
        }


        public void LoadObjectList()
        {
            mediapk2 = Program.game.mediapk2;
            datapk2 = Program.game.datapk2;
            mappk2 = Program.game.mappk2;
            byte[] file = datapk2.getFile("object.ifo");
            StreamReader reader = new StreamReader(new MemoryStream(file));
            reader.ReadLine();
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] tmp = line.Split(' ');
                string model = line.Substring(18, line.Length - 19);
                tmp = model.Split('\\');
                model = tmp[tmp.Length - 1];
                lstObjects.Items.Add(model);

                mapObject_info obj = new mapObject_info();
                obj.id = int.Parse(line.Substring(0, 4));
                obj.unk = line.Substring(6, 10);
                obj.path = line.Substring(18, line.Length - 19);
                mapobjectList.Add(obj);
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.game.Exit();
            this.Close();
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.game.Exit();
        }

        private void lstObjects_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(mapobjectList[lstObjects.SelectedIndex].path);
            //Open up Model Viewer?
        }
    }
}
