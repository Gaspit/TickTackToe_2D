using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Main _main;

    private void OnMouseDown()
    {
        _main.SetCross(_id);
    }
}
