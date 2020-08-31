using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class Playtest : Editor
{
    private float testSpeed = 2;
    [SerializeField] private Player player = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginVertical();
            GUILayout.Label("Playtesting Options");

            GUILayout.BeginHorizontal();
                if (GUILayout.Button("Toggle test speed"))
                {
                    Time.timeScale = Time.timeScale > 0 ? Time.timeScale != testSpeed ? testSpeed : 1 : Time.timeScale;
                }

                testSpeed = EditorGUILayout.Slider(testSpeed, 2, 8);
            GUILayout.EndHorizontal();
      
            if (GUILayout.Button("Enable God mode"))
            {
                player.lives = 1000000;
                Debug.Log("The player now has infinite lives.");
            }
        GUILayout.EndVertical();
    }
}
#endif
