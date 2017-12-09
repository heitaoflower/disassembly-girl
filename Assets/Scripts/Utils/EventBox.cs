using System.Collections.Generic;

namespace Utils
{
    public class EventBox
    {
        public delegate void EventBoxHandler(object data);

        private static IDictionary<string, IList<EventBoxHandler>> handlerDic = new Dictionary<string, IList<EventBoxHandler>>();

        private static IDictionary<object, ListenerHandlerMapper> listenerDic = new Dictionary<object, ListenerHandlerMapper>();

        public static void Send(string type, object data = null)
        {
            if (handlerDic.ContainsKey(type))
            {
                IList<EventBoxHandler> handlers = handlerDic[type];

                for (int index = 0; index < handlers.Count; index++)
                {
                    handlers[index](data);
                }
            }
        }

        public static void Add(string type, EventBoxHandler handler)
        {
            if (!handlerDic.ContainsKey(type))
            {
                IList<EventBoxHandler> handlers = new List<EventBoxHandler>() { handler };
                handlerDic.Add(type, handlers);
            }
            else
            {
                if (!handlerDic[type].Contains(handler))
                {
                    handlerDic[type].Add(handler);
                }
            }

            if (handler.Target != null)
            {
                if (!listenerDic.ContainsKey(handler.Target))
                {
                    ListenerHandlerMapper map = new ListenerHandlerMapper();
                    map.Add(type, handler);
                    listenerDic.Add(handler.Target, map);
                }
                else
                {
                    listenerDic[handler.Target].Add(type, handler); ;
                }
            }
        }

        public static void Remove(string type, EventBoxHandler handler)
        {
            if (handlerDic.ContainsKey(type))
            {
                if (handlerDic[type].Remove(handler))
                {
                    if (handlerDic[type].Count == 0)
                    {
                        handlerDic.Remove(type);
                    }
                }
            }

            if (listenerDic.ContainsKey(handler.Target))
            {
                listenerDic[handler.Target].Remove(type, handler);

                if (listenerDic[handler.Target].Get().Count == 0)
                {
                    listenerDic.Remove(handler.Target);
                }
            }
        }

        public static void RemoveAll(object listener)
        {
            if (listenerDic.ContainsKey(listener))
            {
                foreach (KeyValuePair<string, EventBoxHandler> item in listenerDic[listener].Get())
                {
                    if (handlerDic.ContainsKey(item.Key))
                    {
                        if (handlerDic[item.Key].Remove(item.Value))
                        {
                            if (handlerDic[item.Key].Count == 0)
                            {
                                handlerDic.Remove(item.Key);
                            }
                        }
                    }
                }
            }

            listenerDic.Remove(listener);
        }

        public static void Dispose()
        {
            handlerDic.Clear();
            listenerDic.Clear();
        }
    }

    class ListenerHandlerMapper
    {
        private IList<KeyValuePair<string, EventBox.EventBoxHandler>> handlers = new List<KeyValuePair<string, EventBox.EventBoxHandler>>();

        public void Add(string type, EventBox.EventBoxHandler handler)
        {
            KeyValuePair<string, EventBox.EventBoxHandler> item = new KeyValuePair<string, EventBox.EventBoxHandler>(type, handler);

            if (handlers.Contains(item) == false)
            {
                handlers.Add(item);
            }
        }

        public void Remove(string type, EventBox.EventBoxHandler handler)
        {
            KeyValuePair<string, EventBox.EventBoxHandler> item = new KeyValuePair<string, EventBox.EventBoxHandler>(type, handler);
            handlers.Remove(item);
        }

        public IList<KeyValuePair<string, EventBox.EventBoxHandler>> Get()
        {
            return handlers;
        }
    }
}