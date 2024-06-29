using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NAudio;
using NAudio.Wave;

namespace mp5
{
    public partial class Wav : Form
    {
        public Wav()
        {
            InitializeComponent();
        }

        private string fileI;
        private string fileO;

        private void Wav_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0) 
                {
                    fileI = files[0];
                    textBox1.Text = files[0];
                }
            }
        }

        private void Wav_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "mp3 files (*.mp3)|*.mp3";
            DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            fileI = dialog.FileName;
            textBox1.Text = fileI;
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "wave files (*.wav)| *.wav";
            DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            fileO = dialog.FileName;
            textBox2.Text = fileO;
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            if (fileO != null && fileI != null)
            {
                using (Mp3FileReader reader = new Mp3FileReader(fileI))
                {
                    WaveFileWriter.CreateWaveFile(fileO, reader);
                }
                this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Check file paths");
            }
        }
    }
}
