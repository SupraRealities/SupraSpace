using UnityEngine;

public class SharedData : MonoBehaviour
{
    private static SharedData instance;
    public static SharedData Instance => instance;

    public float calibrationAngleDifference;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
