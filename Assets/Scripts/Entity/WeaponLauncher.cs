using Utils;
using Manager;

using UnityEngine;
using Prototype;

namespace Entity
{
    public class WeaponLauncher : AbstractLauncher
    {
        private BaseEntity2D owner = null;

        private WeaponUI weaponUI = null;

        private Vector3 targetPosition = Vector3.zero;

        private GirlData girl = null;

        public override void Bind(BaseEntity2D owner)
        {
            this.owner = owner;

            girl = owner.data as GirlData;

            Load(UserManager.GetInstance().user.GetActiveWeapon());
        }

        public override void Unload()
        {
            if (weaponUI != null)
            {
                GameObject.Destroy(weaponUI.gameObject);

                weaponUI = null;
            }
        }

        public override void Load(PrototypeObject data)
        {
            if (data == null) { return; }

            WeaponData weapon = data as WeaponData;

            weaponUI = ResourceUtils.AddAndGetComponent<WeaponUI>(GlobalDefinitions.RESOURCE_PATH_WEAPON + weapon.resourceID);
            weaponUI.transform.SetParent(owner.componentsHolder.transform);
            weaponUI.transform.localPosition = new Vector2(weapon.offsetX, weapon.offsetY);
            weaponUI.transform.localScale = Vector2.one;
            weaponUI.Initialize(weapon);
        }

        public override void Apply(Vector3 targetPosition)
        {
            EntityComponent component = owner.componentsHolder.GetComponent(ComponentDefs.Body);

            tk2dSpriteAnimationClip clip = component.GetClipByName(AnimationDefs.Apply.ToString().ToLower());

            WeaponData weapon = UserManager.GetInstance().user.GetActiveWeapon();

            float time = weapon.CD * (1 - girl.attributeBox.GetAttribute(AttributeKeys.DEX) * 0.01f);
            if (time < 0.1f) { time = 0.1f; }
           
            clip.fps = clip.frames.Length / time;
            component.Play(clip);

            this.targetPosition = targetPosition;

            if (weaponUI != null)
            {
                weaponUI.Config(owner, clip.fps);
            }
        }

        public override void LateApply()
        {
            if (weaponUI != null)
            {
                LayerManager.GetInstance().AddObject(weaponUI.transform);

                weaponUI.Run(this.targetPosition);             
            } 
        }
    }
}