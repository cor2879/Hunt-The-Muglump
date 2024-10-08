﻿using UnityEngine;

namespace Interface.Elements.Scripts
{
    /// <summary>
    /// Extend this class to create a Panel
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BasePanel : MonoBehaviour
    {
        /// <summary>
        /// The CanvasGroup component of this object
        /// </summary>
        protected CanvasGroup cg;

        /// <summary>
        /// Is the panel showing
        /// </summary>
        public bool IsShowing => cg.alpha > 0.9;

        protected virtual void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        protected virtual void Update()
        {
            
        }

        public virtual void Show(CanvasSide side)
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            cg.Show(side);
        }

        public virtual void Hide(CanvasSide side)
        {
            this.transform.localScale = Vector3.zero;
            cg.Hide(side);
        }
    }
}