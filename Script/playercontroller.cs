using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//function£º
//(1)hammer follow mouse in a fixed range
//(2)hammer controll body when hitting a rock


public class playercontroller : MonoBehaviour
{
    public Transform Body;
    public Transform HammerHead;
    public AudioSource Force;
    public AudioSource Hiccup;
    public AudioSource Drop;
    public AudioSource Ohh;
    public float maxRange = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        //Body and Hammer should not collid
        Physics2D.IgnoreCollision(Body.GetComponent<Collider2D>(), HammerHead.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        GetherData();

        compute();
  
    }
    #region GetherData
    Vector3 mouse;
    Vector3 center;
    void GetherData()
    {
        // Screen center and mouse position in screen space
        float depth = Mathf.Abs(Camera.main.transform.position.z);
        mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y,depth);
        center = new Vector3(Screen.width/2,Screen.height/2, depth);

        // Transform to world space
        center = Camera.main.ScreenToWorldPoint(center);
        mouse = Camera.main.ScreenToWorldPoint(mouse);

    }

    #endregion

    #region compute
    Vector3 mouseVec;
    Vector3 newHammerPos;
    Vector3 targetbodyPos;
    void compute()
    {
        // Compute new hammer pos
        mouseVec = Vector3.ClampMagnitude(mouse - center, maxRange);
        newHammerPos = Body.position + mouseVec;

        // Check if hammer head is collided with scene objects
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = LayerMask.GetMask("Default");
        Collider2D[] results = new Collider2D[5];
        if (HammerHead.GetComponent<Rigidbody2D>().OverlapCollider(
                contactFilter, results) > 0)  // If collided with scene objects
        {
            targetbodyPos = HammerHead.position - mouseVec;
            Vector2 force = (targetbodyPos - Body.position) * 80.0f;
            Body.GetComponent<Rigidbody2D>().AddForce(force);

            if (Body.GetComponent<Rigidbody2D>().velocity.y >= 5)
            {
                Hiccup.Play();
            }
            

            //limit body velocity
            Body.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(
               Body.GetComponent<Rigidbody2D>().velocity, 6);

        }
        //Adjust the hammerhead
        Vector3 hammerMoveVec = newHammerPos - HammerHead.position;
        newHammerPos = HammerHead.position + hammerMoveVec * 0.2f;

        // Update hammer pos
        HammerHead.GetComponent<Rigidbody2D>().MovePosition(newHammerPos);

        // Update hammer rotation
        HammerHead.rotation = Quaternion.FromToRotation(
            Vector3.right, newHammerPos - Body.position);
    }

    #endregion

    #region restart
    IEnumerator restart()
    {
        yield return new WaitForSeconds(0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().ToString());
    }
    #endregion



}
