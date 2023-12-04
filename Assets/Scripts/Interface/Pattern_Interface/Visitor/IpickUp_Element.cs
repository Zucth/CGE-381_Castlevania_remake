using Mono.Cecil.Rocks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IpickUp_Element
{
    void Accept(IVisitor visitor);
}
