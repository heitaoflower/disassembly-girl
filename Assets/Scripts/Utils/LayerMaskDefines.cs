using UnityEngine;

namespace Utils
{
    public class LayerMaskDefines
    {
        public struct Mask
        {
            public int id;

            public int value;

            public string name;

            public Mask(string name)
            {
                this.name = name;

                this.id = LayerMask.NameToLayer(name);

                this.value = 1 << this.id;
            }

            public void ToLayer(GameObject go)
            {
                Transform[] childs = go.GetComponentsInChildren<Transform>();
                for (int index = 0; index < childs.Length; index++)
                {
                    childs[index].gameObject.layer = id;
                }

                go.layer = id;
            }
        }

        public static readonly Mask DEFAULT = new Mask("Default");
        
        public static readonly Mask UI = new Mask("UI");

        public static readonly Mask MAP = new Mask("Map");

        public static readonly Mask MONSTER = new Mask("Monster");

        public static readonly Mask GIRL = new Mask("Girl");

        public static readonly Mask PET = new Mask("Pet");

        public static readonly Mask WEAPON = new Mask("Weapon");

        public static readonly Mask MONSTER_MISSILE = new Mask("MonsterMissile");

        public static readonly Mask SKILL = new Mask("Skill");

        public static readonly Mask GUIDE = new Mask("Guide");

        public static readonly Mask PET_MISSILE = new Mask("PetMissile");

    }
}