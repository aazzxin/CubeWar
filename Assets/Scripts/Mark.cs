using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mark : MonoBehaviour {

    private Animator _ani;
    [SerializeField]
    private Text _timeText, _killText, _enemyDamageText, _playerDamageText;
    [SerializeField]
    private Text _timeMark, _killMark, _enemyDamageMark, _playerDamageMark, _totalMark;
    // Use this for initialization
    void Start () {
        float seconds = Globe.time / 60;
        int min = (int)seconds / 60;
        seconds -= min * 60;
        int s = (int)seconds;
        seconds -= s;
        int ms = (int)seconds * 100;
        _timeText.text = min + "min" + s + "s" + ms;
        int timeMark = 10 * ((600 - (int)(Globe.time / 60)));
        if (timeMark >= 0)
        {
            _timeMark.text = "+" + System.Convert.ToString(timeMark);
        }
        else
            _timeMark.text = System.Convert.ToString(timeMark);


        _killText.text = Globe.kill.ToString();
        int killMark = 10 * Globe.kill;
        _killMark.text = "+" + (killMark).ToString();
        _enemyDamageText.text = (Globe.ennemyDamage).ToString();
        int enemyDamageMark = Globe.ennemyDamage / 10;
        _enemyDamageMark.text = "+" + (enemyDamageMark).ToString();

        _playerDamageText.text = Globe.playerDamage.ToString();
        int playerDamageMark = -10 * Globe.playerDamage;
        _playerDamageMark.text = (playerDamageMark).ToString();

        int totalMark = (timeMark + killMark + enemyDamageMark + playerDamageMark);
        if (totalMark >= 0)
            _totalMark.text = "+" + totalMark.ToString();
        else
            _totalMark.text = totalMark.ToString();


        _ani=GetComponent<Animator>();
        _ani.SetBool("Open", true);
    }
}
