using UnityEngine;
using System.Collections;

public class dragThrow : MonoBehaviour {
    public float dampingRatio = 5.0f;
    public float frequency = 2.5f;
    public float drag = 10.0f;
    public float angularDrag = 5.0f;

    private SpringJoint2D springJoint;

    void Update () {
       
        if ( !Input.GetMouseButtonDown(0) ) {
            return;
        }

        Camera camera = FindCamera();
        RaycastHit2D hit = Physics2D.Raycast(
                camera.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);

       
        if (hit.collider == null || !hit.rigidbody || hit.rigidbody.isKinematic) {
            return;
        }

        
        if (!springJoint) {
            GameObject obj = new GameObject("Rigidbody2D dragger");
            Rigidbody2D body = obj.AddComponent("Rigidbody2D") as Rigidbody2D;
            this.springJoint = obj.AddComponent("SpringJoint2D") as SpringJoint2D;
            body.isKinematic = true;
        }

        
        springJoint.transform.position = hit.point;
        
        springJoint.anchor = Vector2.zero;
       
        springJoint.connectedAnchor = hit.transform.InverseTransformPoint(hit.point);
        springJoint.dampingRatio = this.dampingRatio;
        springJoint.frequency = this.frequency;
   
        springJoint.collideConnected = false;
        springJoint.connectedBody = hit.rigidbody;


        StartCoroutine(DragObject());
	}

    IEnumerator DragObject() {

        float oldDrag = this.springJoint.connectedBody.drag;
        float oldAngularDrag = this.springJoint.connectedBody.angularDrag;

        springJoint.connectedBody.drag = drag;
        springJoint.connectedBody.angularDrag = angularDrag;

 
        Camera camera = FindCamera();
        while ( Input.GetMouseButton(0) ) {
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            springJoint.transform.position = mousePos;
            yield return null;
        }


        if (springJoint.connectedBody) {
            springJoint.connectedBody.drag = oldDrag;
            springJoint.connectedBody.angularDrag = oldAngularDrag;
            springJoint.connectedBody = null;
        }
    }

    Camera FindCamera() {
        if (camera) {
            return camera;
        } else {
            return Camera.main;
        }
    }
}
