using UnityEngine;

public class PlayerDeplacements : MonoBehaviour
{
    public float moveSpeed = 5.0f;
	public Rigidbody2D rb;
	private Vector3 velocity = Vector3.zero;
	
	public Animator animator;
	public SpriteRenderer sr;

	private int testCollect = 0;
	public Timer finisher;

	public void Increment()
    {
		++testCollect;
		Debug.Log(testCollect);
    }

    void FixedUpdate()
    {
		float horizontalMovement = moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		float verticalMovement = moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		
		Vector3 targetVelocity = new Vector2(horizontalMovement, verticalMovement);
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
		UpdateAnimations(horizontalMovement, verticalMovement);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
			finisher.Finish();

			GameManager.Instance.TriggerWin();
        }
    }

    void UpdateAnimations(float horizontalMovement,float verticalMovement){
		int orient = 0;
		if (horizontalMovement<0){
			orient = 1; //LATERAL
			sr.flipX = false;
		}
		else if (horizontalMovement>0){
			orient = 1;
			sr.flipX = true;
		}
		else if (verticalMovement>0)
			orient = 2; //HAUT
		else if (verticalMovement<0)
			orient = 3; //BAS

		animator.SetInteger("Orientation", orient);
		animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x)+Mathf.Abs(rb.velocity.y));
	}
}
