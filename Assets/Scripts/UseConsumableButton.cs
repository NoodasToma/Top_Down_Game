using UnityEngine;
using UnityEngine.UI;

public class UseConsumableButton : MonoBehaviour
{
    public ConsumableSO item;
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(UseItem);
    }

    public void UseItem()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var handler = player.GetComponent<ConsumableHandler>();
            if (handler != null)
                handler.Consume(item);
        }
    }
}
