using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColor : NetworkBehaviour {
    [Tooltip("The mesh whose color should be changed.")]
    [SerializeField] MeshRenderer meshRendererToChange;

    [SerializeField] InputAction colorAction;
    private void OnEnable() {        colorAction.Enable();    }
    private void OnDisable() {        colorAction.Disable();    }
    void OnValidate() {
        // Provide default bindings for the input actions. Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (colorAction == null)
            colorAction = new InputAction(type: InputActionType.Button);
        if (colorAction.bindings.Count == 0)
            colorAction.AddBinding("<Keyboard>/C");
    }

    [Networked(OnChanged = nameof(NetworkColorChanged))]
    public Color NetworkedColor { get; set; }
    private static void NetworkColorChanged(Changed<PlayerColor> changed) {
        changed.Behaviour.meshRendererToChange.material.color = changed.Behaviour.NetworkedColor;
    }

    void Update() {
        if (!HasStateAuthority) return;
        if (colorAction.WasPerformedThisFrame()) {
            var randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
            // meshRendererToChange.material.color = randomColor;
            NetworkedColor = randomColor;
        }
    }
}
