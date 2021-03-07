using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;
using System.Collections;

namespace Pack
{
    public partial class frmMain : Form
    {
        public Pack pack;
        string outputPath;
        string inputFile;

        public frmMain()
        {
            InitializeComponent();
        }

        public void StartUnpacking()
        {
            try
            {
                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);

                ZipFile.ExtractToDirectory(inputFile, outputPath, true);
                string[] files = File.ReadAllLines(outputPath + "\\meta.data");
                if (files.Length > 2)
                {
                    for (int i = 0; i < files.Length ; i++)
                    {
                        Entry entry = new Entry();
                        string[] exp = files[i].Split(":");
                        entry.filename = exp[0];
                        entry.fileSize = long.Parse(exp[1]);
                        entry.content = new List<byte>();
                        pack.content.Add(entry);
                    }
                    pack.name = inputFile;
                    byte[] data = File.ReadAllBytes(outputPath + "\\data.data");
                    BitArray[] outBits = new BitArray[8];
                    int obIndex = 0;
                    for (int e = 0; e < 8; e++)
                    {
                        outBits[e] = new BitArray(8);
                        outBits[e].SetAll(false);
                    }

                    BitArray tempBit = new BitArray(data);
                    for (long b = 0; b < tempBit.Length; b += 8)
                    {
                        outBits[0].Set(obIndex, tempBit.Get((int)b + 7));
                        outBits[1].Set(obIndex, tempBit.Get((int)b + 6));
                        outBits[2].Set(obIndex, tempBit.Get((int)b + 5));
                        outBits[3].Set(obIndex, tempBit.Get((int)b + 4));
                        outBits[4].Set(obIndex, tempBit.Get((int)b + 3));
                        outBits[5].Set(obIndex, tempBit.Get((int)b + 2));
                        outBits[6].Set(obIndex, tempBit.Get((int)b + 1));
                        outBits[7].Set(obIndex, tempBit.Get((int)b));
                        obIndex++;

                        if (obIndex == 8)
                        {
                            for (int w = 0; w < pack.content.Count; w++)
                            {
                                if (pack.content[w].fileSize > (b / 72))
                                    pack.content[w].content.Add(ConvertToByte(outBits[w]));
                            }
                            obIndex = 0;
                            foreach (BitArray ba in outBits)
                                ba.SetAll(false);
                        }
                    }
                    foreach (Entry entry in pack.content)
                    {
                        File.WriteAllBytes(outputPath.Replace("output", "") + entry.filename, entry.content.ToArray());
                    }
                    Directory.Delete(outputPath, true);
                    MessageBox.Show("Done unpacking!\nPath: " + outputPath.Replace("output", ""), "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("There's only one file in the packer!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StartPacking()
        {
            try
            {
                List<string> lines = new List<string>();

                foreach (Entry entry in pack.content)
                {
                    string[] temp = entry.filename.Split('\\');
                    entry.filename = temp[temp.Length - 1];
                    lines.Add(entry.filename + ":" + entry.fileSize.ToString() + "\n");
                }
                string tempString = "";
                foreach (string line in lines)
                    tempString += line;
                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);
                File.WriteAllText(outputPath + "\\meta.data", tempString);

                long max = pack.GetBiggestFile();
                List<byte> outBytes = new List<byte>();

                for (long i = 0; i < max; i++)
                {
                    byte[] currentByte = { 0, 0, 0, 0, 0, 0, 0, 0 };
                    for (int q = 0; q < pack.content.Count; q++)
                    {
                        if (pack.content[q].fileSize > i)
                            currentByte[q] = pack.content[q].content[(int)i];
                    }
                    for (int w = 7; w >= 0; w--)
                    {
                        BitArray b = new BitArray(8);
                        int tVal = (int)Math.Pow(2, w);
                        for (int q = 0; q < 8; q++)
                        {
                            bool flag = false;
                            if (currentByte[q] >= tVal)
                            {
                                flag = true;
                                currentByte[q] -= (byte)tVal;
                            }
                            b.Set(q, flag);
                        }
                        outBytes.Add(ConvertToByte(b));
                    }
                }
                string zipname = outputPath.Replace("output", DateTime.Now.ToString("yyyyMMddHHmmss")) + ".pack";
                File.WriteAllBytes(outputPath + "\\data.data", outBytes.ToArray());
                ZipFile.CreateFromDirectory(outputPath, zipname, CompressionLevel.Optimal, false);
                Directory.Delete(outputPath, true);
                MessageBox.Show("Done packing!\nPath: " + zipname, "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected byte ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("illegal number of bits");
            }

            byte b = 0;
            if (bits.Get(7)) b++;
            if (bits.Get(6)) b += 2;
            if (bits.Get(5)) b += 4;
            if (bits.Get(4)) b += 8;
            if (bits.Get(3)) b += 16;
            if (bits.Get(2)) b += 32;
            if (bits.Get(1)) b += 64;
            if (bits.Get(0)) b += 128;
            return b;
        }

        private void btnLoadFiles_Click(object sender, EventArgs e)
        {
            OFD.Title = "Choose files to pack";
            OFD.Multiselect = true;
            OFD.Filter = "All files (*.*)|*.*";
            OFD.FilterIndex = 1;
            DialogResult dr = OFD.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (OFD.FileNames.Length > 1 && OFD.FileNames.Length <= 8)
                {
                    outputPath = OFD.FileNames[0].Replace(OFD.SafeFileNames[0], "") + "output";
                    pack = new Pack();
                    pack.content = new List<Entry>();
                    foreach (string filename in OFD.FileNames)
                    {
                        Entry entry = new Entry();
                        entry.filename = filename;
                        entry.content = new List<byte>();
                        entry.content.AddRange(File.ReadAllBytes(filename));
                        entry.fileSize = entry.content.Count;
                        pack.content.Add(entry);
                    }
                    StartPacking();
                }
                else
                {
                    MessageBox.Show("This packer can only pack files from two to eight, so far.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUnloadFile_Click(object sender, EventArgs e)
        {
            OFD.Title = "Choose pack to unpack";
            OFD.Multiselect = false;
            OFD.Filter = "Pack file (.pack)|*.pack";
            OFD.FilterIndex = 1;

            DialogResult dr = OFD.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (OFD.FileName != "")
                {
                    inputFile = OFD.FileName;
                    outputPath = OFD.FileName.Replace(OFD.SafeFileName, "") + "output";
                    pack = new Pack();
                    pack.content = new List<Entry>();
                    StartUnpacking();
                }
                else
                {
                    MessageBox.Show("Must choose a file to unpack.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
