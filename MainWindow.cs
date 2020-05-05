using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuiEditor.Properties;
using System.Drawing.Imaging;
using System.Linq;

namespace FuiEditor
{
    public partial class MainWindow : Form
    {
        private static readonly byte[] PngStartPattern =
        {
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
        };

        private string currentOpenFui = null;
        private bool shouldSaveFui = false;
        private byte[] fuiMainBytes;
        private List<FuiImageInfo> fuiImageInfo = new List<FuiImageInfo>();
        private List<Image> originalImages = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnFileOpenClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "FUI files (*.fui)|*.fui|All Files (*.*)|*.*";
            fileDialog.Title = Resources.DialogOpenFui;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = fileDialog.FileName;
                OpenFui(filepath);
            }
        }

        private void OnFileSaveClick(object sender, EventArgs e)
        {
            SaveFui(currentOpenFui);
        }

        private void OnFileSaveAsClick(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog()
            {
                Filter = "Fui Files (*.fui)|*.fui",
                Title = Resources.DialogSaveFui,
                RestoreDirectory = true
            };

            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = fileDialog.FileName;
                SaveFui(filepath);
            }
        }

        private void OnFileExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnImageReplace(object sender, EventArgs e)
        {
            if (imageListView.SelectedIndices.Count < 1)
            {
                MessageBox.Show(Resources.DialogSelectImage, Resources.DialogInfo);
                return;
            }

            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg)|*.jpg;*.jpeg",
                Title = Resources.DialogReplaceImage,
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                int selected = imageListView.SelectedIndices[0];
                string filepath = fileDialog.FileName;
                Image imageLoaded = Image.FromFile(filepath);
                originalImages[selected] = imageLoaded;
                imageList.Images[selected] = CreateThumbnail(imageLoaded, imageList.ImageSize);
                imageListView.Items[selected].Text = $"{imageLoaded.Width}x{imageLoaded.Height}";

                shouldSaveFui = true;
            }
        }

        private void OnImageSave(object sender, EventArgs e)
        {
            if (imageListView.SelectedIndices.Count < 1)
            {
                MessageBox.Show(Resources.DialogSelectImage, Resources.DialogInfo);
                return;
            }

            SaveFileDialog fileDialog = new SaveFileDialog()
            {
                Filter = "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg)|*.jpg;*.jpeg",
                Title = Resources.DialogSaveImage,
                RestoreDirectory = true
            };

            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                int selected = imageListView.SelectedIndices[0];
                string filepath = fileDialog.FileName;
                Image imageSave = originalImages[selected];

                if(File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                ImageFormat format = Path.GetExtension(filepath) == ".png" ? ImageFormat.Png : ImageFormat.Jpeg;
                imageSave.Save(filepath, format);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if(shouldSaveFui && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = NotifySave();
            }
        }

        private void OnFuiOpened(string filepath)
        {
            currentOpenFui = filepath;
            fileSaveStripMenu.Enabled = true;
            fileSaveAsStripMenu.Enabled = true;
        }

        private void OnFuiOpen()
        {
            fuiImageInfo.Clear();
            originalImages.Clear();
            imageList.Images.Clear();
            imageListView.Items.Clear();
            currentOpenFui = null;
            fileSaveStripMenu.Enabled = false;
            fileSaveAsStripMenu.Enabled = false;
            shouldSaveFui = false;
        }

        private void OnFuiSaved(string filepath)
        {
            shouldSaveFui = false;
            MessageBox.Show(string.Format(Resources.DialogSavedFui, filepath));
        }

        private bool NotifySave()
        {
            DialogResult result = MessageBox.Show(Resources.DialogNotifySave, Resources.DialogWarning, MessageBoxButtons.YesNo);

            if(result == DialogResult.Yes)
            {
                return false;
            }

            return true;
        }

        private void OpenFui(string filepath)
        {
            OnFuiOpen();

            byte[] filedata = File.ReadAllBytes(filepath);
            int pngIndex = ArrayUtils.SearchBytes(filedata, 0, PngStartPattern);
            if (pngIndex < 0)
            {
                MessageBox.Show(Resources.DialogFailedToOpen, Resources.DialogError);
                return;
            }

            FuiImageInfo[] imageInfo = FuiUtils.GetImageInfo(filedata, pngIndex);
            Image[] images = FuiUtils.GetImages(filedata, pngIndex, imageInfo);
            originalImages.AddRange(images);
            for (int i = 0; i < images.Length; i++)
            {
                Image image = images[i];
                imageList.Images.Add(CreateThumbnail(image, imageList.ImageSize));
                imageListView.Items.Add($"{image.Width}x{image.Height}", i);
            }

            fuiMainBytes = filedata.Take(pngIndex - imageInfo.Length * FuiImageInfo.NativeSize).ToArray();
            fuiImageInfo.AddRange(imageInfo);

            OnFuiOpened(filepath);
        }

        private void SaveFui(string filepath)
        {
            List<byte> fuiBytes = new List<byte>();
            List<byte> imageSection = new List<byte>();
            int currentOffset = 0;

            fuiBytes.AddRange(fuiMainBytes);

            for(int i=0; i<fuiImageInfo.Count; i++)
            {
                FuiImageInfo imageInfo = fuiImageInfo[i];
                imageInfo.ImageOffset = currentOffset;
                using (MemoryStream stream = new MemoryStream())
                {
                    Image imageSave = originalImages[i];
                    imageSave.Save(stream, imageSave.RawFormat);

                    byte[] imageBytes = stream.ToArray();
                    imageInfo.ImageSize = imageBytes.Length;

                    fuiBytes.AddRange(imageInfo.ToByteArray());
                    imageSection.AddRange(imageBytes);

                    currentOffset += imageBytes.Length;
                }
            }

            fuiBytes.AddRange(imageSection);
            File.WriteAllBytes(filepath, fuiBytes.ToArray());

            OnFuiSaved(filepath);
        }

        private Image CreateThumbnail(Image original, Size size)
        {
            return CreateThumbnail(original, size.Width, size.Height);
        }

        //CreateThumbnail method, thx!
        //https://www.atmarkit.co.jp/ait/articles/0508/12/news091.html
        private Image CreateThumbnail(Image original, int w, int h)
        {
            Bitmap canvas = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);

            float fw = (float)w / (float)original.Width;
            float fh = (float)h / (float)original.Height;

            float scale = Math.Min(fw, fh);
            fw = original.Width * scale;
            fh = original.Height * scale;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //g.DrawImage(original, (w - fw) / 2, (h - fh) / 2, fw, fh);
            g.DrawImage(original,
                new RectangleF((w - fw) / 2, (h - fh) / 2, fw, fh),
                new RectangleF(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
            g.Dispose();

            return canvas;
        }
    }
}
