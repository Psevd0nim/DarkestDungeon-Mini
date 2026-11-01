using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace MyProject
{
    public class Game_UI_Manager : MonoBehaviour
    {
        public CharacterSequence_UI CharacterSequence_UI => _characterSequence_UI;
        public SkillBoard_UI SkillBoard_UI => _skillBoard_UI;

        [SerializeField] private SkillBoard_UI _skillBoard_UI;
        [SerializeField] private CharacterSequence_UI _characterSequence_UI;
        [SerializeField] private GameObject _gamePanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private TextMeshProUGUI _characterInfo;
        [SerializeField] private Button _resume;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _leave;

        private FightManager _fightManager;

        public void Init(Factory factory, FightManager fightManager)
        {
            _fightManager = fightManager;
            _skillBoard_UI.Init(factory);
            _characterSequence_UI.Init(factory);
            _resume.onClick.AddListener(_fightManager.Resume);
            _restart.onClick.AddListener(_fightManager.RestartFight);
            _leave.onClick.AddListener(_fightManager.LeaveBattle);
            SetPaused(false);
        }

        private void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current == null)
            {
                return;
            }

            if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                _fightManager.TogglePause();
            }
        }

        public void ShowCharacter(Character character)
        {
            _characterInfo.text = character.DisplayName
                + "\nHP " + character.Health + "/" + character.MaxHealth
                + "\nSpeed " + character.Speed + "\nAttack " + character.Attack + "\nDefense " + character.Defense;
        }

        public void SetPaused(bool paused)
        {
            _gamePanel.SetActive(!paused);
            _pausePanel.SetActive(paused);
        }
    }
}
