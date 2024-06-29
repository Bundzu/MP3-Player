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
using mp5.Properties;
using System.Runtime.InteropServices;

namespace mp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
        private bool stateOfTehArt = false;
        private string fileN;
        private bool mute = false;
        private FileInfo fileInfo;

        static class NativeMethods
        {
            [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
            public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);

            [DllImport("winmm.dll", SetLastError = true)]
            public static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "mp3 files (*.mp3)|*.mp3|wav file (*.wav)|*.wav|All files (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            fileN = dialog.FileName;

            fileInfo = new System.IO.FileInfo(fileN);
            ListViewItem item = new ListViewItem(fileInfo.Name);

            this.listMusic.Items.Add(item);

            if(audioFileReader == null){
                audioFileReader = new AudioFileReader(fileN);
                waveOutDevice = new WaveOut();
                waveOutDevice.Init(audioFileReader);
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {  
            if (stateOfTehArt == false)
            {
                if (fileN == null)
                    return;
                if (audioFileReader == null && waveOutDevice == null && fileN != null)
                {
                    audioFileReader = new AudioFileReader(fileN);
                    waveOutDevice = new WaveOut();
                    waveOutDevice.Init(audioFileReader);
                    stateOfTehArt = true;
                    buttonPlay.BackgroundImage = Resources.pause;
                    waveOutDevice.Play();
                    label1.Text = "Now playing: " + fileInfo.Name;
                }
                else
                {
                    stateOfTehArt = true;
                    buttonPlay.BackgroundImage = Resources.pause;
                    waveOutDevice.Play();
                    label1.Text = "Now playing: " + fileInfo.Name;
                }               
            }
            else
            {
                stateOfTehArt = false;
                buttonPlay.BackgroundImage = Resources.play;
                waveOutDevice.Pause();
                label1.Text = "Now playing: ";
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if(waveOutDevice == null)
                return;
            else{
                stateOfTehArt = false;
                buttonPlay.BackgroundImage = Resources.play;
                waveOutDevice.Stop();
                audioFileReader.Dispose();
                audioFileReader = null;
                waveOutDevice.Dispose();
                waveOutDevice = null;
                label1.Text = "Now playing: ";
            }
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            double newVolume = ushort.MaxValue * trackBarVolume.Value / 100.0;
            uint v = ((uint)newVolume) & 0xffff;
            uint vAll = v | (v << 16);
            int retVal = NativeMethods.WaveOutSetVolume(IntPtr.Zero, vAll);
            if (vAll == 0)
            {
                buttonVol.BackgroundImage = Resources.Volume_Off;
                mute = true;
            }
            if (vAll < 2500000000 && vAll > 0)
            {
                buttonVol.BackgroundImage = Resources.Volume_Low;
                mute = false;
            }
            if (vAll > 2500000000)
            {
                buttonVol.BackgroundImage = Resources.Volume_High;
                mute = false;
            }

        }


        private void trackBarSeekBar_Scroll(object sender, EventArgs e)
        {
        }

        private void authorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Authors dialog = new Authors();
            dialog.ShowDialog();
        }

        private void buttonB_Click(object sender, EventArgs e)
        {
            long nPos = audioFileReader.Position + (long)(audioFileReader.WaveFormat.AverageBytesPerSecond * 2.5);
            nPos = Math.Max(0, Math.Min(audioFileReader.Length, nPos));
            audioFileReader.Position = nPos - 100;
        }

        private void buttonF_Click(object sender, EventArgs e)
        {
            long nPos = audioFileReader.Position + (long)(audioFileReader.WaveFormat.AverageBytesPerSecond * 2.5);
            nPos = Math.Max(0, Math.Min(audioFileReader.Length, nPos));
            audioFileReader.Position = nPos + 100;
        }

        private void convertMp3ToWavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Wav dialog = new Wav();
            dialog.ShowDialog();
        }

        private void buttonVol_Click(object sender, EventArgs e)
        {
            if (mute == false){
            int retVal = NativeMethods.WaveOutSetVolume(IntPtr.Zero, 0);
            buttonVol.BackgroundImage = Resources.Volume_Off;
            mute = true;
            trackBarVolume.Value = 0;
            }
            else{
                mute = false;
                buttonVol.BackgroundImage = Resources.Volume_High;
                int retVal = NativeMethods.WaveOutSetVolume(IntPtr.Zero, 4294967295);
                trackBarVolume.Value = 100;
            }
        }

        private void listMusic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                foreach (ListViewItem eachItem in listMusic.SelectedItems)
                {
                    listMusic.Items.Remove(eachItem);
                }
            }
        }

        private void listMusic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
                audioFileReader.Dispose();
                audioFileReader = null;
                waveOutDevice.Dispose();
                waveOutDevice = null;
            } 

            stateOfTehArt = true;
            buttonPlay.BackgroundImage = Resources.pause; 

            if (listMusic.SelectedItems.Count > 0)
            {               
                fileN = fileInfo.DirectoryName + @"\" + listMusic.SelectedItems[0].Text.ToString();
            }
            
            audioFileReader = new AudioFileReader(fileN);
            waveOutDevice = new WaveOut();
            waveOutDevice.Init(audioFileReader);
            stateOfTehArt = true;
            buttonPlay.BackgroundImage = Resources.pause;
            waveOutDevice.Play();
            label1.Text = "Now playing: " + fileInfo.Name;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opcje dialog = new opcje();
            dialog.ShowDialog();
            this.Opacity = dialog.opacit;
        }
    }
}
