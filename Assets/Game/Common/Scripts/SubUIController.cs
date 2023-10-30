using Cysharp.Threading.Tasks;
using RTLTMPro;
using UnityEngine;

namespace Game.Common.Scripts
{
    public class SubUIController : MonoBehaviour
    {
        private async UniTask SetAppearance(bool shouldAppear)
        {
            RTLTextMeshPro[] uiTexts = GetComponentsInChildren<RTLTextMeshPro>();
            foreach (var txt in uiTexts)
            {
                //txt.mesh.SetColors(Color.white);
            }
        }
    }
}
