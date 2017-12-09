using UnityEngine;
using System.Collections;

namespace Orange.TransitionKit
{
    public interface TransitionKitDelegate
    {

        Shader shaderForTransition();

        Mesh meshForDisplay();

        Texture2D textureForDisplay();

        IEnumerator onScreenObscured(TransitionEngine transitionKit);
    }
}
