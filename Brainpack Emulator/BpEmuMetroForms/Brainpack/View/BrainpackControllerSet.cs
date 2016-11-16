// /**
// * @file BrainpackControllerSet.cs
// * @brief Contains the BrainpackControllerSet class
// * @author Mohammed Haider( mohammed @heddoko.com)
// * @date November 2016
// * Copyright Heddoko(TM) 2016,  all rights reserved
// */

using System.Collections.Generic;
using MetroFramework.Controls;

namespace BpEmuMetroForms.Brainpack.View
{
    /// <summary>
    /// a set of unique brainpacks and their associated tiles
    /// </summary>
    public class BrainpackControllerSet
    {

        public Dictionary<BrainpackController, MetroTile> Brainpacks = new Dictionary<BrainpackController, MetroTile>();
        public System.Windows.Forms.FlowLayoutPanel ParentGrid;

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