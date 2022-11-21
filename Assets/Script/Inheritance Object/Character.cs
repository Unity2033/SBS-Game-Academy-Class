using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Biology
{
    private float mouseX;
    private int magazine = 10;
    private float gravity = 40.0f;

    [SerializeField] float speed;
    [SerializeField] float mouseSpeed; 
    [SerializeField] float distance = 100.0f;
    [SerializeField] LayerMask layer;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {    
        if (Input.GetButtonDown("Fire1") && magazine-- > 0)
        {
            ScopeRay();
            audioSource.Play();

            if (magazine <= 0)
            {
                StartCoroutine(Reload());
            }
        }

        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �ٴڰ� �浹�� ���¶��
            if (characterControl.collisionFlags == CollisionFlags.Below)
            {
                // ������ �� �� �ֵ��� �����մϴ�.
                direction.y = 20f;
            }
        }

        direction.y -= gravity * Time.deltaTime;

        characterControl.Move(transform.TransformDirection(direction) * speed * Time.deltaTime);

        mouseX += Input.GetAxis("Mouse X") * speed;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
    }

    private IEnumerator Reload()
    {
        animator.Play("Character_Reload");
 
        yield return new WaitForSeconds(0.01f);

        float curAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
  
        yield return new WaitForSeconds(curAnimationTime);

        magazine = 10;
    }

    public void ScopeRay()
    {
        RaycastHit hit;

        // ȭ���� �߾� ��ǥ (Cross Hair�� �������� Raycast�� �����մϴ�.)
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

        // ���� ��Ÿ� �ȿ� �ε����� ������Ʈ�� ������ target�� ������ �ε��� ��ġ�� �����մϴ�.
        if (Physics.Raycast(ray, out hit, distance, layer))
        {
            hit.collider.GetComponentInParent<Zombie>().health -= 20;

        }
        else if(Physics.Raycast(ray, out hit, distance))
        {
          
        }      
    }
}