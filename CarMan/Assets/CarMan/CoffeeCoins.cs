using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCoins : MonoBehaviour
{
    //define list ofgameobject
    public List<Rigidbody> coffeeCoins;

    public bool isAddforceing;
    public float force = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        // MyEvent.SystemStartEventStageThree.AddListener(OnSystemStartEventTriggered);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAddforceing)
        {
            foreach (var coin in coffeeCoins)
            {
                coin.AddForce(Vector3.up * force, ForceMode.Force);
            }
        }

        
    }
}
