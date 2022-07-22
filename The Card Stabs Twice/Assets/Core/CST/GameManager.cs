using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CST
{

    public enum CardColors
    {
        Red,
        Black,
    }

    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private bool hasGameStarted = false;
        [SerializeField] private GameObject IntroPanel;
        [SerializeField] private GameObject HandView;
        [SerializeField] private GameObject BattleView;

        [SerializeField] private GameObject HandPickView;

        [SerializeField] private GameObject HandViewContainer;
        [SerializeField] private GameObject CardInHandPrefab;

        [SerializeField] private TextMeshProUGUI HealthText;
        [SerializeField] private TextMeshProUGUI RoundText;
        [SerializeField] private TextMeshProUGUI TargetColorText;


        [SerializeField] GameObject TitleNotifierText;

        [SerializeField] private List<Card> deck = new List<Card>();

        [Header("Pick Card Config")]
        [SerializeField] private GameObject ChooseViewContainer;
        [SerializeField] private GameObject ChooseCardPrefab;
        [SerializeField] private TextMeshProUGUI CountdownText;
        [SerializeField] private GameObject SelectedCard;

        [SerializeField] private TextMeshProUGUI BuffText;

        [SerializeField] private List<Card> player1Buffs = new List<Card>();
        [SerializeField] private List<Card> player2Buffs = new List<Card>();

        private Card player1CurrentPickedCard;
        private Card player2CurrentPickedCard;

        private bool player1LockedIn;
        private bool player2LockedIn;

        public int player1Health = 10;
        private List<Card> player1Hand = new List<Card>();

        public int player2Health = 10;
        private List<Card> player2Hand = new List<Card>();

        [SerializeField] private CardColors currentColor;

        private static System.Random rng = new System.Random();

        public int stage = 0;
        public int round = 1;

        private float secondsToPick = 15;

        public void OnStartGame()
        {
            deck = ShuffleList<Card>(deck);
            this.stage = 1;
            this.round = 1;
            player1Health = 10;
            player2Health = 10;
            IntroPanel.SetActive(false);
            FireTitleNotification("Dealing Cards");
            this.HandView.SetActive(true);
            DrawCardsRoutine();
            UpdateHUD();
            ChooseAndSetRandomColor();
            StartCoroutine(WaitForSecondsGeneric(round == 1 ? 10 : 3, () =>
            {
                PickCardRoutine();
            }));
        }

        public void ChooseCard(Card card)
        {
            if (!player1LockedIn)
            {

                player1CurrentPickedCard = card;
                SelectedCard.SetActive(true);
                SelectedCard.GetComponent<UnityEngine.UI.Image>().sprite = player1CurrentPickedCard.cardFaceImage;
            }
        }

        public void LockInCard()
        {
            if (player1CurrentPickedCard != null && !player1LockedIn)
            {
                player1LockedIn = true;
                player1Buffs.Add(player1CurrentPickedCard);
                OnBuffsChanged();
                OnChooseEnd();
            }
        }

        private void OnChooseEnd()
        {
            this.HandView.SetActive(false);
            this.HandPickView.SetActive(false);
            System.Random rand = new System.Random();

            int index = rand.Next(player2Hand.Count);
            player2CurrentPickedCard = player2Hand[index];
            player2Buffs.Add(player2CurrentPickedCard);
            ShakeBehavior.Instance.TriggerShake(0.2f);

            BattleView.SetActive(true);
        }

        private void OnBuffsChanged()
        {
            BuffText.text = string.Empty;

            foreach (Card card in player1Buffs)
            {
                BuffText.text += card.buffDescription + "\n";
            }
        }

        IEnumerator WaitForSecondsGeneric(float seconds, System.Action callback)
        {
            yield return new WaitForSeconds(seconds);
            if (callback != null) callback();
        }


        private void PickCardRoutine()
        {
            SelectedCard.SetActive(false);
            HandPickView.SetActive(true);
            player1CurrentPickedCard = null;
            player1LockedIn = false;
            player2CurrentPickedCard = null;
            player2LockedIn = false;

            foreach (Transform tf in ChooseViewContainer.transform)
            {
                Destroy(tf.gameObject);
            }

            foreach (Card card in player1Hand)
            {
                GameObject go = Instantiate(ChooseCardPrefab);

                go.GetComponent<CardInstance>().cardData = card;
                go.GetComponent<CardInstance>().Activate();
                go.GetComponent<Animation>().Play();
                go.transform.SetParent(ChooseViewContainer.transform, false);
            }

            if (round != 1) round++;
            secondsToPick = 15;
            CountdownText.text = secondsToPick.ToString() + " seconds remaining";
            StartCoroutine(SecondsToPickCountdown());
        }

        IEnumerator SecondsToPickCountdown()
        {
            yield return new WaitForSeconds(1);
            secondsToPick -= 1;
            CountdownText.text = secondsToPick.ToString() + " seconds remaining";

            if (secondsToPick > 0 && !player1LockedIn)
            {
                StartCoroutine(SecondsToPickCountdown());
            }
            else
            {
                OnSecondsToPickEnd();
            }
        }

        private void OnSecondsToPickEnd()
        {
            if (!player1LockedIn)
            {
                System.Random rand = new System.Random();

                int index = rand.Next(player1Hand.Count);
                player1CurrentPickedCard = player1Hand[index];

                OnChooseEnd();
            }
        }

        private void OnGiveCard()
        {
            foreach (Transform tf in HandViewContainer.transform)
            {
                Destroy(tf.gameObject);
            }

            StartCoroutine(WaitToShowCardAdded());
        }

        private void GiveCard(Card card)
        {
            GameObject go = Instantiate(CardInHandPrefab);

            go.GetComponent<CardInstance>().cardData = card;
            go.GetComponent<CardInstance>().Activate();
            go.GetComponent<Animation>().Play();
            go.transform.SetParent(HandViewContainer.transform, false);
        }

        IEnumerator WaitToShowCardAdded(int index = 0)
        {
            yield return new WaitForSeconds(1);
            GiveCard(player1Hand[index]);

            if (index < player1Hand.Count - 1)
            {
                StartCoroutine(WaitToShowCardAdded(index + 1));
            }
        }

        private List<T> ShuffleList<T>(List<T> listToShuffle)
        {
            int n = listToShuffle.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = listToShuffle[k];
                listToShuffle[k] = listToShuffle[n];
                listToShuffle[n] = value;
            }
            return listToShuffle;
        }

        private void ChooseAndSetRandomColor()
        {
            var values = CardColors.GetValues(typeof(CardColors));
            System.Random rand = new System.Random();
            CardColors randomBar = (CardColors)values.GetValue(rand.Next(values.Length));
            currentColor = randomBar;
            TargetColorText.text = "Target Color: " + currentColor;
        }

        private Card GetRandomCard()
        {
            System.Random rand = new System.Random();

            int index = rand.Next(deck.Count);
            return deck[index];
        }

        private void DrawCardsRoutine()
        {
            for (int i = 0; i < 8; i++)
            {
                player1Hand.Add(GetRandomCard());
                player2Hand.Add(GetRandomCard());
            }

            OnGiveCard();
        }

        private void FireTitleNotification(string text)
        {
            TitleNotifierText.GetComponent<TextMeshProUGUI>().text = text;
            TitleNotifierText.GetComponent<Animation>().Play();
        }

        private void UpdateHUD()
        {
            this.HealthText.text = player1Health.ToString() + " - " + player2Health.ToString();
            this.RoundText.text = "Round " + round;
        }
    }

}