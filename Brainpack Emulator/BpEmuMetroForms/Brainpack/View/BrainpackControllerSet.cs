// /**
// * @file BrainpackControllerSet.cs
// * @brief Contains the BrainpackControllerSet class
// * @author Mohammed Haider( mohammed @heddoko.com)
// * @date November 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MetroFramework.Controls;
using Newtonsoft.Json;
using ProtoBuf;

namespace BpEmuMetroForms.Brainpack.View
{
    /// <summary>
    /// a set of unique brainpacks and their associated tiles
    /// </summary>
    public class BrainpackControllerSet
    {

        public Dictionary<BrainpackController, MetroTile> Brainpacks = new Dictionary<BrainpackController, MetroTile>();
        public System.Windows.Forms.FlowLayoutPanel ParentGrid;

        private static string SerializationDir => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private const string JSonFilePath = "BpModels.json";
        /// <summary>
        /// Serializes 
        /// </summary>
        public void SerializeModels()
        {
            var vEnumerable = Brainpacks.Keys.ToList();
            List<BrainpackModel> vModels = new List<BrainpackModel>();
            vEnumerable.ForEach(vX => vModels.Add(vX.Model));

            var vPath = SerializationDir + JSonFilePath;
            JsonSerializer vSerializer = new JsonSerializer();
            StreamWriter vStreamWriter = new StreamWriter(vPath);
            vSerializer.NullValueHandling = NullValueHandling.Ignore;
            vSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            vSerializer.Formatting = Formatting.Indented;
            using (JsonWriter vJsonWriter = new JsonTextWriter(vStreamWriter))
            {
                vSerializer.Serialize(vJsonWriter, vModels);
            }
        }

        public BrainpackControllerSet(System.Windows.Forms.FlowLayoutPanel vParentGrid)
        {
            ParentGrid = vParentGrid;
        }

        public bool BrainpackExist(BrainpackController vController)
        {
            return Brainpacks.ContainsKey(vController);
        }

        public bool ConfigurationPortExist(int vPort)
        {
            foreach (var vBrainpack in Brainpacks)
            {
                if (vBrainpack.Key.Model.ConfigurationPort == vPort)
                {
                    return true;
                }
            }
            return false;
        }
        public void AddBrainpack(BrainpackController vController, MetroTile vTile)
        {
            vController.SetViewParent(ParentGrid);
            vController.DeleteRequestedEvent += RemoveBrainpack;
            Brainpacks.Add(vController, vTile);
        }

        public void RemoveBrainpack(BrainpackController vController)
        {
            Brainpacks.Remove(vController);
        }

        /// <summary>
        /// From a given serial number find the associated Brainpack controller and its model
        /// </summary>
        /// <param name="vKeyName"></param>
        /// <returns></returns>
        public BrainpackController GetController(string vKeyName)
        {
            BrainpackController vReturn = null;

            foreach (var vBrainpack in Brainpacks)
            {
                if (vBrainpack.Key.Model.SerialNum.Equals(vKeyName))
                {
                    vReturn = vBrainpack.Key;
                    break;
                }
            }
            return vReturn;

        }

         
    }
}