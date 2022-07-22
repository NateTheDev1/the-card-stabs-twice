using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CST
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private bool hasGameStarted = false;
        [SerializeField] private GameObject IntroPanel;
        [SerializeField] private TextMeshProUGUI HealthText;
        [SerializeField] private TextMeshProUGUI RoundText;

        [SerializeField] GameObject TitleNotifierText;

        public int player1Health = 10;
        public int player2Health = 10;


        public int stage = 0;

        public void OnStartGame()
        {
            this.stage = 1;
            player1Health = 10;
            player2Health = 10;
            IntroPanel.SetActive(false);
            FireTitleNotification("Dealing Cards");
            UpdateHUD();
        }

        private void DrawCardsRoutine()
        {

        }

        private void FireTitleNotification(string text)
        {
            TitleNotifierText.GetComponent<TextMeshProUGUI>().text = text;
            TitleNotifierText.GetComponent<Animation>().Play();
        }

        private void UpdateHUD()
        {
            this.HealthText.text = player1Health.ToString() + " - " + player2Health.ToString();
            this.RoundText.text = "Round " + stage;
        }
    }

}