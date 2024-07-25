using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputReader : InputLayout.IPlayerActions
{
    private readonly IMessagePublisher _publisher;
    private readonly CompositeDisposable _disposable;

    private InputLayout _input;
    
    [Inject]
    public InputReader(IMessagePublisher publisher, InputLayout layout)
    {
        
        _input = layout;
        _input.Enable();
        _input.Player.Enable();
        _input.Player.SetCallbacks(this);
        
        _publisher = publisher;
        _disposable = new();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            var inputVector = context.ReadValue<Vector2>();
            var delta = new Vector3(inputVector.x, 0, inputVector.y);
            _publisher.Publish(new MoveMessage(delta));
            
            //Debug.Log($"Moving: {inputVector}");
        }
        

        if (context.phase == InputActionPhase.Canceled)
        {
            _publisher.Publish(new StopMessage());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _publisher.Publish(new JumpMessage());
        }
    }
    
}