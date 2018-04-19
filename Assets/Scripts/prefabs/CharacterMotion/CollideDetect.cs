
using UnityEngine;

public class CollideDetect : MonoBehaviour {

    public MapJoins MP;
    private void OnCollisionEnter(Collision collision)
    {
        if(MP != null && MP.CanWork)
        {
            
            if (MP.CurrentJoint != null && MP.CurrentPunchBag != null)
            {
                GameObject JointObj = collision.collider.gameObject;
            
                if (MP.CurrentPunchBag.Equals(gameObject) && JointObj.CompareTag( MP.SkelCharJointMap[MP.CurrentJoint]))
                {
                    //Debug.Log(collision.gameObject + "-" + collision.collider.gameObject.name + "-" + collision.contacts + "-" + "-" + collision.impulse + "-" + collision.relativeVelocity + "-" + collision.transform + "-" + collision.rigidbody);
                    JoinForce JF = JointObj.GetComponent<JoinForce>();
                    float[] FV = JF.FinalVelocities.ToArray();
                    MP.GameMainScreen.Power = (int)JF.sum / FV.Length;
                    MP.GameMainScreen.Score += 5;
                    MP.isAccuracy = false;
                    MP.isPower = true;
                    MP.Map();
                }
            }
        }
    }
}
