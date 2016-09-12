// /**
// * @file FileListAdapter.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 09 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Android.Content;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;

namespace Brainpack_Emulator.DataTransmission.FileSelection
{
    public class FileListAdapter : ArrayAdapter<FileSystemInfo>
    {
        private readonly Context mContext;

        public FileListAdapter(Context vContext, IList<FileSystemInfo> vFileSystemInfos) : base(vContext, Resource.Layout.FilePickerListItem, Android.Resource.Id.Text1, vFileSystemInfos)
        {

        }

        /// <summary>
        /// add directory contents 
        /// </summary>
        /// <param name="vDirectoryContents"></param>
        public void AddDirectoryContents(IEnumerable<FileSystemInfo> vDirectoryContents)
        {
            Clear();
            if (vDirectoryContents.Any())
            {
#if __ANDROID_11__
                AddAll(vDirectoryContents.ToArray());
#else
                lock (this)
                {
                    foreach (var vFsi in vDirectoryContents)
                    {
                        Add(vFsi);
                    }
                }
#endif
                NotifyDataSetChanged();
            }

            else
            {
                NotifyDataSetInvalidated();
            }
        }

        public override View GetView(int vPosition, View convertView, ViewGroup parent)
        {
            var vFileSystemInfo = GetItem(vPosition);

            FileListRowViewHolder fda;
            FileListRowViewHolder vViewHolder;
            View vRow;
            if (convertView == null)
            {
                LayoutInflater vInflation;

                vRow = LayoutInflater.From(mContext).Inflate(Resource.Layout.FilePickerListItem, parent, false);
                vViewHolder = new FileListRowViewHolder(vRow.FindViewById<TextView>(Resource.Id.file_picker_text), vRow.FindViewById<ImageView>(Resource.Id.file_picker_image));
                vRow.Tag = vViewHolder;

            }
            else
            {
                vRow = convertView;
                vViewHolder = (FileListRowViewHolder)vRow.Tag;
            }
            bool vIsDir = FileHelperClass.IsDirectory(vFileSystemInfo.FullName);
            vViewHolder.Update(vFileSystemInfo.Name, vIsDir ? Resource.Drawable.folder : Resource.Drawable.file);
            return vRow;

        }



    }
}