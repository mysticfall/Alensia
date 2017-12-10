using Alensia.Core.Item;
using Alensia.Integrations.UMA;
using UnityEngine;

namespace Alensia.Demo.UMA
{
    // Temporary class to test clothings.
    public class ClothingTest : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var container = other.GetComponentInChildren<IClothingContainer>();
            var clothing = GetComponent<UMAClothing>();
            var slot = clothing.Slot.Name;

            if (container.Contains(slot))
            {
                //container.Remove(slot);
            }
            else
            {
                container.Set(clothing);
            }
        }
    }
}