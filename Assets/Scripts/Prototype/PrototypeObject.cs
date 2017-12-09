namespace Prototype
{
    public enum PrototypeState : int
    {
        Normal,
        Frozen,
        Count
    }

    public class PrototypeObject
    {
        public int id = default(int);

        public string name = default(string);

        public PrototypeState state = PrototypeState.Normal;

        public AttributeBox attributeBox = default(AttributeBox);
    }
}