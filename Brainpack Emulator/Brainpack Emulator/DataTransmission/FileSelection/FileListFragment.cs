// /**
// * @file FileListFragment.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 09 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Brainpack_Emulator.DataTransmission.FileSelection
{
    public class FileListFragment : ListFragment
    {
        public static readonly string DefaultInitialDirectory = "/";
        private FileListAdapter mAdapter;
        private DirectoryInfo mDirectory;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mAdapter = new FileListAdapter(Activity, new FileSystemInfo[0]);
            ListAdapter = mAdapter;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var vFileSystemInfo = mAdapter.GetItem(position);
            bool visDir = FileHelperClass.IsDirectory(vFileSystemInfo.FullName);
            if (!visDir)
            {
                // Do something with the file.  In this case we just pop some toast.
                Log.Verbose("FileListFragment", "The file {0} was clicked.", vFileSystemInfo.FullName);
                Toast.MakeText(Activity, "You selected file " + vFileSystemInfo.FullName, ToastLength.Short).Show();
            }
            else
            {
                // Dig into this directory, and display it's contents
                RefreshFilesList(vFileSystemInfo.FullName);
            }
            base.OnListItemClick(l, v, position, id);
        }
        public override void OnResume()
        {
            base.OnResume();
            RefreshFilesList(DefaultInitialDirectory);
        }

        public void RefreshFilesList(string vDir)
        {
            IList<FileSystemInfo> vVisibleItems = new List<FileSystemInfo>();
            var vDirInfo = new DirectoryInfo(vDir);
            try
            {
                foreach (
                    var vItem in vDirInfo.GetFileSystemInfos().
                    Where(vFileInfo => (!vFileInfo.Attributes.HasFlag(FileAttributes.Hidden))))
                {
                    vVisibleItems.Add(vItem);
                }
            }
            catch(Exception vEx)
            {
                Log.Error("FileListFragment", " Couldn't access directoy " + mDirectory.FullName + ";" + vEx);
                Toast.MakeText(Activity, "Problem retrieving contents of " + mDirectory, ToastLength.Long).Show();
                return;
            }
            mDirectory = vDirInfo;

            mAdapter.AddDirectoryContents(vVisibleItems);
            ListView.RefreshDrawableState();
            Log.Verbose("FileListFragment", "Displaying the contents of directory {0}.", mDirectory);

        }
    }
}