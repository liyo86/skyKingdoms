using SO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private PlayerSO       _playerSO;
    [SerializeField] private GameObject     _playerPrefabBoy;
    [SerializeField] private GameObject     _playerPrefabGirl;

    public PlayerSO PlayerSO
    {
        get => _playerSO;
        set => _playerSO = value;
    }

    public void PlayerInstantation()
    {
        if(_playerSO.player == "B")
            Instantiate( _playerPrefabBoy, new Vector3(0f, 0f, 0f), Quaternion.identity );
        else if (_playerSO.player == "G")
            Instantiate( _playerPrefabGirl, new Vector3(0f, 0f, 0f), Quaternion.identity );
    }
}
