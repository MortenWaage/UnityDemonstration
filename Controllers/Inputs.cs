using System.Collections.Generic;
using UnityEngine;

/*
 *  THIS CLASS WILL TAKE INPUT FROM THE KEYBOARD AND/OR MOUSE
 *  AND CALL INTERFACE METHODS ON THE GAME OBJECTS AND SCRIPTS
 *  WE WANT TO RECEIVE THIS INPUT.
 */

public class Inputs : MonoBehaviour
{
    public List<IInputs> ControllableObjects; //-- List of GameObjects that should be controlled by Player Input.

    void Awake()
    {
        ControllableObjects = new List<IInputs>();
    }

    void Update()
    {
        MovementInput();

        AttackInput();

        AdditionalInput();
    }

    private void MovementInput()
    {
        var direction = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
            direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            direction += Vector3.right;

        if (Input.GetKey(KeyCode.W))
            direction += Vector3.up;

        if (Input.GetKey(KeyCode.S))
            direction += Vector3.down;

        Move(direction);

        void Move(Vector3 direction)
        {
            foreach (var controllableObject in ControllableObjects)
                controllableObject.Move(direction);
        }
    }

    private void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShootProjectile();

        void ShootProjectile()
        {
            foreach (var controllableObject in ControllableObjects)
                controllableObject.ShootProjectile();
        }
    }

    private void AdditionalInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameController.Instance.RestartGame();
        }
    }



}
