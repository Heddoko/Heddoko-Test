using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Brainpack_Emulator.DataTransmission.FileSelection;

namespace Brainpack_Emulator
{
    [Activity(Label = "Brainpack_Emulator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    { 
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.button1);
            TextView vConsole = FindViewById<TextView>(Resource.Id.textView1);
            
            button.Click += (vSender, vEventdata) =>
            {
                var vIntent = new Intent(this, typeof (FileSelectionActivity));

            };

        }
        
    }
}

