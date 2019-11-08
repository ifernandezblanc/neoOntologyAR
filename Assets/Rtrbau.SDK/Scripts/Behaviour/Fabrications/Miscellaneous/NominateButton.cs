/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 05/11/2019
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
#endregion NAMESPACES

namespace Rtrbau
{
    public class NominateButton : MonoBehaviour
    {
        #region INITIALISATION_VARIABLES
        public Action<OntologyEntity> nominate;
        public OntologyEntity individual;
        #endregion INITIALISATION_VARIABLES

        #region CLASS_VARIABLES
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        public TextMeshPro buttonText;
        public MeshRenderer seenPanel;
        public MeshRenderer reportedPanel;
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_EVENTS
        private bool buttonCreated;
        #endregion CLASS_EVENTS

        #region MONOBEHAVIOUR_METHODS
        void Start()
        {
            if (buttonText == null || seenPanel == null || reportedPanel == null)
            {
                throw new ArgumentException("NominateButton::Start: Script requires some prefabs to work.");
            }
        }
        #endregion MONOBEHAVIOUR_METHODS

        #region CLASS_METHODS
        #region PRIVATE

        #endregion PRIVATE

        #region PUBLIC
        public void Initialise(Action<OntologyEntity> nominateIndividual, OntologyEntity relationshipValue)
        {
            // Assign button variables
            nominate = nominateIndividual;
            individual = relationshipValue;
            // Show individual name the button refers to
            buttonText.text = individual.name;
            // Assign button as created
            buttonCreated = true;
        }

        public void NominateIndividual()
        {
            if (buttonCreated == true)
            {
                nominate.Invoke(individual);
            }
        }

        public void ReportMaterial(Material material)
        {
            reportedPanel.material = material;
        }

        public void SeenMaterial(Material material)
        {
            seenPanel.material = material;
        }
        #endregion PUBLIC
        #endregion CLASS_METHODS
    }
}
