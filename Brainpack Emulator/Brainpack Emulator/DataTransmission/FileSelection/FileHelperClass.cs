// /**
// * @file FileHelperClass.cs
// * @brief Contains the 
// * @author Mohammed Haider( 
// * @date 09 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System.IO;

namespace Brainpack_Emulator.DataTransmission.FileSelection
{
    public static class FileHelperClass
    {
        /// <summary>
        /// Is the passed in path a directory? false if a file
        /// </summary>
        /// <param name="vPath"></param>
        /// <returns></returns>
        public static bool IsDirectory(string vPath)
        { 
            bool vIsDir = (File.GetAttributes(vPath) & FileAttributes.Directory)
                == FileAttributes.Directory;
            return vIsDir;
        } 
    }
}