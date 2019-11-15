/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 08/07/2019
==============================================================================*/

#region NAMESPACES
/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#endregion


namespace Rtrbau
{
    /// <summary>
    /// Static members for a game run. Allocated to rtrbauer.
    /// Declare in here for simplification purposes.
    /// </summary>
    #region STATIC_CLASSES
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class User
    {
        #region MEMBERS
        [SerializeField]
        public string name;
        [SerializeField]
        public RtrbauElementType procedure;
        [SerializeField]
        private Dictionary<RtrbauComprehensiveness, bool> comprehension;
        [SerializeField]
        private Dictionary<RtrbauDescriptiveness, bool> description;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public User()
        {
            comprehension = new Dictionary<RtrbauComprehensiveness, bool>();
            description = new Dictionary<RtrbauDescriptiveness, bool>();

            foreach (RtrbauComprehensiveness comprehensiveness in Enum.GetValues(typeof(RtrbauComprehensiveness)))
            {
                comprehension.Add(comprehensiveness, false);
            }

            foreach (RtrbauDescriptiveness descriptiveness in Enum.GetValues(typeof(RtrbauDescriptiveness)))
            {
                description.Add(descriptiveness, false);
            }

        }
        #endregion CONSTRUCTORS

        #region METHODS
        public void AssignComprehensiveness(RtrbauComprehensiveness comprehensiveness, bool availability)
        {
            comprehension[comprehensiveness] = availability;
        }

        public List<RtrbauComprehensiveness> Comprehensiveness()
        {
            List<RtrbauComprehensiveness> available = new List<RtrbauComprehensiveness>();

            foreach (KeyValuePair<RtrbauComprehensiveness, bool> comprehensiveness in comprehension)
            {
                if (comprehensiveness.Value == true) { available.Add(comprehensiveness.Key); }
            }

            return available;
        }

        public void AssignDescriptiveness(RtrbauDescriptiveness descriptiveness, bool availability)
        {
            description[descriptiveness] = availability;
        }

        public List<RtrbauDescriptiveness> Descriptivenesses()
        {
            List<RtrbauDescriptiveness> available = new List<RtrbauDescriptiveness>();

            foreach (KeyValuePair<RtrbauDescriptiveness, bool> descriptiveness in description)
            {
                if (descriptiveness.Value == true) { available.Add(descriptiveness.Key); }
            }

            return available;
        }
        #endregion METHODS
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    [Serializable]
    public class Environment
    {
        #region MEMBERS
        [SerializeField]
        private Dictionary<RtrbauSense, bool> senses;
        #endregion MEMBERS

        #region CONSTRUCTORS
        public Environment()
        {
            senses = new Dictionary<RtrbauSense, bool>();

            foreach (RtrbauSense sense in Enum.GetValues(typeof(RtrbauSense)))
            {
                senses.Add(sense, false);
            }
        }
        #endregion CONSTRUCTORS

        #region METHODS
        public void AssignSense(RtrbauSense sense, bool availability)
        {
            senses[sense] = availability;
        }

        public List<RtrbauSense> Senses()
        {
            List<RtrbauSense> available = new List<RtrbauSense>();

            foreach (KeyValuePair<RtrbauSense, bool> sense in senses)
            {
                if (sense.Value == true) { available.Add(sense.Key); }
            }

            return available;
        }
        #endregion METHODS
    }
    #endregion STATIC_CLASSES
}
