#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Pickup.Editor
{
    [InitializeOnLoadAttribute]
    public static class Initializer
    {
        static Initializer(){
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

        static void LoadDefaultScene(PlayModeStateChange state){
            if (state == PlayModeStateChange.ExitingEditMode) {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
            }

            if (state == PlayModeStateChange.EnteredPlayMode) {
                EditorSceneManager.LoadScene (0);
            }
        }
    }
}
#endif