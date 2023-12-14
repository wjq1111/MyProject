using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public enum FormLayer
{
    InvalidForm = -1,
    LobbyForm,
    StartGameForm,
}

public class FormManager : MonoSingleton<FormManager>
{
    private int formUILayerID = 123456789;
    private string formPath = "Prefabs/Forms/";
    private Dictionary<FormLayer, int> openingForms = new Dictionary<FormLayer, int>();

    protected override void Init()
    {
        EventManager.Instance.AddEventListener(EventId.OnClickStartGameButton, PrepareLoadGameForm);
    }

    protected override void Uninit()
    {
        base.Uninit();
        EventManager.Instance.RemoveEventListener(EventId.OnClickStartGameButton, PrepareLoadGameForm);
    }

    private void PrepareLoadGameForm()
    {
        DestroyPrefab("LobbyForm");
        LoadPrefab("StartGameForm");
    }

    private void Start()
    {
        LoadPrefab("LobbyForm");
    }

    private FormLayer StringToFormLayer(string formName)
    {
        FormLayer layer;
        try
        {
            layer = (FormLayer)System.Enum.Parse(typeof(FormLayer), formName);
        }
        catch (Exception)
        {
            Debug.Log("form not exist:" + formName);
            return FormLayer.InvalidForm;
        }
        return layer;
    }

    private void LoadPrefab(string formName)
    {
        if (GameObject.Find(formName) != null)
        {
            return;
        }
        FormLayer layer = StringToFormLayer(formName);
        if (layer < 0)
        {
            return;
        }
        GameObject obj = Resources.Load<GameObject>(formPath + formName);
        GameObject form = Instantiate(obj);
        if (form != null)
        {
            form.name = formName;
            form.transform.SetParent(gameObject.transform, true);
            form.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            form.transform.localScale = Vector3.one;
            var canvas = form.GetComponent<Canvas>();
            canvas.worldCamera = GameFramework.Instance.MainCamera;
            canvas.planeDistance = (float)layer + 1.0f;
            canvas.sortingLayerID = formUILayerID;

            openingForms.Add(layer, openingForms.Count);
            Debug.Log("Load Prefab" + formName);
        }
        else
        {
            Debug.Log("form prefab " + formName + " null");
        }
    }

    private void DestroyPrefab(string formName)
    {
        FormLayer layer = StringToFormLayer(formName);
        if ((float)layer < 0)
        {
            Debug.Log("form invalid");
            return;
        }
        GameObject form = GameObject.Find(formName);
        Destroy(form);
        openingForms.Remove(layer);
    }
}
