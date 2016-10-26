using UnityEngine;
using System.Collections.Generic;
using System;

[Tiled2Unity.CustomTiledImporter]
class CustomImporterNavigation : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        
    }

    public void CustomizePrefab(GameObject prefab)
    {
        TrackNavigation[] navList = prefab.GetComponentsInChildren<TrackNavigation>();
        int i = 0;
        foreach (TrackNavigation nav in navList)
        {
            Debug.Log("Navigation: " + nav.ToString());
            nav.gameObject.name = "Navigation_" + i++;
            BoxCollider2D collider = nav.gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.5f, 0.5f);
            collider.offset = new Vector2(0.25f, 0.25f);
            nav.centerOffset = collider.offset;
        }

        foreach (TrackNavigation nav in navList)
        {
            Vector3 position = nav.gameObject.transform.TransformPoint(new Vector3(0.25f, 0.25f));
            Vector3 dir = nav.gameObject.transform.right;

            bool origFlag = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = false;
            RaycastHit2D hit = Physics2D.Raycast(position, dir, TrackNavigation.MAX_RAY_LENGTH, 1 << nav.gameObject.layer);
            if (hit.collider != null)
            {
                TrackNavigation other = hit.collider.gameObject.GetComponent<TrackNavigation>();
                if (other != null)
                {
                    nav.nextNavigationLink = other;
                }
            }
            Physics2D.queriesStartInColliders = origFlag;
        }
    }
}