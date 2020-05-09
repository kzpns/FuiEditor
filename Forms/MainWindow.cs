using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using FuiEditor.Properties;
using FuiEditor.Utils;

namespace FuiEditor.Forms
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
        private List<byte[]> originalImagesData = new List<byte[]>();
        private List<ImageFormat> originalImageFormats = new List<ImageFormat>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClickFileOpen(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Fui files (*.fui)|*.fui|All Files (*.*)|*.*";
            fileDialog.Title = Resources.DialogOpenFui;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = fileDialog.FileName;
                OpenFui(filepath);
            }
        }

        private void OnClickFileSave(object sender, EventArgs e)
        {
            SaveFui(currentOpenFui);
        }

        private void OnClickFileSaveAs(object sender, EventArgs e)
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

        private void OnClickFileExit(object sender, EventArgs e)
        {
            Close();
        }

        private void OnClickImageReplace(object sender, EventArgs e)
        {
            if (imageListView.SelectedIndices.Count < 1)
            {
                MessageBox.Show(Resources.DialogSelectImage, Resources.DialogInfo);
                return;
            }

            CommonFileDialogCheckBox correctColorCb = new CommonFileDialogCheckBox("correctColorCb",
                Resources.DialogCorrectColor, false);

            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog(Resources.DialogReplaceImage)
            {
                Filters =
                {
                    new CommonFileDialogFilter("PNG", "*.png"),
                    new CommonFileDialogFilter("JPEG", "*.jpg;*.jpeg"),
                    new CommonFileDialogFilter(Resources.DialogAllFiles, "*.*")
                },
                Controls =
                {
                    correctColorCb
                },
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                int selected = imageListView.SelectedIndices[0];
                string filepath = fileDialog.FileName;
                byte[] filedata = File.ReadAllBytes(filepath);

                if (!correctColorCb.IsChecked)
                {
                    originalImagesData[selected] = filedata;
                }

                using (MemoryStream stream = new MemoryStream(filedata, false))
                {
                    Image imageLoaded = Image.FromStream(stream);

                    if(correctColorCb.IsChecked)
                    {
                        ImageUtils.ReverseColorRB((Bitmap)imageLoaded);
                        MemoryStream saveStream = new MemoryStream();

                        imageLoaded.Save(saveStream, imageLoaded.RawFormat);
                        originalImagesData[selected] = saveStream.ToArray();

                        saveStream.Dispose();
                    }
                    else
                    {
                        originalImagesData[selected] = filedata;
                    }

                    imageList.Images[selected].Dispose();
                    imageList.Images[selected] = ImageUtils.CreateThumbnail(imageLoaded, imageList.ImageSize);
                    imageListView.Items[selected].Text = $"{imageLoaded.Width}x{imageLoaded.Height}";

                    imageLoaded.Dispose();
                }

                shouldSaveFui = true;
            }
        }

        private void OnClickImageSave(object sender, EventArgs e)
        {
            if (imageListView.SelectedIndices.Count < 1)
            {
                MessageBox.Show(Resources.DialogSelectImage, Resources.DialogInfo);
                return;
            }

            CommonFileDialogCheckBox correctColorCb = new CommonFileDialogCheckBox("correctColorCb",
                Resources.DialogCorrectColor, false);

            ImageFormat[] imageFormats =
            {
                ImageFormat.Png,
                ImageFormat.Jpeg
            };

            CommonSaveFileDialog fileDialog = new CommonSaveFileDialog(Resources.DialogSaveImage)
            {
                Filters =
                {
                    new CommonFileDialogFilter("PNG", "*.png"),
                    new CommonFileDialogFilter("JPEG", "*.jpg;*.jpeg")
                },
                Controls =
                {
                    correctColorCb
                },
                RestoreDirectory = true
            };

            if(fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string filepath = fileDialog.FileName;
                int selected = imageListView.SelectedIndices[0];
                byte[] imageData = originalImagesData[selected];
                ImageFormat saveFormat = imageFormats[fileDialog.SelectedFileTypeIndex - 1];
                ImageFormat imageFormat = originalImageFormats[selected];

                if (!correctColorCb.IsChecked && saveFormat == imageFormat)
                {
                    File.WriteAllBytes(filepath, imageData);
                }
                else
                {
                    using (MemoryStream stream = new MemoryStream(imageData))
                    {
                        Image imageSave = Image.FromStream(stream);

                        if (correctColorCb.IsChecked)
                        {
                            ImageUtils.ReverseColorRB((Bitmap)imageSave);
                        }

                        imageSave.Save(filepath, saveFormat);
                        imageSave.Dispose();
                    }
                }
            }

            fileDialog.Dispose();
        }

        private void OnClickFileSaveAllImages(object sender, EventArgs e)
        {
            Dictionary<ImageFormat, string> extensions = new Dictionary<ImageFormat, string>()
            {
                {
                    ImageFormat.Png, ".png"
                },
                {
                    ImageFormat.Jpeg, ".jpg"
                }
            };

            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog(Resources.DialogSaveAllImages)
            {
                IsFolderPicker = true,
                RestoreDirectory = true
            };

            if(fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string directorySelected = fileDialog.FileName;
                string filepathBase = Path.Combine(
                    directorySelected, Path.GetFileNameWithoutExtension(currentOpenFui) + "_{0}{1}");

                for(int i=0; i<originalImageFormats.Count; i++)
                {
                    string filepath;
                    ImageFormat imageFormat = originalImageFormats[i];

                    if(extensions.ContainsKey(imageFormat))
                    {
                        string extension = extensions[imageFormat];
                        filepath = string.Format(filepathBase, i, extension);
                    }
                    else
                    {
                        filepath = string.Format(filepathBase, i, "img");
                    }

                    File.WriteAllBytes(filepath, originalImagesData[i]);
                }
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if(shouldSaveFui && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = NotifySave();
            }
        }

        private void OnOpendFui(string filepath)
        {
            currentOpenFui = filepath;
            fileSaveStripMenu.Enabled = true;
            fileSaveAsStripMenu.Enabled = true;
            fileSaveAllImagesStripMenu.Enabled = true;

            SetStatus(Path.GetFileName(filepath));
        }

        private void OnOpenFui()
        {
            foreach(Image image in imageList.Images)
            {
                image.Dispose();
            }
            fuiImageInfo.Clear();
            originalImagesData.Clear();
            originalImageFormats.Clear();
            imageList.Images.Clear();
            imageListView.Items.Clear();
            currentOpenFui = null;
            fileSaveStripMenu.Enabled = false;
            fileSaveAsStripMenu.Enabled = false;
            shouldSaveFui = false;
            fileSaveAllImagesStripMenu.Enabled = false;

            SetStatus(null);
        }

        private void OnSavedFui(string filepath)
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

        private void SetStatus(string message)
        {
            if (message != null)
            {
                Text = message + " - Fui Editor";
            }
            else
            {
                Text = "Fui Editor";
            }
        }

        private void OpenFui(string filepath)
        {
            OnOpenFui();

            byte[] filedata = File.ReadAllBytes(filepath);
            int pngIndex = ArrayUtils.SearchBytes(filedata, 0, PngStartPattern);
            if (pngIndex < 0)
            {
                MessageBox.Show(Resources.DialogFailedToOpen, Resources.DialogError);
                return;
            }

            FuiImageInfo[] imageInfo = FuiUtils.GetImageInfo(filedata, pngIndex);
            List<byte[]> imagesData = FuiUtils.GetImagesData(filedata, pngIndex, imageInfo);
            originalImagesData.AddRange(imagesData);
            for (int i = 0; i < imagesData.Count; i++)
            {
                byte[] imageData = imagesData[i];
                using (MemoryStream stream = new MemoryStream(imageData, false))
                {
                    Image image = Image.FromStream(stream);
                    originalImageFormats.Add(image.RawFormat);
                    imageList.Images.Add(ImageUtils.CreateThumbnail(image, imageList.ImageSize));
                    imageListView.Items.Add($"{image.Width}x{image.Height}", i);
                    image.Dispose();
                }
            }

            fuiMainBytes = filedata.Take(pngIndex - imageInfo.Length * FuiImageInfo.NativeSize).ToArray();
            fuiImageInfo.AddRange(imageInfo);

            OnOpendFui(filepath);
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
                using (MemoryStream imageStream = new MemoryStream(originalImagesData[i], false))
                {
                    Image imageSave = Image.FromStream(imageStream);

                    byte[] imageBytes = originalImagesData[i];
                    imageInfo.Width = imageSave.Width;
                    imageInfo.Height = imageSave.Height;
                    imageInfo.ImageOffset = currentOffset;
                    imageInfo.ImageSize = imageBytes.Length;

                    fuiBytes.AddRange(imageInfo.ToByteArray());
                    imageSection.AddRange(imageBytes);

                    currentOffset += imageBytes.Length;
                    imageSave.Dispose();
                }
            }

            fuiBytes.AddRange(imageSection);
            File.WriteAllBytes(filepath, FuiUtils.ProcessHeader(fuiBytes.ToArray()));

            OnSavedFui(filepath);
        }
    }
}
