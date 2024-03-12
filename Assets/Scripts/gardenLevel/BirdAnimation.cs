using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : MonoBehaviour
{
    public CharacterJoint jointToModify;

    public void DisconnectJoint()
    {
        if (jointToModify != null)
        {
            jointToModify.connectedBody = null;
        }
    }
}
