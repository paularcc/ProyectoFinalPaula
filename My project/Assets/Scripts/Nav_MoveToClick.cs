using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav_MoveToClick : MonoBehaviour
{
    public Camera cam;
    private Animator animator;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) //Botón izdo ratón
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

        }

    }
}
