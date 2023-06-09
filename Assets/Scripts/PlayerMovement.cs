using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mirror {
    public class PlayerMovement : NetworkBehaviour {
        public float moveSpeed= 5f;
        public Rigidbody2D rb;
        Vector2 movement;

        // Update is called once per frame
        void Update()
        {
            movement.y=Input.GetAxisRaw("Vertical");
            movement.x=Input.GetAxisRaw("Horizontal");
        }

        void FixedUpdate() {
            if(isLocalPlayer) {
                rb.MovePosition(rb.position+movement*moveSpeed*Time.fixedDeltaTime);
            } 
            
        }
    }
}
