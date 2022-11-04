using System.Collections;
using CardGame.Commands;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Visual
{
    public class MessageManager : MonoBehaviour
    {
        [SerializeField] private Text _messageText;
        [SerializeField] private GameObject _messagePanel;

        public static MessageManager Instance;

        private void Awake()
        {
            Instance = this;
            _messagePanel.SetActive(false);
        }

        public void ShowMessage(string message, float duration, Command command) =>
            StartCoroutine(ShowMessageCoroutine(message, duration, command));

        private IEnumerator ShowMessageCoroutine(string message, float duration, Command command)
        {
            _messageText.text = message;
            _messagePanel.SetActive(true);

            yield return new WaitForSeconds(duration);

            _messagePanel.SetActive(false);
            Command.CommandExecutionComplete();
        }
    }
}