﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class UIUtil : MonoBehaviour
{
    /// <summary>
    /// 创建面板，请求资源管理器
    /// </summary>
    public static void CreateUI(string pfbName, Transform parent, UnityAction<GameObject> func)
    {
        GameObject pfbCached = PanelStack.Instance.GetCachedPanel(pfbName);
        if (pfbCached)
        {
            GameObject goCached = GameObject.Instantiate(pfbCached) as GameObject;
            if (goCached == null) return;
            goCached.name = pfbName;
            goCached.layer = LayerMask.NameToLayer("UI");
            goCached.transform.SetParent(parent);
            goCached.transform.localScale = Vector3.one;
            goCached.transform.localPosition = Vector3.zero;
            goCached.transform.localRotation = Quaternion.identity;
            goCached.AddComponent<LShapBehaviour>();
            goCached.SetActive(false);
            if (func != null)
            {
                Util.DelayCallForEndOfFrame(() => { func(goCached); });
            }

            PanelStack.Instance.RemoveCachePanel(pfbName);
        }
        else
        {
            Global.AssetLoadManager.LoadUIPanel(pfbName, (prefab) =>
            {
                GameObject go = GameObject.Instantiate(prefab) as GameObject;
                if (go == null) return;
                go.name = pfbName;
                go.layer = LayerMask.NameToLayer("UI");
                go.transform.SetParent(parent);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.AddComponent<LShapBehaviour>();
                go.SetActive(false);
                if (func != null)
                {
                    Util.DelayCallForEndOfFrame(() => { func(go); });
                }
            });
        }
    }

    /// <summary>
    /// 显示对话窗
    /// </summary>
    public static void ShowDialogbox(string content, UnityAction confirmFunc)
    {
        DialogBox.Template(TemplateName.DialogBox).Show(
            buttons: new DialogActions()
            {
                            {"确认", () =>{ confirmFunc(); return true; }},
                            {"取消", DialogBox.Close},
            },
            message: content);
    }

    /// <summary>
    /// 显示提示窗
    /// </summary>
    public static void ShowTips(string content)
    {
        DialogBox.Template(TemplateName.DialogBox).Show(message: content, autoHide: true);
    }

    /// <summary>
    /// 清理UI缓存
    /// </summary>
    public static void ClearUICache()
    {
        PopupsManager.Instance.ClearCache();
        PanelStack.Instance.ClearStack();
    }

}
