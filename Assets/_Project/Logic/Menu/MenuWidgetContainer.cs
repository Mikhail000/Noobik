using System;
using System.Collections.Generic;
using UnityEngine;
using YG;


namespace _Project.Logic.Menu
{
    public class MenuWidgetContainer : MonoBehaviour
    {
        [SerializeField] private List<LvlButtonWidget> widgets;
        
        private void Awake()
        {
            UpdateWidgets();
        }

        public void UpdateWidgets()
        {
            Debug.Log(YG2.saves.openLevels.Length);
            Debug.Log(widgets.Count);

            if (YG2.saves.openLevels.Length != widgets.Count)
            {
                throw new ArgumentException("The lengths of openLevels and levels arrays must be the same.");
            }
            
            for (int i = 0; i < YG2.saves.openLevels.Length; i++)
            {
                if (YG2.saves.openLevels[i])
                {
                    widgets[i].UpdateWidgetState(true);
                }
                else
                {
                    widgets[i].UpdateWidgetState(false);
                }
            }
            
        }
    }
}
