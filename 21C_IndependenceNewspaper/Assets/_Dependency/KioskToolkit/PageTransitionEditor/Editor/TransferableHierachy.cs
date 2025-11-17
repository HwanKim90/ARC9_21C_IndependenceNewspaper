using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Arc9.Unity.KioskToolkit
{
    [InitializeOnLoad]
    public static class TransferableHierachy
    {
        public struct HierarchyItem
        {
            public int InstanceID { get; }
            public bool IsSelected { get; }
            public bool IsHovered { get; }

            public Rect BackgroundRect { get; }
            public Rect TextRect { get; }
            public Rect CollapseToggleIconRect { get; }
            public Rect PrefabIconRect { get; }
            public Rect EditPrefabIconRect { get; }

            public HierarchyItem(int instanceID, Rect selectionRect)
            {
                InstanceID = instanceID;
                IsSelected = Selection.Contains(instanceID);

                float xPos = selectionRect.position.x + 60f - 28f - selectionRect.xMin;
                float yPos = selectionRect.position.y;
                float xSize = selectionRect.size.x + selectionRect.xMin + 28f - 60 + 16f;
                float ySize = selectionRect.size.y;
                BackgroundRect = new Rect(xPos, yPos, xSize, ySize);

                xPos = selectionRect.position.x + 18f;
                yPos = selectionRect.position.y;
                xSize = selectionRect.size.x - 18f;
                ySize = selectionRect.size.y;
                TextRect = new Rect(xPos, yPos, xSize, ySize);

                xPos = selectionRect.position.x - 14f;
                yPos = selectionRect.position.y + 1f;
                xSize = 13f;
                ySize = 13f;
                CollapseToggleIconRect = new Rect(xPos, yPos, xSize, ySize);

                xPos = selectionRect.position.x;
                yPos = selectionRect.position.y;
                xSize = 16f;
                ySize = 16f;
                PrefabIconRect = new Rect(xPos, yPos, xSize, ySize);

                xPos = BackgroundRect.xMax - 16f;
                yPos = selectionRect.yMin;
                xSize = 16f;
                ySize = 16f;
                EditPrefabIconRect = new Rect(xPos, yPos, xSize, ySize);

                IsHovered = BackgroundRect.Contains(Event.current.mousePosition);
            }
        }

        static TransferableHierachy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        //https://github.com/NCEEGEE/PrettyHierarchy
        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            UnityEngine.Object instance = EditorUtility.InstanceIDToObject(instanceID);

            if (instance != null)
            {
                PageTransitionBase pageTransitionBase = (instance as GameObject).GetComponent<PageTransitionBase>();

                if (pageTransitionBase != null)
                {

                    HierarchyItem item = new HierarchyItem(instanceID, selectionRect);
                    /*
                    PainBackground(item);
                    PaintHoverOverlay(item);
                    PaintText(item);
                    PaintCollapseToggleIcon(item);
                    PaintPrefabIcon(item);
                    PaintEditPrefabIcon(item);
                    */
                    if (!pageTransitionBase.gameObject.activeSelf)
                    {
                        PaintText(item, pageTransitionBase.name, Color.red);
                    }
                    else
                    {
                        PaintText(item, pageTransitionBase.name, Color.green);
                    }

                }

                TransferableElement transferableElement = (instance as GameObject).GetComponent<TransferableElement>();

                if (transferableElement != null)
                {

                    HierarchyItem item = new HierarchyItem(instanceID, selectionRect);
                    /*
                    PainBackground(item);
                    PaintHoverOverlay(item);
                    PaintText(item);
                    PaintCollapseToggleIcon(item);
                    PaintPrefabIcon(item);
                    PaintEditPrefabIcon(item);
                    */
                    //transferableElement.is
                    if(transferableElement.transform.parent != null)
                    {
                        if((transferableElement.transform.parent as Transform).gameObject.activeSelf)
                        {
                            PaintText(item, transferableElement.name, Color.cyan);
                        }
                        else{
                            PaintText(item, transferableElement.name, Color.magenta);
                        }
                    }
                    else
                    {
                        PaintText(item, transferableElement.name, Color.cyan);
                    }
                }
            }
        }

        private static void PaintText(HierarchyItem item, string txt, Color color)
        {
            GUIStyle labelGUIStyle = new GUIStyle
            {
                normal = new GUIStyleState { textColor = color },
            };

            EditorGUI.LabelField(item.TextRect, txt, labelGUIStyle);
        }

    }
}