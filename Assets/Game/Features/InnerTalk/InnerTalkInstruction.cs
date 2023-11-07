using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class InnerTalkInstruction : MonoBehaviour
{
    [SerializeField] TextMeshPro txt;
    InnerTalkSentence sentence;
    void Start()
    {
        txt.color = Color.clear;
        sentence = GetComponent<InnerTalkSentence>();
        sentence.WordTapped += OnWordTap;

        Appear().Forget();
    }

    public async UniTask Appear()
    {
        await UniTask.Delay(6000);  // wait 6 seconds before appear
        float lerpTime = 0;
        float duration = 1f;

        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = t * t * t * (t * (6f * t - 15f) + 10f); // very smooth and nice step function

            txt.color = Color.Lerp(Color.clear, Color.white, t);

            await UniTask.Yield();
        }
    }

    private void OnWordTap(InnerTalkWord word)
    {
        Destroy(txt.gameObject);
        sentence.WordTapped -= OnWordTap;
    }
}
