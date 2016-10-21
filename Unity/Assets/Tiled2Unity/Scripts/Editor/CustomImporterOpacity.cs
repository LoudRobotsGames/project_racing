using UnityEngine;
using System.Collections.Generic;
using System;

[Tiled2Unity.CustomTiledImporter]
class CustomImporterOpacity : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        // Simply add a component to our GameObject
        if (props.ContainsKey("opacity"))
        {
            try
            {
                string prop = props["opacity"];
                float opacity = float.Parse(prop);
                Tiled2Unity.TiledInitialShaderProperties properties = gameObject.GetComponentInChildren<Tiled2Unity.TiledInitialShaderProperties>();
                properties.InitialOpacity = opacity;
            }
            catch (Exception e)
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