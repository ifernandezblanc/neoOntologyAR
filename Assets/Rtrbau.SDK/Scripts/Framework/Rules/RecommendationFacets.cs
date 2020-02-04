/*==============================================================================
Author: Iñigo Fernandez del Amo - 2019
Email: inigofernandezdelamo@outlook.com
License: This code has been developed for research and demonstration purposes.

Copyright (c) 2020 Iñigo Fernandez del Amo. All Rights Reserved.
Copyright (c) 2020 Cranfield University. All Rights Reserved.
Copyright (c) 2020 Babcock International Group. All Rights Reserved.

All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.

Date: 01/02/2020
==============================================================================*/

/// <summary>
/// Describe script purpose
/// Add links when code has been inspired
/// </summary>
#region NAMESPACES
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion NAMESPACES

namespace Rtrbau
{
    /// <summary>
    /// Describe script purpose
    /// Add links when code has been inspired
    /// </summary>
    public static class RecommendationFacets
    {
        // Facet naming convention: ClassAttributerangeFacettype
        // Built to define recommendation rules generically for new fabrications that become IRecommendable to accept them
        // UPG: to standardise the method by which ontologies are assigned to rules

        #region FACETS
        #region BINARY_FACETS
        public static RecommendationFacet StateDominionBinary = new RecommendationFacet(new RecommendationRule(RtrbauRecommendationRuleType.Binary), Libraries.RecommendationStateAttributesLibrary[2], Libraries.RecommendationStateRangesLibrary[2]);
        public static RecommendationFacet StatePhenonmenonBinary = new RecommendationFacet(new RecommendationRule(RtrbauRecommendationRuleType.Binary), Libraries.RecommendationStateAttributesLibrary[3], Libraries.RecommendationStateRangesLibrary[3]);
        public static RecommendationFacet StateUnitBinary = new RecommendationFacet(new RecommendationRule(RtrbauRecommendationRuleType.Binary), Libraries.RecommendationStateAttributesLibrary[4], Libraries.RecommendationStateRangesLibrary[4]);
        #endregion BINARY_FACETS

        #region SYMMETRIC_FACETS
        #endregion SYMMETRIC_FACETS

        #region COMPONENT_FACETS
        public static RecommendationFacet StateComponentComponent = new RecommendationFacet(new RecommendationRule(RtrbauRecommendationRuleType.Component), Libraries.RecommendationStateAttributesLibrary[5], Libraries.RecommendationStateRangesLibrary[5]);
        #endregion COMPONENT_FACETS

        #region SUBSET_FACETS
        public static RecommendationFacet StateStatusSubset = new RecommendationFacet(new RecommendationRule(RtrbauRecommendationRuleType.Subset, Libraries.RecommendationStateStatusSubsetsLibrary), Libraries.RecommendationStateAttributesLibrary[1], Libraries.RecommendationStateRangesLibrary[1]);
        #endregion SUBSET_FACETS
        #endregion FACETS
    }
}
