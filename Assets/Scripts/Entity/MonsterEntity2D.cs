using Orange.StateKit;
using UnityEngine;
using Entity.AI;
using Utils;
using Prototype;
using Manager;
using Effect.Component;
using Entity.Effector;

namespace Entity
{
    public class MonsterEntity2D : BaseEntity2D 
    {
        public StateMachine<MonsterEntity2D> machine = null;
        
        public static readonly float DEFAULT_IDLE_TIME = 3;

        public static readonly float DEFAULT_RUN_TIME = 5;

        [HideInInspector]
        public float runTime = default(float);
        [HideInInspector]
        public float idleTime = default(float);
        [HideInInspector]
        public float runCounter = default(float);
        [HideInInspector]
        public float idleCounter = default(float);

        public override bool ImmuneEffector(EffectorData effectorData)
        {
            MonsterData monster = data as MonsterData;

            if (monster.immunityEffectors.IndexOf(effectorData.id) != -1)
            {
                return true;
            }

            return false;
        }

        public override void BindData(object data)
        {
            MonsterData monster = data as MonsterData;

            monster.attributeBox.OnValidateCompleted = delegate (ValidatePayload payload)
            {
                PlayComponentsDestroyEffect();

                if (payload.weaponData != null)
                {
                    if (payload.weaponEffectorsTriggered)
                    {
                        foreach (EffectorData effectorData in payload.weaponData.effectors)
                        {
                            AbstractApplierEffector.Apply(effectorData, this);
                        }
                    }

                    AbstractApplierEffector.ApplyPayload(this, payload);
                }

                if (payload.missileData != null)
                {
                    if (payload.missileEffectorsTriggered)
                    {
                        foreach (EffectorData effectorData in payload.missileData.effectors)
                        {
                            AbstractApplierEffector.Apply(effectorData, this);
                        }
                    }

                    AbstractApplierEffector.ApplyPayload(this, payload);
                }

                if (payload.skillData != null)
                {
                    if (payload.skillEffectorsTriggered)
                    {
                        foreach (EffectorData effectorData in payload.skillData.effectors)
                        {
                            AbstractApplierEffector.Apply(effectorData, this);
                        }
                    }                   
                }

                EventBox.Send(CustomEvent.DUNGEON_MONSTER_DAMAGE, new object[] { this, payload });

            };

            this.data = monster;

            groundDamping = monster.groundDamping;
            airDamping = monster.airDamping;

            if (monster.type == (int)MonsterType.Flying)
            {
                gravityDisabled = true;
            }

            componentsHolder.BindDatas(monster.componentDatas);
        }

        public override void Initialize()
        {
            machine = new StateMachine<MonsterEntity2D>(this, new MonsterComposeState());
            machine.AddState(new MonsterWanderState());
            machine.AddState(new MonsterDeadState());
            machine.AddState(new MonsterTransferState());
            machine.AddState(new MonsterAttackState());
            machine.AddState(new MonsterIdleState());
            machine.AddState(new MonsterDizzyState());
            machine.AddState(new MonsterBackDashState());

            Visible(false);

            FadeIn();

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.MONSTER.id, LayerMaskDefines.MONSTER.id);
        }

        protected override void Update()
        {
            if (machine != null)
            {
                machine.Update(Time.deltaTime);
            }

            if (machine.currentState.GetName() != StateTypes.Dead.ToString())
            {
                base.Update();
            }
        }

        public override void UpdateHUD()
        {
     
            
        }

        public override void Dispose()
        {
            base.Dispose();

            if (machine != null)
            {
                machine.Dispose();
            }
        }


        protected override void OnBecameInvisible()
        {
            Dispose();
        }

        public override void Flip()
        {
            if (direction == Vector2.left)
            {
                if (componentsHolder.transform.localScale.x < 0f)
                {
                    base.Flip();
                }
            }
            else
            {
                if (componentsHolder.transform.localScale.x > 0f)
                {
                    base.Flip();
                }
            }

        }

        public override void OnMissileHit(BaseEntity2D sender, MissileData missile, Vector2 point)
        {
            if (sender != null && machine.currentState.GetName() != StateTypes.Dead.ToString())
            {
                PetData pet = sender.data as PetData;

                data.attributeBox.Validate(ValidateType.Injured, pet.attributeBox, missile, point);
            }
        }

        public override void OnWeaponHit(BaseEntity2D sender, WeaponData weapon, Vector2 point)
        {
            if (sender != null && machine.currentState.GetName() != StateTypes.Dead.ToString())
            {
                GirlData girl = sender.data as GirlData;

                data.attributeBox.Validate(ValidateType.Injured, girl.attributeBox, weapon, point);            
            }                  
        }

        public override void OnSkillHit(BaseEntity2D sender, SkillData skill, Vector2 point)
        {
            if (sender != null && machine.currentState.GetName() != StateTypes.Dead.ToString())
            {
                GirlData girl = sender.data as GirlData;

                data.attributeBox.Validate(ValidateType.Injured, girl.attributeBox, skill, point);
            }
        }

        private void PlayComponentsDestroyEffect()
        {
            AbstractComponentEffect effect = AbstractComponentEffect.LoadRandomEffect();
            effect.transform.position = GetMountPointAtRandom(Space.World);
            effect.Initaizlie();

            LayerManager.GetInstance().AddObject(effect.transform);
        }

        protected override void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.gameObject.tag.CompareTo(TagDefines.TAG_MONSTER_IN_POINT) == 0)
            {
                machine.ChangeState(StateTypes.Transfer.ToString());

            }         
        }       
    }
}