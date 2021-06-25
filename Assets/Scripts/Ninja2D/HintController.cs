using TMPro;
using UnityEngine;

public class HintController : MonoBehaviour
{
    [TextArea(15, 10)]
    public string text;
    public TextMeshProUGUI textField;

    private Animator anim;

    void Start()
    {
        textField = GetComponentInChildren<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
        textField.text = text;
    }

    public void Activate()
    {
        anim.SetBool("IsOpen", true);
    }

    public void DeActivate()
    {
        anim.SetBool("IsOpen", false);
    }
}
