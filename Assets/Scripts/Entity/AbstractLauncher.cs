using UnityEngine;
using Prototype;

namespace Entity
{
    public abstract class AbstractLauncher
    {
        public abstract void Bind(BaseEntity2D owner);

        public abstract void Unload();

        public abstract void Load(PrototypeObject data);

        public abstract void Apply(Vector3 targetPosition);

        public abstract void LateApply();
    }
}
