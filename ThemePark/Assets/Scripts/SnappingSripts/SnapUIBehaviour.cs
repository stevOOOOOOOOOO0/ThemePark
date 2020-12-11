﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//[ExecuteInEditMode]
//[CustomEditor(typeof(PositionHandle)),CanEditMultipleObjects]
public class SnapUIBehaviour : MonoBehaviour
{
    #region ---------------- Variables ----------------
    
    [Header("Snapping Options")]
    [SerializeField]
    private Vector3SO positionVectorSO;
    //[SerializeField]
    //[ListToPopup(typeof(Pop))]
    
    [SerializeField][Range(20.0f, 400.0f)]
    private float _snapRadius = 20f;
    
    [Header("Visualization Options")]
    [SerializeField]
    private bool _ShowVisualization = true;
    
    /// <summary>
    /// Not in editor vars
    /// </summary>
    private RectTransform _thisObjRectTransform;
    
    #endregion

    #region ---------------- Setup ----------------

    private void Start()
    {
        _thisObjRectTransform = GetComponent<RectTransform>();
        
    }

    #endregion
    
    #region ---------------- Work Stuff ----------------

    //Snap this object to the position set in the vector 3 sciptable obj
    private void SnapToAnchor(Vector3 target, Vector3 position)
    {
        position = target;
    }

    private void SetPosition(Vector3 target,Transform thispos)
    {
        //thispos.position = new Vector3(target.x,target.y,target.z);
        thispos.position = target;
    }
    //Check the distance between two vector3s 
    private bool DistanceCheck(Vector3 one,Vector3 two, float distance)
    {
        bool _isWithinRad;
        if (Vector3.Distance(one, two) < distance)
        {
            _isWithinRad = true;
            return _isWithinRad;
        }
        else
        {
            _isWithinRad = false;
            return _isWithinRad;
        }
    }
    //populate popup menu
    private void CreateList()
    {
        
    }
    private void Update()
    {
        if (DistanceCheck(positionVectorSO.Position, transform.position, _snapRadius))
        {
            Debug.Log("ping");
            //SnapToAnchor(positionVectorSO.Position,_thisObjRectTransform.position);
            SetPosition(positionVectorSO.Position,transform);
        }
    }
    
    #endregion
    
    #region ---------------- Visualization ----------------
    private void VisualizeTarget(Vector3 target, float radius)
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(target,radius);
    }

    private void OnDrawGizmos()
    {
        if (_ShowVisualization)
        {
            VisualizeTarget(positionVectorSO.Position,_snapRadius);
        }
    }
    #endregion

    #region ---------------- Inspector Stuff ----------------

    /*
    public class ListToPopupAttribute : PropertyAttribute
    {
        public Type myType;
        public string propertyName;

        public ListToPopupAttribute(Type _myType, string _propertyName)
        {
            myType = _myType;
            propertyName = _propertyName;
        }
    }
    [CustomPropertyDrawer(typeof(ListToPopupAttribute))]
    public class ListPopupDrawer : PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ListToPopupAttribute atb = attribute as ListToPopupAttribute;
            List<string> stringList = null;

            if (atb.myType.GetField(atb.propertyName) != null)
            {
                stringList = atb.myType.GetField(atb.propertyName).GetValue(atb.myType) as List<string>;
            }

            if (stringList != null && stringList.Count != 0)
            {
                int selectedIndex = Mathf.Max(stringList.IndexOf(property.stringValue),0);
                EditorGUI.Popup(position, property.name, selectedIndex, stringList.ToArray());
                property.stringValue = stringList[selectedIndex];
            }
            else EditorGUI.PropertyField(position,property,label);
            {
                
            }
        }
    }

    public class PopUpMaker : MonoBehaviour
    {
        public static List<string> myList;

        [ListToPopup(typeof(PopUpMaker), "myList")]
        public string Popup;
        [ContextMenu("create Number list")]
        public void CreateNumberList()
        {
            myList = new List<string> {"1","2","3"};
        }
    }
    */
    

    #endregion
}
