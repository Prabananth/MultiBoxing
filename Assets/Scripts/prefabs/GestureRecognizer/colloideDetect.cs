using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class colloideDetect : MonoBehaviour
{

    public bool started = false;
    float cur = 0.0f;
    public float lastcollided =-1;
    public GameObject start, mid, end;
    public float validangle;
    public bool anglevalid = true;
    public bool flag=false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(this.name) && !other.gameObject.name.Equals(this.name))
        {
            Debug.Log("Collided" + other.gameObject.name);
            string[] currr = other.name.Split('.');
            cur = float.Parse(currr[1]);
            BodyProperties BP = transform.parent.gameObject.GetComponent<BodyProperties>();
            if ((cur > lastcollided || other.name.Contains("Initial")))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("C:\\Users\\rajan\\Desktop\\" + BP.GD.gestureName + ".xml");
                foreach (XmlNode node in doc.DocumentElement)
                {
                    if (node.Attributes[0].InnerText == "ValidAngle" && node.Attributes[1].InnerText == this.name)
                    {
                        start = GameObject.Find(node.Attributes[2].InnerText);
                        mid = GameObject.Find(node.Attributes[3].InnerText);
                        end = GameObject.Find(node.Attributes[4].InnerText);
                        validangle = float.Parse(node.ChildNodes[0].InnerText);
                        Vector3 a = start.transform.position - mid.transform.position;
                        Vector3 b = end.transform.position - mid.transform.position;
                        if (Vector3.Angle(a, b) > validangle - 40 && Vector3.Angle(a, b) < validangle + 40)
                            anglevalid = true;
                        else
                        {
                            anglevalid = false;
                            break;
                        }
                    }
                }
                if (lastcollided != cur && anglevalid && BP.getType() == "Dynamic")
                {
                    if (other.name.Contains("Initial") && started == false)
                    {
                        Debug.Log("First" + gameObject.name);
                        started = true;
                        flag = true;
                        BP.increaseInitial();
                        BP.increaseCollisions();
                        lastcollided = cur;
                        BP.GD.accuracy = 0;
                    }
                    else if (other.name.Contains("Initial") && started == true)
                    {
                        Debug.Log("Last" + gameObject.name);
                        flag = false;
                        BP.increaseCollisions();
                        BP.increaseInitial();
                        BP.ResetColliders();
                        lastcollided = 0;
                    }
                    else if(flag)
                    {
                        Debug.Log("Middle" + gameObject.name);
                        BP.increaseCollisions();
                        lastcollided = cur;
                    }

                }
                else if (BP.getType() == "Static")
                {
                    BP.GD.gestureTrue = true;
                }
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains(this.name))
        {
            BodyProperties BP = transform.parent.gameObject.GetComponent<BodyProperties>();
            if (BP.getType() == "Static")
                BP.GD.gestureTrue = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains(this.name))
        {
            BodyProperties BP = transform.parent.gameObject.GetComponent<BodyProperties>();
            if (BP.getType() == "Static")
                BP.GD.gestureTrue = false;
            else if (other.name.Contains("Initial") && BP.getType() == "Dynamic")
            {
                BP.GD.FrameTime = 0f;
            }
        }
    }
}