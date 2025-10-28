using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDropPlatform : MonoBehaviour
{
    public bool fallThrough;
    [SerializeField] private GameInput gameInput;

    private void Start()
    {
        gameInput.OnDropStart += GameInput_OnDropStart;
        gameInput.OnDropEnd += GameInput_OnDropEnd;
    }

    

    private void GameInput_OnDropStart(object sender, System.EventArgs e)
    {
        fallThrough = true;
    }

    private void GameInput_OnDropEnd(object sender, System.EventArgs e)
    {
        fallThrough = false;
    }

    private void Update()
    {

    }
}
