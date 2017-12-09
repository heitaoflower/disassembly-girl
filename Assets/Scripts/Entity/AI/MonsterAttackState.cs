using Orange.StateKit;
using Utils;
using System;
using UnityEngine;
using Manager;
using Entity.Missile;
using Prototype;

namespace Entity.AI
{
    public class MonsterAttackState : State<MonsterEntity2D>
    {
        public override void Begin(object data = null)
        {
            MonsterData monster = GetContext().data as MonsterData;

            Action<EntityComponent> Callback = (EntityComponent component) =>
            {
                component.animator.AnimationCompleted = null;

                Transform missile = ResourceUtils.GetComponent<Transform>(GlobalDefinitions.RESOURCE_PATH_MISSLE + monster.missileData.resourceID);
                missile.position = GetContext().GetMountPoint(ComponentDefs.Weapon).transform.position;

                LayerManager.GetInstance().AddObject(missile);

                MonsterMissileUI missileUI = missile.gameObject.AddComponent<MonsterMissileUI>();
                missileUI.owner = GetContext();
                missileUI.Run(DungeonManager.GetInstance().girlEntity.transform.position);

                DungeonManager.GetInstance().missiles.Add(missileUI);
                missileUI.OnDispose = () => 
                {
                    DungeonManager.GetInstance().missiles.Remove(missileUI);
                };
    
                GetStateMachine().ChangeState(StateTypes.Wander.ToString());
            };       

            foreach (EntityComponent component in GetContext().componentsHolder.components)
            {
                if (component.type == ComponentDefs.Weapon)
                {
                    component.Play(AnimationDefs.Attack.ToString().ToLower(), Callback);
                }
                else
                {
                    component.Play(AnimationDefs.Attack.ToString().ToLower());
                }              
            }        

            GetContext().componentsHolder.GetComponent(ComponentDefs.Weapon).OnUnloadFinished = () =>
            {
                GetStateMachine().ChangeState(StateTypes.Idle.ToString());
            };

        }

        public override void Update(float deltaTime)
        {
           
        }

        public override void End()
        {
            base.End();

            if (GetContext().componentsHolder.HasComponent(ComponentDefs.Weapon))
            {
                GetContext().componentsHolder.GetComponent(ComponentDefs.Weapon).animator.AnimationCompleted = null;
                GetContext().componentsHolder.GetComponent(ComponentDefs.Weapon).OnUnloadFinished = null;
                GetContext().componentsHolder.GetComponent(ComponentDefs.Body).animator.AnimationCompleted = null;

                GetContext().componentsHolder.GetComponent(ComponentDefs.Weapon).Play(AnimationDefs.Idle.ToString().ToLower());
                GetContext().componentsHolder.GetComponent(ComponentDefs.Body).Play(AnimationDefs.Idle.ToString().ToLower());

            }
         
        }

        public override string GetName()
        {
            return StateTypes.Attack.ToString();
        }
    }
}