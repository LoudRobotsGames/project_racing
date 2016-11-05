using UnityEngine;
using System.Collections.Generic;
using System;

[Tiled2Unity.CustomTiledImporter]
class CustomImporterNavigation : Tiled2Unity.ICustomTiledImporter
{
    private const float TRIGGER_SIZE = 2f;
    private const float TRIGGER_OFFSET = 0.25f;

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
            collider.isTrigger = true;
            collider.size = new Vector2(TRIGGER_SIZE, TRIGGER_SIZE);
            collider.offset = new Vector2(TRIGGER_OFFSET, TRIGGER_OFFSET);
            nav.centerOffset = collider.offset;
        }

        foreach (TrackNavigation nav in navList)
        {
            Vector3 position = nav.position;
            Vector3 dir = nav.gameObject.transform.right;

            bool origFlag = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = false;
            RaycastHit2D hit = Physics2D.Raycast(position, dir, TrackNavigation.MAX_RAY_LENGTH, 1 << nav.gameObject.layer);
            if (hit.collider != null)
            {
                TrackNavigation other = hit.collider.gameObject.GetComponent<TrackNavigation>();
                if (other != null)
                {
                    float distance = Vector3.Distance(position, other.position);
                    hit = Physics2D.Raycast(position, dir, distance, 1 << LayerMask.NameToLayer("FinishLine"));
                    if (hit.collider != null)
                    {

                        GameObject go = new GameObject("FinishLine");
                        go.transform.parent = nav.transform.parent;
                        go.transform.position = hit.point;
                        TrackNavigation finish = go.AddComponent<TrackNavigation>();
                        finish.nextNavigationLink = other;
                        finish.previousNavigationLink = nav;
                        nav.nextNavigationLink = finish;
                        other.previousNavigationLink = finish;
                        finish.crossesFinishLine = true;
                    }
                    else
                    {
                        nav.nextNavigationLink = other;
                        other.previousNavigationLink = nav;
                    }
                }
            }
            Physics2D.queriesStartInColliders = origFlag;
        }
    }
}