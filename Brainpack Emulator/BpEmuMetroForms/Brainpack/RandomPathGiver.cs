// /**
// * @file RandomPathGiver.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 11 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */
namespace BpEmuMetroForms.Brainpack
{
    public class RandomPathGiver
    {
        private int mCurrentIndex=0;
        private string[] vPaths =
        {
            "45.536375, -73.622314", "45.534781, -73.618551", "45.533346, -73.615429", "45.532699, -73.615930",
            "45.531376, -73.612895", "45.531376, -73.612895", "45.530978, -73.613925", "45.529648, -73.615320",
            "45.529608, -73.616039"
        };

        public string GetNextPointInPath()
        {
            var vReturn = vPaths[mCurrentIndex++];
            if (mCurrentIndex >= vPaths.Length)
            {
                mCurrentIndex = 0;
            }

            return vReturn;
        }
    }
}