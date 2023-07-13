using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "Player/PlayerSO")]
    public class PlayerSO : ScriptableObject
    {
       public string player;
       public int level;
    }
}
