using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;



public class BehaviorTreeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BehaviorTree bTree = this.gameObject.AddComponent<BehaviorTree>();
        bTree.StartWhenEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
