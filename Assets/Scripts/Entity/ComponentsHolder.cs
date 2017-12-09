using System.Collections.Generic;
using UnityEngine;
using Effect.Component;
using System;
using Prototype;
using Orange.EffectKit;
using Manager;
using Utils;

namespace Entity
{

    public enum ComponentDefs
    {
        Unknown = 0,
        Weapon,
        Head,
        Body,
        Helmet,
        RightArm,
        LeftArm,
        Arms,
        RightHand,
        LeftHand,
        Hands,
        Armour,
        Tail,
        Foot,
        DecorationA,
        DecorationB,
        DecorationC,
        HUD,
        Count
    }

    public class EntityComponent : MonoBehaviour
    {
        public static IDictionary<ComponentDefs, KeyValuePair<Action<AttributeBox>, Action<AttributeBox>>> componentAttributeFormulas = new Dictionary<ComponentDefs, KeyValuePair<Action<AttributeBox>, Action<AttributeBox>>>()
        {
            { ComponentDefs.Armour, new KeyValuePair<Action<AttributeBox>, Action<AttributeBox>>(
                (AttributeBox attributes)=>
                {
                    attributes.AddAttribute(AttributeKeys.ANTI, AttributeSetTypes.PercentValue, -1.0f);
                    attributes.AddAttribute(AttributeKeys.DEF, AttributeSetTypes.PercentValue, -1.0f);
                    attributes.SubAttribute(AttributeKeys.SPD, AttributeSetTypes.PercentValue, -0.5f);
                }, 

                (AttributeBox attributes)=> 
                {
                    attributes.SubAttribute(AttributeKeys.ANTI, AttributeSetTypes.PercentValue, -1.0f);
                    attributes.SubAttribute(AttributeKeys.DEF, AttributeSetTypes.PercentValue, -1.0f);
                    attributes.AddAttribute(AttributeKeys.SPD, AttributeSetTypes.PercentValue, -0.5f);
                })
            }
        };

        public ComponentDefs type = ComponentDefs.Unknown;

        public tk2dSprite sprite = null;

        public tk2dSpriteAnimator animator = null;

        public EntityComponentData data = null;

        public ComponentsHolder holder = null;

        public Action OnUnloadFinished = null;

        public void Initialize(ComponentsHolder holder, ComponentDefs type, tk2dSprite sprite, tk2dSpriteAnimator animator)
        {
            this.holder = holder;
            this.type = type;
            this.sprite = sprite;
            this.animator = animator;
            this.sprite.ignoreMaterialChange = true;
        }

        public void BindData(EntityComponentData data)
        {
            this.data = data;

            AttributeBox box = this.holder.owner.data.attributeBox;

            if (componentAttributeFormulas.ContainsKey(type))
            {
                componentAttributeFormulas[type].Key(box);
            }
        }

        public void UnBindData()
        {
            this.data = null;

            AttributeBox box = this.holder.owner.data.attributeBox;

            if (componentAttributeFormulas.ContainsKey(type))
            {
                componentAttributeFormulas[type].Value(box);
            }
        }

        public void Blink()
        {
            BlinkEffectScript script = this.gameObject.GetComponent<BlinkEffectScript>();

            if (script != null)
            {
                script.Blink(0f, 0.1f);
            }
        }

        public bool IsPlaying(string name)
        {
            return animator.IsPlaying(name + "_" + StringUtils.Uncaptalize(type.ToString()));
        }

        public tk2dSpriteAnimationClip GetClipByName(string name)
        {
            return animator.GetClipByName(name + "_" + StringUtils.Uncaptalize(type.ToString()));
        }

        public void Stop()
        {
            animator.Stop();

            animator.AnimationCompleted = null;
        }

        public void Pause()
        {
            animator.Pause();
        }

        public void Resume()
        {
            animator.Resume();
        }

        public void PlayFromFrame(int frame)
        {
            if (animator != null)
            {
                animator.PlayFromFrame(frame);
            }
        }

        public bool Play(tk2dSpriteAnimationClip clip)
        {
            if (animator != null)
            {
                animator.Play(clip);

                return true;
            }

            return false;
        }

        public bool Play(string name, Action<EntityComponent> OnCompleted = null)
        {
            if (animator != null)
            {
                tk2dSpriteAnimationClip clip = animator.GetClipByName(name + "_" + StringUtils.Uncaptalize(type.ToString()));

                if (clip != null)
                {
                    if (OnCompleted != null)
                    {
                        animator.AnimationCompleted = (tk2dSpriteAnimator _animator, tk2dSpriteAnimationClip _clip) => { OnCompleted(this); };
                    }

                    animator.Play(clip);

                    return true;
                }
            }

            return false;
        }

        public void ValidateAndUpdate(float threshold)
        {
            if (data != null)
            {
                if (threshold < data.threshold)
                {
                    holder.RemoveComponent(type);

                    Play(AnimationDefs.Dead.ToString().ToLower());
                }
            }
        }

        public void Unload()
        {
            UnBindData();

            gameObject.AddComponent<ComponentDefaultEffect>().spinEnabled = false;

            LayerManager.GetInstance().AddObject(gameObject.transform);

            if (OnUnloadFinished != null)
            {
                OnUnloadFinished();
            }

            OnUnloadFinished = null;
        }
    }

    public class ComponentsHolder : MonoBehaviour
    {
        public IList<EntityComponent> components = null;

        public BaseEntity2D owner = null;

        public ComponentsHolder()
        {
            components = new List<EntityComponent>();
        }

        public void AddComponents()
        {
            foreach (ComponentDefs type in (ComponentDefs[])Enum.GetValues(typeof(ComponentDefs)))
            {
                AddComponent(type);
            }
        }

        public bool IsEmpty()
        {
            return components.Count == 0 ? true : false;
        }

        public void AddComponent(ComponentDefs type)
        {
            Transform findObject = transform.Find(type.ToString());

            if (findObject != null)
            {
                AddComponent(type, findObject.GetComponent<tk2dSprite>(), findObject.GetComponent<tk2dSpriteAnimator>());
            }
        }

        private void AddComponent(ComponentDefs type, tk2dSprite sprite, tk2dSpriteAnimator animator)
        {
            EntityComponent component = sprite.gameObject.AddComponent<EntityComponent>();
            component.Initialize(this, type, sprite, animator);
            AddComponent(component);
        }

        private void AddComponent(EntityComponent component)
        {
            components.Add(component);
        }

        public EntityComponent GetComponent(ComponentDefs type)
        {
            foreach (EntityComponent component in components)
            {
                if (component.type == type)
                {
                    return component;
                }
            }

            return null;
        }
        
        public bool HasComponent(ComponentDefs type)
        {
            foreach (EntityComponent component in components)
            {
                if (component.type == type)
                {
                    return true;
                }
            }

            return false;
        }
        
        public EntityComponent RemoveComponent(ComponentDefs type)
        {
            EntityComponent component = GetComponent(type);

            if (component != null)
            {
                components.Remove(component);
                component.Unload();
            }

            return component;
        }

        public void BindDatas(IList<EntityComponentData> datas)
        {
            foreach (EntityComponentData data in datas)
            {
                EntityComponent component = GetComponent((ComponentDefs)Enum.Parse(typeof(ComponentDefs), data.name));

                component.BindData(data);
            }
        }

        public void ValidateAndUpdate(float threshold)
        {
            for (int index = components.Count - 1; index >= 0; index--)
            {
                EntityComponent component = components[index];

                component.ValidateAndUpdate(threshold);
            }
        }
    }
}