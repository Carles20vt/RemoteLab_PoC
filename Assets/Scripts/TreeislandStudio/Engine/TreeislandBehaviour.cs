using TreeislandStudio.Engine.Exceptions;
using UnityEngine;

namespace TreeislandStudio.Engine {
    public class TreeislandBehaviour : MonoBehaviour {
        #region Types
        
        /// <summary>
        /// Max of skipped frames on fixed updates, this should not happen, but in case it occurs should avoid the game objects using a lot of time doing fixedUpdate calculations
        /// </summary>
        protected const int MaxSkippedFrames = 10;
        
        #endregion
        #region Non public methods

        /// <summary>
        /// Logs the message (and the context)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        protected static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }

        /// <summary>
        /// Logs the message
        /// </summary>
        /// <param name="message"></param>
        protected static void Log(object message)
        {
            Debug.Log(message);
        }

        /// <summary>
        /// Return the transform gameobject or raises a GameObjectNotFoundException with the given message
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static GameObject GetGameObject(Transform transform, string message)
        {
            if ((transform == null) || (transform.gameObject == null)) {
                throw new GameObjectNotFoundException(message);
            }

            return transform.gameObject;
        }

        /// <summary>
        /// Return the transform gameobject or raises a GameObjectNotFoundException with the given message
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static GameObject GetGameObject(GameObject gameObject, string name, string message)
        {
            return GetGameObject(gameObject.transform.Find(name), message);
        }

        /// <summary>
        /// Return the transform gameobject or raises a GameObjectNotFoundException with the given message
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetGameObject(GameObject gameObject, string name)
        {
            return GetGameObject(gameObject, name, "A GameOject named \"" + name + "\" is required");
        }

        /// <summary>
        /// Return the transform gameobject or raises a GameObjectNotFoundException with the given message
        /// </summary>
        /// <param name="gameObjectName"></param>
        /// <returns></returns>
        protected GameObject GetGameObject(string gameObjectName)
        {
            return GetGameObject(
                transform.gameObject,
                gameObjectName,
                "A GameOject named \"" + gameObjectName + "\" is required"
            );
        }

        /// <summary>
        /// Return the given component or raises and exception of type ComponentNotFound this the given message
        /// </summary>
        /// <param name="message"></param>
        protected Type GetComponent<Type>(string message)
        {
            var component = base.GetComponent<Type>();

            if (component == null) {
                throw new ComponentNotFoundException(message);
            }

            return component;
        }

        /// <summary>
        /// Return the given component or raises and exception of type ComponentNotFound this the given message
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="message"></param>
        protected static Type GetComponent<Type>(GameObject gameObject, string message)
        {
            var component = gameObject.GetComponent<Type>();

            if (component == null) {
                throw new ComponentNotFoundException(message);
            }

            return component;
        }

        /// <summary>
        /// Return the given component
        /// </summary>
        protected new Type GetComponent<Type>()
        {
            return GetComponent<Type>("A component of type " + typeof(Type).Name + " is required");
        }

        /// <summary>
        /// Return the given component
        /// </summary>
        protected static Type GetComponent<Type>(GameObject gameObject)
        {
            return GetComponent<Type>(
                gameObject,
                "A component of type " + typeof(Type).Name + " is required"
            );
        }

        /// <summary>
        /// Log out all the component names in this gameobject
        /// </summary>
        /// <param name="gameObject"></param>
        public static void DebugComponents(GameObject gameObject)
        {
            Log("Components in " + gameObject.name, gameObject);
            foreach (var component in gameObject.GetComponents<Component>()) {
                Log("    " + component.name, component);
            }

        }

        #endregion
    }
}