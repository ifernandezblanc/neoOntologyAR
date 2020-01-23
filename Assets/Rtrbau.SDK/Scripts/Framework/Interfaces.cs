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
#endregion

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    #region INTERFACES
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public interface IManageable
    {
        void ActivateObject();
        void DeactivateObject();
        void CheckAvailability();
        void InputIntoReport();
        void OnElementFound();
    }

    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public interface IElementable
    {
        void DownloadElement();
        void EvaluateElement();
        void LocateElement();
        bool DestroyElement();
        void SelectFabrications();
        void CreateFabrications();
        void InputIntoReport();
        void ActivateLoadingPlate();
        void DeactivateLoadingPlate();
    }
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public interface IFabricationable
    {
        void Initialise(AssetVisualiser visualiser, RtrbauFabrication fabrication, Transform elementParent, Transform fabricationParent);
        void Scale();
        void InferFromText();
        void OnNextVisualisation();
        Renderer GetRenderer();
    }

    public interface ILoadable
    {
        string URL();
        string FilePath();
        string EventName();
    }

    public interface IVisualisable
    {
        void LocateIt();
        void ActivateIt();
        void DestroyIt();
        void ModifyMaterial(Material material);
    }

    public interface IRecordable
    {
        void ActivateRecords();
        void DeactivateRecords();
    }

    public interface INominatable
    {
        void ActivateNominates();
        void DeactivateNominates();
        bool NominatesNewReportElement(bool reportedForced);
    }
    #endregion INTERFACES
}
