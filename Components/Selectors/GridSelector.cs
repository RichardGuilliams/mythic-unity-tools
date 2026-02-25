using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[AddComponentMenu("ComponentsMythic/Selectors/Grid Selector")]
public class GridSelector : Selector
{
    public GameObject container;
    public SelectionShape shape;
    public override void showPanels()
    {
        shape.selector = this;
        shape.origin = this.assignedParent;
        shape.range = (int)GetComponentInParent<Unit>().stats.GetValue<int>("Range");
        GameObject obj = Instantiate(container, this.transform.parent.position, Quaternion.identity, this.transform);
        
        shape.showPanels(obj);
    }
}