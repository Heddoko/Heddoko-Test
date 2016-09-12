using System; 
using Android.Widget;

namespace Brainpack_Emulator.DataTransmission.FileSelection
{

    /// <summary>
    /// this class is used to hold refrences to the views contained in a list row
    /// </summary>
    public class FileListRowViewHolder : Java.Lang.Object
    {
        public ImageView ImageView { get; private set; }
        public TextView TextView { get; private set; }
        public FileListRowViewHolder(TextView vText, ImageView vImageView)
        {
            TextView = vText;
            ImageView = vImageView;
        }

        public void Update(string vFileNmae, int vFileResourceId)
        {
            TextView.Text = vFileNmae;
            ImageView.SetImageResource(vFileResourceId);
        }
    }
}