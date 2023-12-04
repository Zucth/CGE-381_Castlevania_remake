using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour, IpickUp_Element
{
    private List<IpickUp_Element> _elementList = new List<IpickUp_Element>();
    private void Start()
    {
        _elementList.Add(gameObject.AddComponent<Score_PickUp>());
    }

    public void Accept(IVisitor visitor)
    {
        foreach(IpickUp_Element element in _elementList)
        {
            element.Accept(visitor);
        }
    }
}
