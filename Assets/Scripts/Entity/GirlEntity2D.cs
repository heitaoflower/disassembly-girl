using Prototype;
using UnityEngine;
using Utils;
using Manager;
using Orange.StateKit;
using Entity.AI;
using System.Collections.Generic;
namespace Entity
{
    public class GirlEntity2D : BaseEntity2D
    {
        [HideInInspector]
        public StateMachine<GirlEntity2D> machine = null;
        [HideInInspector]
        public AbstractLauncher weaponLauncher = null;
        [HideInInspector]
        public AbstractLauncher skillLauncher = null;

        void Start()
        {
          
        }

        protected override void Update()
        {
            if (machine != null)
            {
                machine.Update(Time.deltaTime);
            }
        }

        public override void Initialize()
        {
            machine = new StateMachine<GirlEntity2D>(this, new GirlIdleState());
            machine.AddState(new GirlDeadState());

            EventBox.Add(CustomEvent.USER_APPLY_WEAPON, OnUserApplyWeapon);
            EventBox.Add(CustomEvent.WEAPON_LEVEL_UP, OnWeaponLevelUp);

            ConfigWeaponAndSkill();           
        }

        private void ConfigWeaponAndSkill()
        {
            skillLauncher = new SkillLauncher();
            weaponLauncher = new WeaponLauncher();

            componentsHolder.GetComponent(ComponentDefs.Body).animator.AnimationCompleted += delegate (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
            {
                componentsHolder.GetComponent(ComponentDefs.Body).Play(AnimationDefs.Idle.ToString().ToLower());

                weaponLauncher.Load(UserManager.GetInstance().user.GetActiveWeapon());
            };

            componentsHolder.GetComponent(ComponentDefs.Body).animator.AnimationEventTriggered += delegate (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameIndex)
            {
                tk2dSpriteAnimationFrame frame = clip.GetFrame(frameIndex);

                if (frame.eventInfo == AnimationTriggerDefs.WeaponApply.ToString())
                {
                    weaponLauncher.LateApply();
                }
            };
        }

        protected override void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.layer == LayerMaskDefines.MONSTER.id && machine.currentState.GetName() != StateTypes.Dead.ToString())
            {
                machine.ChangeState(StateTypes.Dead.ToString());
            }
        }

        public override void OnMissileHit(BaseEntity2D sender, MissileData missile, Vector2 point)
        {
            if (machine.currentState.GetName() != StateTypes.Dead.ToString())
            {
                machine.ChangeState(StateTypes.Dead.ToString());
            }         
        }

        public override void BindData(object data)
        {
            GirlData girl = data as GirlData;

            this.data = girl;

            weaponLauncher.Bind(this);

            skillLauncher.Bind(this);

            Flip();
        }

        public override bool ImmuneEffector(EffectorData effectorData)
        {
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();

            weaponLauncher.Unload();

            skillLauncher.Unload();
          
            EventBox.RemoveAll(this);
        }

        public void ApplySkill(Vector3 position, SkillData skillData)
        {
            EntityComponent component = componentsHolder.GetComponent(ComponentDefs.Body);

            if (!component.IsPlaying(AnimationDefs.Idle.ToString().ToLower()))
            {
                return;
            }

            if (skillData.state == PrototypeState.Frozen)
            {
                return;
            }
      
            weaponLauncher.Unload();
           
            if (position.x > transform.position.x)
            {
                if (this.componentsHolder.transform.localScale.x > 0f)
                {
                    Flip();
                }
            }
            else
            {
                if (this.componentsHolder.transform.localScale.x < 0f)
                {
                    Flip();
                }
            }

            component.Play(AnimationDefs.Focus.ToString().ToLower());

            skillLauncher.Load(skillData);

            skillLauncher.Apply(position);

            EventBox.Send(CustomEvent.USER_APPLY_SKILL, skillData);

            EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.GirlSkill, 1));
            
        }

        public void ApplyWeapon(Vector3 position)
        {
            EntityComponent component = componentsHolder.GetComponent(ComponentDefs.Body);

            if (!component.IsPlaying(AnimationDefs.Idle.ToString().ToLower()))
            {
                return;
            }

            if (position.x > transform.position.x)
            {
                if (this.componentsHolder.transform.localScale.x > 0f)
                {
                    Flip();
                }
            }
            else
            {
                if (this.componentsHolder.transform.localScale.x < 0f)
                {
                    Flip();
                }
            }

            weaponLauncher.Apply(position);

            EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.GirlAttack, 1));
        }

        private void OnWeaponLevelUp(object data)
        {
            WeaponData weaponData = data as WeaponData;

            EntityComponent component = componentsHolder.GetComponent(ComponentDefs.Body);

            if ((UserManager.GetInstance().user.GetActiveWeapon() != null) && weaponData.id == UserManager.GetInstance().user.GetActiveWeapon().id)
            {
                weaponLauncher.Unload();

                weaponLauncher.Load(UserManager.GetInstance().user.GetActiveWeapon());

                component.PlayFromFrame(0);
            }            
        }

        private void OnUserApplyWeapon(object data)
        {
            EntityComponent component = componentsHolder.GetComponent(ComponentDefs.Body);

            if (component.IsPlaying(AnimationDefs.Idle.ToString().ToLower()))
            {
                weaponLauncher.Unload();

                weaponLauncher.Load(UserManager.GetInstance().user.GetActiveWeapon());

                component.PlayFromFrame(0);
            }
        }
    }
}