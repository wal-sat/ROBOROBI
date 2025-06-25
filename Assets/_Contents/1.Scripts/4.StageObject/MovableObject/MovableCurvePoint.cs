using UnityEngine;

public class MovableCurvePoint : MonoBehaviour
{
    [SerializeField] private MovableObjectDirection _direction;

    // ----- Private Methods -----

    private void OnTriggerStay2D(Collider2D collider)
    {
        MovableObject movableObject = collider.GetComponent<MovableObject>();
        if (movableObject == null) return;
        
        switch (movableObject.CurrentDirection)
        {
            case MovableObjectDirection.Up:
                if (collider.transform.position.y > this.transform.position.y && !movableObject.IsChangedDirection)
                {
                    movableObject.IsChangedDirection = true;
                    ChangeDirection(movableObject, collider);
                }
                break;
            case MovableObjectDirection.Down:
                if (collider.transform.position.y < this.transform.position.y && !movableObject.IsChangedDirection)
                {
                    movableObject.IsChangedDirection = true;
                    ChangeDirection(movableObject, collider);
                }
                break;
            case MovableObjectDirection.Left:
                if (collider.transform.position.x < this.transform.position.x && !movableObject.IsChangedDirection)
                {
                    movableObject.IsChangedDirection = true;
                    ChangeDirection(movableObject, collider);
                }
                break;
            case MovableObjectDirection.Right:
                if (collider.transform.position.x > this.transform.position.x && !movableObject.IsChangedDirection)
                {
                    movableObject.IsChangedDirection = true;
                    ChangeDirection(movableObject, collider);
                }
                break;
            case MovableObjectDirection.UpLeft:
                if (collider.transform.position.x < this.transform.position.x && collider.transform.position.y > this.transform.position.y && !movableObject.IsChangedDirection)
                {
                    movableObject.IsChangedDirection = true;
                    ChangeDirection(movableObject, collider);
                }
                break;
            case MovableObjectDirection.UpRight:
                if (collider.transform.position.x > this.transform.position.x && collider.transform.position.y > this.transform.position.y && !movableObject.IsChangedDirection)
                {
                    movableObject.IsChangedDirection = true;
                    ChangeDirection(movableObject, collider);
                }
                break;
            case MovableObjectDirection.DownLeft:
                if (collider.transform.position.x < this.transform.position.x && collider.transform.position.y < this.transform.position.y && !movableObject.IsChangedDirection)
                {
                    movableObject.IsChangedDirection = true;
                    ChangeDirection(movableObject, collider);
                }
                break;
            case MovableObjectDirection.DownRight:
                if (collider.transform.position.x > this.transform.position.x && collider.transform.position.y < this.transform.position.y && !movableObject.IsChangedDirection)
                {
                    movableObject.IsChangedDirection = true;
                    ChangeDirection(movableObject, collider);
                }
                break;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        MovableObject movableObject = collider.GetComponent<MovableObject>();
        if (movableObject != null)
        {
            movableObject.IsChangedDirection = false;
        }
    }

    private void ChangeDirection(MovableObject movableObject, Collider2D collider)
    {
        movableObject.SetDirection(_direction);
        collider.transform.position = this.transform.position;
    }
}
