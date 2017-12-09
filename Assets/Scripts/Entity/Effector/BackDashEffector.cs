using System.Collections;
using UnityEngine;
using Entity.AI;
using Utils;

namespace Entity.Effector
{
    public class BackDashEffector : AbstractApplierEffector
    {
        private tk2dSpriteAnimator effector = null;

        private class DashState
        {
            public bool isDashing = false;
            public float timeDashed = default(float);
            public Vector3 dashDir = Vector2.zero;
            public Vector3 start = Vector2.zero;
        }

        private DashState state = null;

        protected override void Activate()
        {
            base.Activate();

            if (owner.motor2D.isGrounded)
            {
                MonsterEntity2D monster = owner as MonsterEntity2D;

                state = new DashState();
                state.isDashing = true;
                state.dashDir = -monster.direction;
                state.start = monster.transform.position;
                state.timeDashed = Time.deltaTime;

                monster.machine.ChangeState(StateTypes.BackDash.ToString());

                Transform mountPoint = monster.GetMountPoint(ComponentDefs.Foot);

                if (mountPoint != null)
                {
                    effector = ResourceUtils.GetComponent<tk2dSpriteAnimator>(GlobalDefinitions.RESOURCE_PATH_BUFF + data.resourceID);
                    effector.transform.SetParent(mountPoint);
                    effector.transform.localPosition = Vector3.zero;
                    effector.AnimationCompleted = (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) =>
                    {
                        GameObject.Destroy(effector.gameObject);
                    };
                }
            }
      
        }

        protected override void Deactivate()
        {
            if (owner.motor2D.isGrounded)
            {
                ((MonsterEntity2D)owner).machine.ChangeState(StateTypes.Idle.ToString());
            }

            base.Deactivate();

            Destroy(this);

        }

        protected override IEnumerator RunProcessor()
        {
            while (state != null && state.isDashing)
            {
                state.timeDashed = Mathf.Clamp(state.timeDashed + Time.deltaTime, 0f, data.parameter1);
                float normalizedTime = state.timeDashed / data.parameter1;

                float distance = EaseOutQuad(0, data.parameter2, normalizedTime);
             
                owner.motor2D.move(state.dashDir * distance);

                if (state.timeDashed >= data.parameter1)
                {
                    ((MonsterEntity2D)owner).machine.ChangeState(StateTypes.Idle.ToString());

                    state.isDashing = false;
                }

                yield return null;
            }

            Cancel();
        }

        private float EaseOutQuad(float start, float end, float value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
        }
    }
}