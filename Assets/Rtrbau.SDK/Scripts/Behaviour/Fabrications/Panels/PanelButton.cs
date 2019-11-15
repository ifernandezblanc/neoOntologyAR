using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Rtrbau
{
    public class PanelButton : MonoBehaviour
    {
        public TextMeshProUGUI label;
        public OntologyEntity fabricatedEntity;

        // UPG: MERGE WITH BUTTONNOMINATE

        public void Initialise(OntologyEntity entity)
        {
            fabricatedEntity = entity;

            // To comply with ontologies visualisation which do not have name, only ontology fields.
            if (fabricatedEntity.Name() != null)
            {
                label.text = fabricatedEntity.Name();
            }
            else
            {
                label.text = fabricatedEntity.Ontology().Name();
            }
        }

        public void OnClicked()
        {
            PanellerEvents.TriggerEvent(fabricatedEntity.Entity(), fabricatedEntity);
        }
    }
}