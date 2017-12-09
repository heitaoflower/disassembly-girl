namespace Prototype
{
    public class EntityComponentData
    {
        public string name = null;

        public float threshold = default(float);
       
        public static EntityComponentData Parse(string component)
        {
            EntityComponentData data = new EntityComponentData();

            data.name = component.Split(':')[0];
            data.threshold = float.Parse(component.Split(':')[1]);

            return data;
        }
    }
}