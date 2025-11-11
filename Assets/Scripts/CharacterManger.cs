using UnityEngine;

public class CharacterManger : MonoBehaviour
{
    private static CharacterManger _instance;

    public static CharacterManger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManger").AddComponent<CharacterManger>();
            }
            return _instance; 
        } 
    }

    public Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
        { //기존의 인스턴스 안에 있는 것과 내가 다르다면 현재 것을 파괴
            Destroy(gameObject);
        }
    }
}
