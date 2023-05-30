using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

// From Fusion tutorial: https://doc.photonengine.com/fusion/current/tutorials/shared-mode-basics/3-movement-and-camera
public class PlayerMovement : NetworkBehaviour {
    [SerializeField] float speed = 2f;

    [SerializeField] InputAction moveAction;
    private void OnEnable() { moveAction.Enable(); }
    private void OnDisable() { moveAction.Disable(); }
    void OnValidate() {
        // Provide default bindings for the input actions.
        // Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Button);
        if (moveAction.bindings.Count == 0)
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
    }


    private CharacterController _controller;
    public override void Spawned() {
        _controller = GetComponent<CharacterController>();
    }

    private Vector3 velocity = new Vector3(0, 0, 0);
    public override void FixedUpdateNetwork() {
        // Only move own player and not every other player. Each player controls only its own player object.
        if (!HasStateAuthority) {
            return;
            // NetworkTransform only synchronizes changes from the StateAuthority.             
            // If someone without StateAuthority tries to change, the change will be local, and not transmitted to other players.
        }

        Vector3 movement = moveAction.ReadValue<Vector2>(); 
        velocity.x = movement.x * speed;
        velocity.z = movement.y * speed;
        _controller.Move(velocity * Runner.DeltaTime);

        //if (move != Vector3.zero) {
        //   gameObject.transform.forward = move;
        //}
    }
}


