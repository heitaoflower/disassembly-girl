using Utils;
using UnityEngine;
using Entity;
using Manager;
using View.Guide;
using System.Collections;
using System;
using Prototype;
using View.Weapon;
using View.Dungeon;

namespace Guide
{
    public enum GuideScriptID : int
    {
        G01 = 1,
    }

    public class GuideScript01 : AbstractGuideScript
    {
        private Task task = null;

        public GuideScript01()
        {

        }

        public override void Start()
        {
            task = new Task(Action1());
        }

        private IEnumerator Action1()
        {
            SystemManager.GetInstance().sceneTouchEnabled = false;

            LayerManager.GetInstance().AddPopUpView<GuideWindow>();

            GirlEntity2D girl = GameObject.FindGameObjectWithTag(TagDefines.TAG_GIRL).GetComponent<GirlEntity2D>();

            girl.Play(AnimationDefs.Sleep.ToString().ToLower());

            LayerMaskDefines.GUIDE.ToLayer(girl.gameObject);

            yield return new WaitForSeconds(4f);

            Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> AnimationCompleted = null;
            AnimationCompleted = (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) =>
            {
                girl.componentsHolder.GetComponent(ComponentDefs.Body).animator.AnimationCompleted -= AnimationCompleted;

                task.Stop();

                task = new Task(Action2());

                GuideWindow guideWin = LayerManager.GetInstance().GetPopUpView<GuideWindow>();
                guideWin.FadeOut();
            };

            girl.componentsHolder.GetComponent(ComponentDefs.Body).animator.AnimationCompleted += AnimationCompleted;
            girl.componentsHolder.GetComponent(ComponentDefs.Body).Play(AnimationDefs.Wake.ToString().ToLower());
        }

        private IEnumerator Action2()
        {
            yield return new WaitForSeconds(1f);

            GirlEntity2D girl = GameObject.FindGameObjectWithTag(TagDefines.TAG_GIRL).GetComponent<GirlEntity2D>();
            LayerMaskDefines.GIRL.ToLayer(girl.gameObject);

            girl.Flip();

            yield return new WaitForSeconds(1f);

            girl.Flip();

            yield return new WaitForSeconds(1f);

            FunctionData functionData = new FunctionData();
            functionData.id = (int)FunctionType.Weapon;

            EventBox.Send(CustomEvent.HOME_SHOW_FUNCTION, functionData);

            yield return new WaitForSeconds(1.0f);
 
            Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> AnimationCompleted = null;
            AnimationCompleted = (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) =>
            {
                girl.componentsHolder.GetComponent(ComponentDefs.Body).animator.AnimationCompleted -= AnimationCompleted;

                task.Stop();

                task = new Task(Action3());
            };

            girl.componentsHolder.GetComponent(ComponentDefs.Body).animator.AnimationCompleted += AnimationCompleted;
            girl.componentsHolder.GetComponent(ComponentDefs.Body).Play(AnimationDefs.Puzzle.ToString().ToLower());
        }

        private IEnumerator Action3()
        {
            yield return new WaitForSeconds(1.0f);

            SystemManager.GetInstance().sceneTouchEnabled = true;

            EventBox.EventBoxHandler OnWindowClose = null;
            OnWindowClose = delegate (object data)
            {
                if (data == typeof(WeaponWindow))
                {
                    WeaponData weaponData = UserManager.GetInstance().user.GetActiveWeapon();

                    if (weaponData != null)
                    {
                        EventBox.Remove(CustomEvent.WINDOW_CLOSE, OnWindowClose);

                        task.Stop();

                        task = new Task(Action4());
                    }
                }                         
            };

            EventBox.Add(CustomEvent.WINDOW_CLOSE, OnWindowClose);
        }

        private IEnumerator Action4()
        {
            SystemManager.GetInstance().sceneTouchEnabled = false;

            UserManager.GetInstance().OpenFunction(FunctionType.Weapon);
            UserManager.GetInstance().OpenFunction(FunctionType.Dungeon);

            FunctionData functionData = UserManager.GetInstance().GetFunction(FunctionType.Dungeon);
            EventBox.Send(CustomEvent.HOME_SHOW_FUNCTION, functionData);

            EventBox.EventBoxHandler OnWindowOpen = null;
            OnWindowOpen = delegate (object data)
            {
                if (data == typeof(DungeonSelectWindow))
                {
                    EventBox.Remove(CustomEvent.WINDOW_OPEN, OnWindowOpen);

                    SystemManager.GetInstance().sceneTouchEnabled = true;
                    GuideManager.GetInstance().FinishGuide(GuideScriptID.G01);

                    task.Stop();
                }
            };

            EventBox.Add(CustomEvent.WINDOW_OPEN, OnWindowOpen);

            yield return new WaitForSeconds(1.0f);

            GirlEntity2D girl = GameObject.FindGameObjectWithTag(TagDefines.TAG_GIRL).GetComponent<GirlEntity2D>();
            girl.weaponLauncher.Unload();

            girl.componentsHolder.GetComponent(ComponentDefs.Body).Play(AnimationDefs.Amaze.ToString().ToLower());
        }
    }
}