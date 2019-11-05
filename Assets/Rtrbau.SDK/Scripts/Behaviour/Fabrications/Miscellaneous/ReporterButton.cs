/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2019 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2019 Cranfield University. All Rights Reserved.
Copyright (c) 2019 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 25/07/2019
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
#endregion


namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public class ReporterButton : MonoBehaviour
    {

        #region CLASS_VARIABLES
        #endregion CLASS_VARIABLES

        #region GAMEOBJECT_PREFABS
        #endregion GAMEOBJECT_PREFABS

        #region CLASS_METHODS
        public void SendReport()
        {
            // Send report
            Reporter.instance.SendReport();
            // Reinitialise report
            Reporter.instance.ReinitialiseReport();
            // Reinitialise visualiser
            Rtrbauer.instance.ReloadVisualiser();
            // Reinitialise paneller at ontologies level
            string ontologiesURI = Rtrbauer.instance.ontology.ontologyURI.AbsoluteUri + "/" + "ontologies#ontologies";
            OntologyEntity ontologies = new OntologyEntity(ontologiesURI);
            PanellerEvents.TriggerEvent("LoadOperationOntologies", ontologies);
        }
        #endregion CLASS_METHODS
    }
}