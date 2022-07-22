using UnityEngine;

namespace CST
{
    [CreateAssetMenu(fileName = "Card", menuName = "The Card Stabs Twice/Card", order = 0)]
    public class Card : ScriptableObject
    {
        public int ID = 0;
        public string cardName;
        public int damage = 1;
        public CardColors cardColor;
        public string buffDescription;
        public int effectForRounds = 1;
        public bool ignoreEffectForRoundsMakeInfinite = false;
        public string effectTypeID;
        public Sprite cardFaceImage;
    }
}