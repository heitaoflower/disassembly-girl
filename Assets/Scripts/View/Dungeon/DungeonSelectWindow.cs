using UnityEngine;
using Manager;
using System.Collections.Generic;
using Utils;

namespace View.Dungeon
{
    [View(prefabPath = "UI/Dungeon/DungeonSelectWindow", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class DungeonSelectWindow : BaseView
    {
        public UIButton closeBtn = null;

        public UIGrid grid = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;

        }

        void Awake()
        {
            Refresh();
        }

        private void Refresh()
        {
            foreach (DisassemblygirlDungeonConfig config in ConfigMgr.GetInstance().DisassemblygirlDungeon.configs.Values)
            {
                DungeonItemRenderer itemRenderer = ResourceUtils.GetComponent<DungeonItemRenderer>(DungeonItemRenderer.GetPrefabPath());
                itemRenderer.Initialize(new KeyValuePair<int, string>(config.id, config.iconID));
                itemRenderer.transform.SetParent(grid.transform);
            }

            grid.Reposition();
        }

        private void OnClick(GameObject go)
        {
            if (go == closeBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_CLOSE.AsAudioClip());

                LayerManager.GetInstance().RemovePopUpView(this);
            }
        }
    }
}