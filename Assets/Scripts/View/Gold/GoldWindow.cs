using UnityEngine;
using Manager;
using Utils;
using Orange.StateKit;
using System.Collections.Generic;
using Prototype;
using System;

namespace View.Gold
{
    [View(prefabPath = "UI/Gold/GoldWindow", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class GoldWindow : BaseView
    {
        public UIButton closeBtn = null;

        public UIButton getBtn = null;

        public UIButton raiseBtn = null;

        public UIButton pressBtn = null;

        public UISprite startImg = null;

        public UISprite rpImg = null;

        public UISprite gpImg = null;

        public UILabel valueLab = null;

        public UIGrid itemGrid = null;

        public IDictionary<SlotIndex, GoldItemRenderer> goldItems = null;

        private StateMachine<GoldWindow> machine = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;
        }

        public override void Hide()
        {
            base.Hide();

            goldItems.Clear();

            EventBox.RemoveAll(this);
        }

        void Awake()
        {
            goldItems = new Dictionary<SlotIndex, GoldItemRenderer>();

            foreach (SlotIndex index in Enum.GetValues(typeof(SlotIndex)))
            {
                if (index != SlotIndex.None && index != SlotIndex.Count)
                {
                    GoldData goldData = new GoldData();
                    goldData.index = index;
                    GoldItemRenderer itemRenderer = ResourceUtils.GetComponent<GoldItemRenderer>(GoldItemRenderer.PREFAB_PATH);
                    itemRenderer.Initialize(goldData);
                    itemRenderer.transform.SetParent(itemGrid.transform);
                    goldItems.Add(index, itemRenderer);
                }
            }

            itemGrid.Reposition();
            
            machine = new StateMachine<GoldWindow>(this, new IdleState());
            machine.AddState(new StartState());
            machine.AddState(new StopState());

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

    enum StateTypes
    {
        Idle,
        Start,
        Stop
    }

    class IdleState : State<GoldWindow>
    {        
        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().valueLab.text = 0.ToString();
            GetContext().startImg.GetComponent<UISpriteAnimation>().Pause();
            GetContext().rpImg.gameObject.SetActive(true);
            GetContext().raiseBtn.gameObject.SetActive(true);
            GetContext().closeBtn.isEnabled = true;

            UIEventListener.Get(GetContext().startImg.gameObject).onClick += OnClick;
            UIEventListener.Get(GetContext().raiseBtn.gameObject).onClick += OnClick;

            foreach (GoldItemRenderer itemRenderer in GetContext().goldItems.Values)
            {
                itemRenderer.Refresh();
            }
        }

        public override void End()
        {
            base.End();
            
            GetContext().closeBtn.isEnabled = false;
            GetContext().raiseBtn.gameObject.SetActive(false);
            UIEventListener.Get(GetContext().startImg.gameObject).onClick -= OnClick;
            UIEventListener.Get(GetContext().raiseBtn.gameObject).onClick -= OnClick;
        }

        public override void Update(float deltaTime)
        {

        }

        public override string GetName()
        {
            return StateTypes.Idle.ToString();
        }

        private void OnClick(GameObject go)
        {
            if (go == GetContext().raiseBtn.gameObject)
            {
                int currentRP = int.Parse(GetContext().valueLab.text);
                if (UserManager.GetInstance().GetCurrentRP() > currentRP + 100)
                {
                    currentRP += 100;
                    GetContext().valueLab.text = currentRP.ToString();
                }
            }
            else if (go == GetContext().startImg.gameObject)
            {
                int currentRP = int.Parse(GetContext().valueLab.text);
                if (currentRP > 0)
                {
                    if (UserManager.GetInstance().CostRP(currentRP))
                    {
                        EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.GoldBet, currentRP));

                        GetContext().startImg.gameObject.GetComponent<UISpriteAnimation>().Play();

                        GetStateMachine().ChangeState(StateTypes.Start.ToString());
                    }
                }
            }
        }
    }

    class StartState : State<GoldWindow>
    {
        private IList<Task> shuffleTasks = null;

        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().pressBtn.gameObject.SetActive(true);
            UIEventListener.Get(GetContext().pressBtn.gameObject).onClick += OnClick;

            Shuffle();
        }

