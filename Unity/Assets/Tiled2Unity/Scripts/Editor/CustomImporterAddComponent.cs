using UnityEngine;
using System.Collections.Generic;
using System;

[Tiled2Unity.CustomTiledImporter]
class CustomImporterAddComponent : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        // Simply add a component to our GameObject
        if (props.ContainsKey("AddComp"))
        {
            try
            {
                string type = props["AddComp"];
                Type T = TypeMapping.GetType(type);
                Component comp = gameObject.AddComponent(T);
                ITiledComponent tiledComponent = comp as ITiledComponent;
                if (tiledComponent != null)
                {
                    tiledComponent.SetupFromProperties(props);
                }
            }
            catch( Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public void CustomizePrefab(GameObject prefab)
    {
        // Do nothing
    }
}