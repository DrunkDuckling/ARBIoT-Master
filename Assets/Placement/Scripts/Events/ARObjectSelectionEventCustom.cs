using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ARObjectSelectionEventCustom : UnityEvent<ARSelectionInteractableCustom, GameObject, bool>
{

}