using UnityEngine;
using System.Collections;

public class chara_charaCon : MonoBehaviour {

    //***********//
    float speed;
    float jumpSpeed;
    float rotateSpeed;
    float gravity;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController charaCon;
    //***********//

    float gravitySpeed;


    // Use this for initialization
    void Start()
    {
        //********//
        speed = 3.0f;
        jumpSpeed = 30.0F;
        rotateSpeed = 20f;
        gravity = 120.0F;
        charaCon = GetComponent<CharacterController>();
        //********//

        gravitySpeed = -0.1f;

    }

    // Update is called once per frame
    void Update()
    {

        CameraAxisControl();

        //ジャンプ
        if (charaCon.isGrounded)
        {
            jumpControl();
        }

        attachMove();
        attachRotation();
    }

    //標準的なコントロール
    void NormalControl()
    {
        //if (charaCon.isGrounded)
        //{
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);    //?必要ない気もする
            moveDirection *= speed;
        //}
    }

    //カメラ軸に沿った移動コントロール
    void CameraAxisControl()
    {
        //if (charaCon.isGrounded)
        //{
            Vector3 forward = GameObject.Find("cameraOrigin").transform.TransformDirection(Vector3.forward);
            Vector3 right = GameObject.Find("cameraOrigin").transform.TransformDirection(Vector3.right);

            moveDirection = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
            moveDirection *= speed;
        //}
    }
    //標準的なジャンプコントロール
    void jumpControl()
    {
        if (Input.GetButton("Jump"))
        {
            gravitySpeed = 0.5f;
        }
    }

    //移動処理 
    void attachMove()
    {
        //重力関係
        if (gravitySpeed < -0.1f)
        {
            gravitySpeed = -0.1f;
        }
        charaCon.Move(moveDirection * Time.deltaTime + Vector3.up * gravitySpeed);
    }

    //キャラクターを進行方向へ向ける処理 
    void attachRotation()
    {
        var moveDirectionYzero = -moveDirection;
        moveDirectionYzero.y = 0;

        //ベクトルの２乗の長さを返しそれが0.001以上なら方向を変える（０に近い数字なら方向を変えない） 
        if (moveDirectionYzero.sqrMagnitude > 0.001)
        {

            //２点の角度をなだらかに繋げながら回転していく処理（stepがその変化するスピード） 
            float step = rotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, moveDirectionYzero, step, 0f);

            transform.rotation = Quaternion.LookRotation(-moveDirectionYzero);
        }
    }

}
