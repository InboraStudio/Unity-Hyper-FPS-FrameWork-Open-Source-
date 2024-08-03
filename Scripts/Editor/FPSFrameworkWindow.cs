using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public class FPSFrameworkWindow : EditorWindow
    {
        //this script needs a lot of work it's not final

        //[MenuItem(MenuItemPaths.Help)]
        public static void Draw()
        {
            EditorWindow window = GetWindow(typeof(FPSFrameworkWindow));

            window.titleContent = new GUIContent("FPS Engine Helper");
            window.minSize = new Vector2(276, 271);
        }

        private void OnGUI()
        {
            //nothing here but this will have links to coming documntation, discord, youtube etc.. and some welcome text 
        }
    }
}