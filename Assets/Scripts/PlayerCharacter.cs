using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : NetworkBehaviour {

    [SerializeField] float offsetSize = 3f;

    public override void Spawned()  {  // Use instead of Start/Awake for NetworkObjects
        /* Move to a random location around the current location */
        float offset_x = Random.Range(-offsetSize, offsetSize);
        float offset_z = Random.Range(-offsetSize, offsetSize);
        transform.position += new Vector3(offset_x, 0, offset_z);
    }
}
