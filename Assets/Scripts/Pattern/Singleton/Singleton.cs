using UnityEngine;

public class Singleton<T> : MonoBehaviour 
    where T : Component
{

    private static T _instant;

    public static T Instance
    {
        get
        {
            if(_instant == null) //find itself as the gameobject
            {
                _instant = FindObjectOfType<T>();

                if(_instant == null) //instantiae a new one if there is none
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instant = obj.AddComponent<T>();
                }
            }

            return _instant;
        }
    }

    public virtual void Awake()
    {
        if (_instant == null)
        {
            _instant = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected void DestroyThyself()
    {
        Destroy(gameObject);
        _instant = null;    // because destroy doesn't happen until end of frame
    }

}
