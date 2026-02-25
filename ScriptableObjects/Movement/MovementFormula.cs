using UnityEngine;

[CreateAssetMenu(fileName = "NewMovement", menuName = "ComponentsMythic/MovementFormula")]
public class MovementFormula : AbstractScriptable
{
    public float moveSpeed;
    public float xMultiplier;
    public float yMultiplier;
    public float zMultiplier;
    public float drag;
    public float gravity;

}