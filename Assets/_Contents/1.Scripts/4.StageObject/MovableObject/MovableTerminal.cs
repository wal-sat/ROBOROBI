using UnityEngine;

public class MovableTerminal : MonoBehaviour
{
    // ----- Private Methods -----

    private void OnTriggerStay2D(Collider2D collider)
    {
        MovableObject movableObject = collider.GetComponent<MovableObject>();
        if (collider.GetComponent<MovableObject>() == null) return;
        
        switch (movableObject.CurrentDirection)
        {
            case MovableObjectDirection.Up:
                if (collider.transform.position.y > this.transform.position.y)
                {
                    Destroy(collider.gameObject);
                }
                break;
            case MovableObjectDirection.Down:
                if (collider.transform.position.y < this.transform.position.y)
                {
                    Destroy(collider.gameObject);
                }
                break;
            case MovableObjectDirection.Left:
                if (collider.transform.position.x < this.transform.position.x)
                {
                    Destroy(collider.gameObject);
                }
                break;
            case MovableObjectDirection.Right:
                if (collider.transform.position.x > this.transform.position.x)
                {
                    Destroy(collider.gameObject);
                }
                break;
            case MovableObjectDirection.UpLeft:
                if (collider.transform.position.x < this.transform.position.x && collider.transform.position.y > this.transform.position.y)
                {
                    Destroy(collider.gameObject);
                }
                break;
            case MovableObjectDirection.UpRight:
                if (collider.transform.position.x > this.transform.position.x && collider.transform.position.y > this.transform.position.y)
                {
                    Destroy(collider.gameObject);
                }
                break;
            case MovableObjectDirection.DownLeft:
                if (collider.transform.position.x < this.transform.position.x && collider.transform.position.y < this.transform.position.y)
                {
                    Destroy(collider.gameObject);
                }
                break;
            case MovableObjectDirection.DownRight:
                if (collider.transform.position.x > this.transform.position.x && collider.transform.position.y < this.transform.position.y)
                {
                    Destroy(collider.gameObject);
                }
                break;
        }
    }
}
