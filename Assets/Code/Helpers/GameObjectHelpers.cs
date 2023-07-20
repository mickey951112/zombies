using UnityEngine;
using UnityEngine.Assertions;

public static class GameObjectHelpers
{
    public static GameObject Create(
        string name,
        Vector3? position = null,
        Quaternion? rotation = null,
        Vector3? scale = null,
        Transform parent = null,
        bool useWorldSpace = true
    )
    {
        Assert.IsTrue(useWorldSpace || parent != null);

        var gameObject = new GameObject(name);
        gameObject.transform.SetParent(parent, worldPositionStays: false);

        if (useWorldSpace)
        {
            if (position.HasValue)
            {
                if (rotation.HasValue)
                {
                    gameObject.transform.SetPositionAndRotation(position.Value, rotation.Value);
                }
                else
                {
                    gameObject.transform.position = position.Value;
                }
            }
            else if (rotation.HasValue)
            {
                gameObject.transform.rotation = rotation.Value;
            }

            if (scale.HasValue)
            {
                var scaleToUse = scale.Value;
                if (parent != null)
                {
                    scaleToUse = new Vector3(
                        scaleToUse.x / parent.lossyScale.x,
                        scaleToUse.y / parent.lossyScale.y,
                        scaleToUse.z / parent.lossyScale.z
                    );
                }
                gameObject.transform.localScale = scaleToUse;
            }
        }
        else
        {
            if (position.HasValue)
            {
                gameObject.transform.localPosition = position.Value;
            }
            if (rotation.HasValue)
            {
                gameObject.transform.localRotation = rotation.Value;
            }
            if (scale.HasValue)
            {
                gameObject.transform.localScale = scale.Value;
            }
        }

        return gameObject;
    }

    public static (GameObject, T) Create<T>(
        string name,
        Vector3? position = null,
        Quaternion? rotation = null,
        Vector3? localScale = null
    )
        where T : Component
    {
        var gameObject = Create(name, position, rotation, localScale);
        var component1 = gameObject.AddComponent<T>();
        return (gameObject, component1);
    }

    public static (GameObject, T1, T2) Create<T1, T2>(
        string name,
        Vector3? position = null,
        Quaternion? rotation = null,
        Vector3? localScale = null
    )
        where T1 : Component
        where T2 : Component
    {
        var gameObject = Create(name, position, rotation, localScale);
        var component1 = gameObject.AddComponent<T1>();
        var component2 = gameObject.AddComponent<T2>();
        return (gameObject, component1, component2);
    }
}
