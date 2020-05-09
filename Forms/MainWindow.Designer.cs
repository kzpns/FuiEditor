namespace FuiEditor.Forms
{
    partial class MainWindow
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.fileStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fileOpenStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fileStripMenuSeparatorOpenSave = new System.Windows.Forms.ToolStripSeparator();
            this.fileSaveStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSaveAsStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSaveAllImagesStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fileStripMenuSeparatorSaveExit = new System.Windows.Forms.ToolStripSeparator();
            this.fileExitStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.statusLabel = new System.Windows.Forms.Label();
            this.imageListView = new System.Windows.Forms.ListView();
            this.imageMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageReplaceStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.imageSaveStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip.SuspendLayout();
            this.imageMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileStripMenu
            // 
            this.fileStripMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileOpenStripMenu,
            this.fileStripMenuSeparatorOpenSave,
            this.fileSaveStripMenu,
            this.fileSaveAsStripMenu,
            this.fileSaveAllImagesStripMenu,
            this.fileStripMenuSeparatorSaveExit,
            this.fileExitStripMenu});
            this.fileStripMenu.Name = "fileStripMenu";
            resources.ApplyResources(this.fileStripMenu, "fileStripMenu");
            // 
            // fileOpenStripMenu
            // 
            this.fileOpenStripMenu.Name = "fileOpenStripMenu";
            resources.ApplyResources(this.fileOpenStripMenu, "fileOpenStripMenu");
            this.fileOpenStripMenu.Click += new System.EventHandler(this.OnClickFileOpen);
            // 
            // fileStripMenuSeparatorOpenSave
            // 
            this.fileStripMenuSeparatorOpenSave.Name = "fileStripMenuSeparatorOpenSave";
            resources.ApplyResources(this.fileStripMenuSeparatorOpenSave, "fileStripMenuSeparatorOpenSave");
            // 
            // fileSaveStripMenu
            // 
            resources.ApplyResources(this.fileSaveStripMenu, "fileSaveStripMenu");
            this.fileSaveStripMenu.Name = "fileSaveStripMenu";
            this.fileSaveStripMenu.Click += new System.EventHandler(this.OnClickFileSave);
            // 
            // fileSaveAsStripMenu
            // 
            resources.ApplyResources(this.fileSaveAsStripMenu, "fileSaveAsStripMenu");
            this.fileSaveAsStripMenu.Name = "fileSaveAsStripMenu";
            this.fileSaveAsStripMenu.Click += new System.EventHandler(this.OnClickFileSaveAs);
            // 
            // fileSaveAllImagesStripMenu
            // 
            resources.ApplyResources(this.fileSaveAllImagesStripMenu, "fileSaveAllImagesStripMenu");
            this.fileSaveAllImagesStripMenu.Name = "fileSaveAllImagesStripMenu";
            this.fileSaveAllImagesStripMenu.Click += new System.EventHandler(this.OnClickFileSaveAllImages);
            // 
            // fileStripMenuSeparatorSaveExit
            // 
            this.fileStripMenuSeparatorSaveExit.Name = "fileStripMenuSeparatorSaveExit";
            resources.ApplyResources(this.fileStripMenuSeparatorSaveExit, "fileStripMenuSeparatorSaveExit");
            // 
            // fileExitStripMenu
            // 
            this.fileExitStripMenu.Name = "fileExitStripMenu";
            resources.ApplyResources(this.fileExitStripMenu, "fileExitStripMenu");
            this.fileExitStripMenu.Click += new System.EventHandler(this.OnClickFileExit);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileStripMenu});
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Name = "menuStrip";
            // 
            // statusLabel
            // 
            resources.ApplyResources(this.statusLabel, "statusLabel");
            this.statusLabel.Name = "statusLabel";
            // 
            // imageListView
            // 
            resources.ApplyResources(this.imageListView, "imageListView");
            this.imageListView.ContextMenuStrip = this.imageMenuStrip;
            this.imageListView.HideSelection = false;
            this.imageListView.LargeImageList = this.imageList;
            this.imageListView.MultiSelect = false;
            this.imageListView.Name = "imageListView";
            this.imageListView.SmallImageList = this.imageList;
            this.imageListView.StateImageList = this.imageList;
            this.imageListView.TileSize = new System.Drawing.Size(100, 100);
            this.imageListView.UseCompatibleStateImageBehavior = false;
            // 
            // imageMenuStrip
            // 
            this.imageMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageReplaceStripMenu,
            this.imageSaveStripMenu});
            this.imageMenuStrip.Name = "contextMenuStrip1";
            resources.ApplyResources(this.imageMenuStrip, "imageMenuStrip");
            // 
            // imageReplaceStripMenu
            // 
            this.imageReplaceStripMenu.Name = "imageReplaceStripMenu";
            resources.ApplyResources(this.imageReplaceStripMenu, "imageReplaceStripMenu");
            this.imageReplaceStripMenu.Click += new System.EventHandler(this.OnClickImageReplace);
            // 
            // imageSaveStripMenu
            // 
            this.imageSaveStripMenu.Name = "imageSaveStripMenu";
            resources.ApplyResources(this.imageSaveStripMenu, "imageSaveStripMenu");
            this.imageSaveStripMenu.Click += new System.EventHandler(this.OnClickImageSave);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            resources.ApplyResources(this.imageList, "imageList");
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageListView);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.imageMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ToolStripMenuItem fileStripMenu;
        private System.Windows.Forms.ToolStripMenuItem fileOpenStripMenu;
        private System.Windows.Forms.ToolStripMenuItem fileSaveStripMenu;
        private System.Windows.Forms.ToolStripMenuItem fileSaveAsStripMenu;
        private System.Windows.Forms.ToolStripMenuItem fileExitStripMenu;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ListView imageListView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip imageMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem imageReplaceStripMenu;
        private System.Windows.Forms.ToolStripMenuItem imageSaveStripMenu;
        private System.Windows.Forms.ToolStripSeparator fileStripMenuSeparatorOpenSave;
        private System.Windows.Forms.ToolStripMenuItem fileSaveAllImagesStripMenu;
        private System.Windows.Forms.ToolStripSeparator fileStripMenuSeparatorSaveExit;
    }
}

