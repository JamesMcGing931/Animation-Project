
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private Transform RagdollRoot;
    [SerializeField]
    private bool StartRagdoll = false;
    // Only public for Ragdoll Runtime GUI for explosive force
    public Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;

    [SerializeField]
    private RagdollEnabler[] Ragdolls;
    [SerializeField]
    private float ExplosiveForce = 5_000;
    [SerializeField]
    private float UpwardModifier = 2;
    [SerializeField]
    private float ExplosiveRadius = 2f;

    private void Awake()
    {
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();

        if (StartRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }
    }

    public void EnableRagdoll()
    {
        Animator.enabled = false;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
        foreach (Collider collider in Colliders)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
    }

    public void EnableAnimator()
    {
        Animator.enabled = true;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;
        }
        foreach (Collider collider in Colliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            EnableRagdoll();
/*            Explode();
*/        }
    }

    private void Explode()
    {
        foreach (RagdollEnabler ragdoll in Ragdolls)
        {
            foreach (Rigidbody rigidbody in ragdoll.Rigidbodies)
            {
                rigidbody.AddExplosionForce(ExplosiveForce, ragdoll.transform.position, ExplosiveRadius, UpwardModifier);
            }
        }
    }
}
