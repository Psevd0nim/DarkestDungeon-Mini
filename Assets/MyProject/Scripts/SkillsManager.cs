using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    private Game_UI_Manager _game_UI_Manager;

    public void Init(Game_UI_Manager game_UI_Manager)
    {
        _game_UI_Manager = game_UI_Manager;
    }

    public void ShowAvaialbeSkills(List<Skill> skills)
    {
        //_game_UI_Manager.ShowAvaialbeSkills(skills);

    }

    public void ShowSkills(List<Skill> skills)
    {
        //_game_UI_Manager.ShowSkills(skills);
    }


}