        public override void End()
        {
            base.End();

            GetContext().rpImg.gameObject.SetActive(false);
            GetContext().pressBtn.gameObject.SetActive(false);
            UIEventListener.Get(GetContext().pressBtn.gameObject).onClick -= OnClick;
        }

        public override void Update(float deltaTime)
        {

        }

        public override string GetName()
        {
            return StateTypes.Start.ToString();
        }

        private void OnClick(GameObject go)
        {
            if (go == GetContext().pressBtn.gameObject)
            {
                for (int index = 0; index < shuffleTasks.Count; index++)
                {
                    if (shuffleTasks[index].Running)
                    {
                        shuffleTasks[index].Stop();
                        shuffleTasks.RemoveAt(index);
                        break;
                    }
                }

                if (shuffleTasks.Count == 0)
                {
                    GetStateMachine().ChangeState(StateTypes.Stop.ToString());
                }
            }
        }

        private void Shuffle()
        {
            shuffleTasks = new List<Task>();

            foreach (GoldItemRenderer itemRenderer in GetContext().goldItems.Values)
            {
                shuffleTasks.Add(new Task(itemRenderer.Shuffle()));
            }
        }
    }

    class StopState : State<GoldWindow>
    {
        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().getBtn.gameObject.SetActive(true);
            GetContext().gpImg.gameObject.SetActive(true);
            UIEventListener.Get(GetContext().getBtn.gameObject).onClick += OnClick;

            Reward();
        }

        public override void End()
        {
            base.End();

            GetContext().getBtn.gameObject.SetActive(false);
            GetContext().gpImg.gameObject.SetActive(false);
            GetContext().goldItems[SlotIndex.I].winImg.gameObject.SetActive(false);
            GetContext().goldItems[SlotIndex.II].winImg.gameObject.SetActive(false);
            GetContext().goldItems[SlotIndex.III].winImg.gameObject.SetActive(false);

            UIEventListener.Get(GetContext().getBtn.gameObject).onClick -= OnClick;
        }

        public override void Update(float deltaTime)
        {

        }

        public override string GetName()
        {
            return StateTypes.Stop.ToString();
        }
        private void OnClick(GameObject go)
        {
            if (go == GetContext().getBtn.gameObject)
            {
                UserManager.GetInstance().AddGP(int.Parse(GetContext().valueLab.text));

                GetStateMachine().ChangeState(StateTypes.Idle.ToString());
            }
        }

        private void Reward()
        {
            GoldData goldDataI = GetContext().goldItems[SlotIndex.I].data;
            GoldData goldDataII = GetContext().goldItems[SlotIndex.II].data;
            GoldData goldDataIII = GetContext().goldItems[SlotIndex.III].data;

            int value = 0;
            if (goldDataI.value == goldDataII.value)
            {
                if (goldDataII.value == goldDataIII.value)
                {
                    GetContext().goldItems[SlotIndex.I].winImg.gameObject.SetActive(true);
                    GetContext().goldItems[SlotIndex.II].winImg.gameObject.SetActive(true);
                    GetContext().goldItems[SlotIndex.III].winImg.gameObject.SetActive(true);

                    value = goldDataI.value * 2;

                    EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.GoldHit, 3));
                }
                else
                {
                    GetContext().goldItems[SlotIndex.I].winImg.gameObject.SetActive(true);
                    GetContext().goldItems[SlotIndex.II].winImg.gameObject.SetActive(true);

                    value = goldDataI.value;
                }
            }
            else
            {
                if (goldDataII.value == goldDataIII.value)
                {
                    GetContext().goldItems[SlotIndex.II].winImg.gameObject.SetActive(true);
                    GetContext().goldItems[SlotIndex.III].winImg.gameObject.SetActive(true);

                    value = goldDataIII.value;
                }
                else
                {
                    if (goldDataI.value == goldDataIII.value)
                    {
                        GetContext().goldItems[SlotIndex.I].winImg.gameObject.SetActive(true);
                        GetContext().goldItems[SlotIndex.III].winImg.gameObject.SetActive(true);

                        value = goldDataIII.value;
                    }
                    else
                    {
                        value = 1;
                    }
                }
            }

            int currentRP = int.Parse(GetContext().valueLab.text);
            int currentGP = (int)(currentRP * 0.05 * value);
            GetContext().valueLab.text = currentGP.ToString();
        }
    }
}