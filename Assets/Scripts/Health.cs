using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] NumberField HealthDisplay;

    [Networked(OnChanged = nameof(NetworkedHealthChanged))]
    public int NetworkedHealth { get; set; } = 100;
    private static void NetworkedHealthChanged(Changed<Health> changed) {
        // Here you would add code to update the player's healthbar.
        Debug.Log($"Health changed to: {changed.Behaviour.NetworkedHealth}");
        changed.Behaviour.HealthDisplay.SetNumber(changed.Behaviour.NetworkedHealth);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    // All players can call this function; only the StateAuthority receives the call.
    public void DealDamageRpc(int damage) {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        NetworkedHealth -= damage;
    }
}
