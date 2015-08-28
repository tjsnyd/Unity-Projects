[RequireComponent (typeof (LineRenderer))]

public class Rope : MonoBehaviour {


	private Transform current;
	public Transform target;
	public float resolution = 0.5F;							  //  Sets the amount of joints there are in the rope (1 = 1 joint for every 1 unit)
	public float ropeDrag = 0.1F;								 //  Sets each joints Drag
	public float ropeMass = 0.1F;							//  Sets each joints Mass
	public float ropeColRadius = 0.5F;					//  Sets the radius of the collider in the SphereCollider component
	//public float ropeBreakForce = 25.0F;					 //-------------- TODO (Hopefully will break the rope in half...
	private Vector2[] segmentPos;			//  DONT MESS!	This is for the Line Renderer's Reference and to set up the positions of the gameObjects
	private GameObject[] joints;			//  DONT MESS!	This is the actual joint objects that will be automatically created
	private LineRenderer line;							//  DONT MESS!	 The line renderer variable is set up when its assigned as a new component
	private int segments = 0;					//  DONT MESS!	The number of segments is calculated based off of your distance * resolution
	private bool rope = false;						 //  DONT MESS!	This is to keep errors out of your debug window! Keeps the rope from rendering when it doesnt exist...
	
	//Joint Settings
	public Vector3 swingAxis = new Vector3(1,1,1);				 //  Sets which axis the character joint will swing on (1 axis is best for 2D, 2-3 axis is best for 3D (Default= 3 axis))
	public float lowTwistLimit = -100.0F;					//  The lower limit around the primary axis of the character joint. 
	public float highTwistLimit = 100.0F;					//  The upper limit around the primary axis of the character joint.
	public float swing1Limit  = 20.0F;					//	The limit around the primary axis of the character joint starting at the initialization point.
	
	
	public Vector2 StartPos;
	public Vector2 EndPos;
	public float speed;
	RaycastHit2D hit;
	Rigidbody2D body;
	private bool moveOk = false;
	GameObject hook;
	
	void Awake()
	{
		body = GetComponent<Rigidbody2D> ();
	}
	
	void Update()
	{
		// Put rope control here!
		if (Input.GetKeyDown("q")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (hit = Physics2D.Raycast(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition),75,(1<<0))){
				Debug.DrawLine(ray.origin, hit.point, Color.red, 3);
				StartPos = new Vector2(transform.position.x, transform.position.y);
				EndPos = hit.point;
				target = hit.transform;
				Debug.Log (EndPos);
				//MoveTo = true; // Enables the lerp function to update until it's finished, or cancelled.
				if(!rope)
				{
					Debug.Log(StartPos);
					BuildRope();
					moveOk = true;
				}
			}
		}
		if (moveOk && rope && Input.GetKeyDown ("v")) {
			body.gravityScale = 0;
			float step = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, EndPos, step);
			if (new Vector2(transform.position.x,transform.position.y) == EndPos){ // This enables gravity when you're done moving. If you're using a rigidbody you could use rigidbody.isKinematic = true; to turn gravity back on.
				moveOk = false;
			}
		}
		if (Input.GetKeyUp ("v") && (body.gravityScale == 0)) {
			body.gravityScale = 1;
		}
		//Destroy Rope Test	(Example of how you can use the rope dynamically)
		if(rope && Input.GetKeyDown("b"))
		{
			DestroyRope();	
		}	
		
	}
	void LateUpdate()
	{
		// Does rope exist? If so, update its position
		if(rope) {
			for(int i=0;i<segments;i++) {
				if(i == 0) {
					line.SetPosition(i,transform.position);
				} else
				if(i == segments-1) {
					line.SetPosition(i,hit.transform.localPosition);	
				} else {
					line.SetPosition(i,joints[i].transform.position);
				}
			}
			line.enabled = true;
		} else {
			line.enabled = false;	
		}
	}
	
	GameObject endJ;
	
	void BuildRope()
	{
		line = gameObject.GetComponent<LineRenderer>();
		
		// Find the amount of segments based on the distance and resolution
		// Example: [resolution of 1.0 = 1 joint per unit of distance]
		segments = (int)(Vector2.Distance(StartPos,EndPos)*resolution);
		Debug.Log (segments);
		line.SetVertexCount(segments);
		segmentPos = new Vector2[segments];
		joints = new GameObject[segments];
		segmentPos[0] = StartPos;
		segmentPos[segments-1] = EndPos;
		
		// Find the distance between each segment
		var segs = segments-1;
		var seperation = ((EndPos - StartPos)/segs);
		endJ = target.gameObject;
		for(int s=1;s < segments;s++)
		{
			// Find the each segments position using the slope from above
			Vector2 vector = (seperation*s) + StartPos;	
			segmentPos[s] = vector;
			
			//Add Physics to the segments
			AddJointPhysics(s);
		}
		
		// Attach the joints to the target object and parent it to this object	

		DistanceJoint2D end = endJ.AddComponent<DistanceJoint2D>();
		end.connectedBody = joints[joints.Length-1].transform.GetComponent<Rigidbody2D>();
		//target.parent = transform;
		
		// Rope = true, The rope now exists in the scene!
		rope = true;
	}
	
	void AddJointPhysics(int n)
	{
		current = transform;
		joints[n] = new GameObject("Joint_" + n);
		//joints[n].transform.parent = current;
		Rigidbody2D rigid = joints[n].AddComponent<Rigidbody2D>();
		CircleCollider2D col = joints[n].AddComponent<CircleCollider2D>();
		DistanceJoint2D ph = joints[n].AddComponent<DistanceJoint2D>();
		//ph.breakForce = ropeBreakForce; <--------------- TODO
		
		joints[n].transform.position = segmentPos[n];
		
		rigid.drag = ropeDrag;
		rigid.mass = ropeMass;
		col.radius = ropeColRadius;
		
		if(n==1){		
			ph.connectedBody = transform.GetComponent<Rigidbody2D>();
		} else
		{
			ph.connectedBody = joints[n-1].GetComponent<Rigidbody2D>();	
		}
		
	}
	
	void DestroyRope()
	{
		// Stop Rendering Rope then Destroy all of its components
		rope = false;
		for(int dj=0;dj<joints.Length-1;dj++)
		{
			if(dj == joints.Length-2){
				Destroy(endJ.GetComponent<DistanceJoint2D>());
			}
			Destroy(joints[dj]);	
		}
		
		segmentPos = new Vector2[0];
		joints = new GameObject[0];
		segments = 0;
	}
}
