using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 2.5f;
    public Transform interactionTransform;

    public virtual bool Interact(Transform playerTransform)
    {
        Debug.Log("Interacting with " + interactionTransform.name);
        float distance = Vector3.Distance(playerTransform.position, interactionTransform.position);
        if (distance > radius)
        {
            Debug.Log("It is too far away to interact with " + interactionTransform.name);
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
