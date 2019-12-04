using System;
using System.Collections.Generic;

namespace MatchThree
{
    public static class ServiceContainer
    {
        private static Dictionary<Type, object> _services = new Dictionary<Type, object>(10);

        public static bool GetService<T>(out T service) where T : class
        {
            var type = typeof(T);

            if (!_services.ContainsKey(type))
            {
                service = null;
                return false;
            }

            service = _services[type] as T;
            return true;
        }

        public static bool RegisterService<T>(T concrete, bool rewrite = false) where T : class
        {
            var type = typeof(T);

            if (!_services.ContainsKey(typeof(Type)))
            {
                _services.Add(type, concrete);
                return true;
            }

            if (rewrite)
            {
                _services[type] = concrete;
                return true;
            }

            return false;
        }

        public static void RemoveService<T>()
        {
            var type = typeof(T);

            if (_services.ContainsKey(typeof(T)))
            {
                _services.Remove(type);
            }
        }

        public static void ClearAll() => _services.Clear();
    }
}

