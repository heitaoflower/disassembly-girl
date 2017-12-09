using System;

namespace View
{
    public enum ViewLayerTypes : int
    {
        None = 0,
        Scene,
        Main,
        Window,
        Alert,
        Guide,
        Count
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ViewAttribute : Attribute
    {
       public string prefabPath { get; set; }

       public bool isSingleton { get; set; }

       public ViewLayerTypes layer { get; set; }
    }
}