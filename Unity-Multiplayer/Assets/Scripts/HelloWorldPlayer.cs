using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<int> Health = new NetworkVariable<int>();
        public NetworkVariable<Color> CurrColor = new NetworkVariable<Color>();

        public override void OnNetworkSpawn()
        {
            Debug.Log("OnNetworkSpawn called");

            Health.OnValueChanged += OnHealthChanged;
            CurrColor.OnValueChanged += OnColorChanged;

            if (IsServer)
            {
                Health.Value = 10;

                // Set the color of the player to random color
                CurrColor.Value = Random.ColorHSV();

                // Call the ChangeHealth coroutine
                StartCoroutine(ChangeHealth(10));
            }

            if (IsOwner)
            {
                transform.position = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
            }

            GetComponent<Renderer>().material.color = CurrColor.Value;
        }

        public override void OnNetworkDespawn()
        {
            Health.OnValueChanged -= OnHealthChanged;
            CurrColor.OnValueChanged -= OnColorChanged;
        }

        public void OnHealthChanged(int previous, int current)
        {
            if (Health.Value != previous)
            {
                Debug.Log("OnStateChanged: " + previous + " -> " + current);
            }
            if (Health.Value <= 0)
            {
                Debug.Log("Player died");
                DespawnPlayerServerRpc();
            }
        }

        public void OnColorChanged(Color previous, Color current)
        {
            if (CurrColor.Value != previous)
            {
                Debug.Log("OnColorChanged: " + previous + " -> " + current);
                GetComponent<Renderer>().material.color = CurrColor.Value;
            }
        }

        [Rpc(SendTo.Server)]
        public void DespawnPlayerServerRpc()
        {
            Destroy(gameObject);
            NetworkObject.Despawn(true);
        }

        public IEnumerator ChangeHealth(int newHealth)
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                Health.Value -= 1;
            }
        }
    }
}