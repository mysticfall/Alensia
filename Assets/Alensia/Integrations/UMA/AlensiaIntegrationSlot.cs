using UMA;
using UnityEngine;
using Zenject;

namespace Alensia.Integrations.UMA
{
    public class AlensiaIntegrationSlot : MonoBehaviour
    {
        public void OnCharacterBegun(UMAData umaData)
        {
        }

        public void OnSlotAtlassed(
            UMAData umaData, SlotData slotData, Material material, Rect region)
        {
        }

        public void OnDnaApplied(UMAData umaData)
        {
        }

        public void OnCharacterCompleted(UMAData umaData)
        {
            var character = umaData.transform.root;
            var context = character.GetComponentInChildren<GameObjectContext>();

            if (context == null || context.Initialized) return;

            var avatar = character.GetComponent<UMAAvatarBase>();

            if (avatar != null)
            {
                context.Container
                    .BindInterfacesAndSelfTo(avatar.GetType())
                    .FromInstance(avatar)
                    .AsSingle();
            }

            context.Run();
        }
    }
}