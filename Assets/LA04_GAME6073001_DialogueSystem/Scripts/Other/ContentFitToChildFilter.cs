using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    public class ContentFitToChildFilter : MonoBehaviour
    {
        public void RefitSizeToContent()
        {
            transform.localPosition = Vector3.zero;
            GetComponent<RectTransform>().sizeDelta = Vector2.one;

            RectTransform childCont = transform.GetComponentInChildren<RectTransform>();

            float minX, maxX, minY, maxY;
            minX = maxX = transform.localPosition.x;
            minY = maxY = transform.localPosition.y;

            foreach (RectTransform child in childCont)
            {
                Vector2 scale = child.sizeDelta;
                float m_minX, m_maxX, m_minY, m_maxY;

                m_minX = child.localPosition.x - (scale.x / 2);
                m_maxX = child.localPosition.x + (scale.x / 2);
                m_minY = child.localPosition.y - (scale.y / 2);
                m_maxY = child.localPosition.y + (scale.y / 2);

                if (m_minX < minX)
                    minX = m_minX;
                if (m_maxX > maxX)
                    maxX = m_maxX;

                if (m_minY < minY)
                    minY = m_minY;
                if (m_maxY > maxY)
                    maxY = m_maxY;
            }

            GetComponent<RectTransform>().sizeDelta = new Vector2(maxX - minX, maxY - minY);
        }
    }
}