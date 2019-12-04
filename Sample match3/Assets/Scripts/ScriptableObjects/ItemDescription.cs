using UnityEngine;

namespace MatchThree.Objects
{
    [CreateAssetMenu (fileName = "New Item", menuName = "Create new item", order = 51)]
    public class ItemDescription : ScriptableObject
    {
        [SerializeField]
        [Tooltip ("Целое число больше нуля")]
        private int _id;

        [SerializeField]
        private Sprite _icon;
        
        public int ID { get => _id; }
        public Sprite Icon { get => _icon; }
    }
}
