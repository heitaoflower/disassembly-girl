using Orange.StateKit;
using Prototype;
using System;
using UnityEngine;
using Utils;
using Manager;
using Entity.Missile;

namespace Entity.AI
{
    public class PetAttackState : State<PetEntity2D>
    {
        public override void Begin(object data = null)
        {
            base.Begin(data);

            if (DungeonManager.GetInstance().battleStatus == BattleStatus.Start && DungeonManager.GetInstance().monsters.Count != 0)
            {
                PetData petData = GetContext().data as PetData;

                Action<EntityComponent> Callback = (EntityComponent component) =>
                {
                    component.animator.AnimationCompleted = null;

                    BaseEntity2D target = DungeonManager.GetInstance().SearchMonsterAtRandom();

                    if (target != null)
                    {
                        Transform missile = ResourceUtils.GetComponent<Transform>(GlobalDefinitions.RESOURCE_PATH_MISSLE + petData.missileData.resourceID);
                        missile.position = GetContext().GetMountPoint(ComponentDefs.Weapon).transform.position;

                        LayerManager.GetInstance().AddObject(missile);

                        PetMissileUI missileUI = missile.gameObject.AddComponent<PetMissileUI>();
                        missileUI.Config(((PetData)GetContext().data).missileData, GetContext());
                        missileUI.Run(target.GetMountPoint(ComponentDefs.Body).transform.position);
                        missileUI.OnDispose = () =>
                        {
                            DungeonManager.GetInstance().missiles.Remove(missileUI);
                        };

                        DungeonManager.GetInstance().missiles.Add(missileUI);
                       
                    }                   

                    GetStateMachine().ChangeState(StateTypes.Idle.ToString());
                };

                foreach (EntityComponent component in GetContext().componentsHolder.components)
                {
                    component.Play(AnimationDefs.Attack.ToString().ToLower(), Callback);
                }
            }
            else
            {
                GetStateMachine().ChangeState(StateTypes.Idle.ToString());
            }            
        }

        public override void End()
        {
            base.End();
        }

        public override void Update(float deltaTime)
        {

        }

        public override string GetName()
        {
            return StateTypes.Attack.ToString();
        }

    }
}