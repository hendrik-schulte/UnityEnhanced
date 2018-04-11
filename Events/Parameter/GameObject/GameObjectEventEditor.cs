#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomEditor(typeof(GameObjectEvent), true)]
    [CanEditMultipleObjects]
    public class GameObjectEventEditor : ParameterEventEditor<GameObject, GameObjectEvent>
    {
    }
}
#endif