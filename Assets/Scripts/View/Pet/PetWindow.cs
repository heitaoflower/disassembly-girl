using UnityEngine;
using Manager;
using Utils;
using System.Collections.Generic;
using Prototype;

namespace View.Pet
{
    [View(prefabPath = "UI/Pet/PetWindow", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class PetWindow : BaseView
    {
        public UIButton closeBtn = null;

        public UIGrid petGrid = null;

        private IList<PetItemRenderer> petItems = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;

            EventBox.Add(CustomEvent.PET_LEVEL_UP, OnPetLevelUp);
            EventBox.Add(CustomEvent.USER_APPLY_PET, OnUserApplyPet);
        }

        public override void Hide()
        {
            base.Hide();

            petItems.Clear();

            EventBox.RemoveAll(this);
        }

        void Awake()
        {
            ShowPets();
        }

        private void ShowPets()
        {
            petItems = new List<PetItemRenderer>();

            foreach (PetData petData in UserManager.GetInstance().user.petDatas)
            {
                PetItemRenderer itemRenderer = ResourceUtils.GetComponent<PetItemRenderer>(PetItemRenderer.PREFAB_PATH);
                itemRenderer.Initialize(petData);
                itemRenderer.transform.SetParent(petGrid.transform);
                petItems.Add(itemRenderer);
            }

            petGrid.Reposition();
        }

        private void OnClick(GameObject go)
        {
            if (go == closeBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_CLOSE.AsAudioClip());

                LayerManager.GetInstance().RemovePopUpView(this);
            }
        }

        private void OnUserApplyPet(object data)
        {
            PetData petData = data as PetData;

            UserManager.GetInstance().user.activePetID = petData.id;

            for (int index = 0; index < petItems.Count; index++)
            {
                petItems[index].Refresh();
            }
        }

        private void OnPetLevelUp(object data)
        {
            PetData petData = data as PetData;

            for (int index = 0; index < petItems.Count; index++)
            {
                if (petItems[index].data.id == petData.id)
                { 
                    petItems[index].Refresh();
                    break;
                }
            }
        }
    }
}