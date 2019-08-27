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

        public void Initialise(OntologyEntity entity)
        {
            fabricatedEntity = entity;

            // To comply with ontologies visualisation which do not have name, only ontology fields.
            if (fabricatedEntity.name != "")
            {
                label.text = fabricatedEntity.name;
            }
            else
            {
                label.text = fabricatedEntity.ontology;
            }
        }

        public void OnClicked()
        {
            PanellerEvents.TriggerEvent(fabricatedEntity.Entity(), fabricatedEntity);
        }
    }
}