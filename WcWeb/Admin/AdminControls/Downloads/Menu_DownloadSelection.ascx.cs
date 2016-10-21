using System;
using System.IO;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.Downloads
{
    public partial class Menu_DownloadSelection : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                PopulateTree();
        }

        public string Title = "Download selector";
        private void PopulateTree()
        {
            //Populate the tree based on the subfolders of the specified VirtualImageRoot
            string mappedRoot = Download.BaseDirectory;
            if(! Directory.Exists(mappedRoot))
                Directory.CreateDirectory(mappedRoot);

            DirectoryInfo rootFolder = new DirectoryInfo(mappedRoot);//app name
            
            TreeNode root = AddNodeAndDescendents(rootFolder, null);

            //Add the root to the TreeView
            tvDirectory.Nodes.Add(root);
        }
        private TreeNode AddNodeAndDescendents(DirectoryInfo folder, TreeNode parentNode)
        {
            string mappedRoot = Download.BaseDirectory;

            //Add the TreeNode, displaying the folder's name and storing the full path to the folder as the value...
            string mappedPath = null;

            if(parentNode == null)
                mappedPath = mappedRoot;
            else
                mappedPath = string.Format("{0}/{1}/", parentNode.Value, folder.Name);

            TreeNode node = new TreeNode(folder.Name, mappedPath);

            //Recurse through this folder's subfolders
            DirectoryInfo[] subFolders = folder.GetDirectories();
            foreach(DirectoryInfo subFolder in subFolders)
            {
                TreeNode child = AddNodeAndDescendents(subFolder, node);
                node.ChildNodes.Add(child);
            }

            return node;
        }

        protected void tvDirectory_SelectedNodeChanged(object sender, EventArgs e)
        {
            //Refresh the DataList whenever a new node is selected
            //e.Node
            //DisplayPicturesInFolder(PictureTree.SelectedValue)
        }
        private void DisplayFilesInFolder(string folderPath)
        {
            //Get information about the files in the specified folder
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            FileInfo[] fileList = folder.GetFiles();

            //PicturesInFolder.DataSource = fileList;
            //PicturesInFolder.DataBind();
        }
}
}       