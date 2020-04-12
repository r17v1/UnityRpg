using RPG.Helper;
using UnityEngine;

public class LongAttackRotate : MonoBehaviour
{
    bool rotate = false;
    GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void Update()
    {
        if(rotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, player.transform.position),Time.deltaTime*10);
        }
    }
    void RotateOn()
    {
        rotate = true;
    }
    void RotateOff()
    {
        rotate = false ;
    }

}
