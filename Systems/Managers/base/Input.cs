using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;


[AddComponentMenu("ComponentsMythic/Managers/Input")]
public class InputManager : Manager
{
    [Header("Keyboard Bindings")]
    public Key okKey = Key.Enter;
    public Key cancelKey = Key.Escape;
    public Key leftKey = Key.A;
    public Key rightKey = Key.D;
    public Key upKey = Key.W;
    public Key downKey = Key.S;

    [Header("Gamepad Defaults (non-Inspector enums, version-safe)")]
    [Tooltip("OK/Confirm on gamepad (usually A / Cross)")]
    [SerializeField] private bool gamepadOkUsesSouth = true;

    [Tooltip("Cancel/Back on gamepad (usually B / Circle)")]
    [SerializeField] private bool gamepadCancelUsesEast = true;

    [Header("Shoulders / Start / Select")]
    [Tooltip("If true, gamepad Start/Select/Shoulders/Triggers are enabled.")]
    [SerializeField] private bool useGamepadButtons = true;

    [Header("Triggers")]
    [Tooltip("How far a trigger must be pressed to count as 'Held' (0..1).")]
    [Range(0f, 1f)]
    [SerializeField] private float triggerHeldThreshold = 0.5f;

    [Tooltip("How far a trigger must be pressed to count as 'Pressed This Frame' (0..1).")]
    [Range(0f, 1f)]
    [SerializeField] private float triggerPressedThreshold = 0.5f;

    [Header("Joystick")]
    public bool includeDpadInJoystick = true;
    public float deadzone = 0.2f;

    // Internal trigger edge detection
    private bool _ltWasHeld;
    private bool _rtWasHeld;

    // --- Keyboard helpers ---
    private static bool KeyDown(Key k)
    {
        var kb = Keyboard.current;
        return kb != null && kb[k].wasPressedThisFrame;
    }

    private static bool KeyHeld(Key k)
    {
        var kb = Keyboard.current;
        return kb != null && kb[k].isPressed;
    }

    // --- Gamepad helpers (no GamepadButton enum needed) ---
    private static Gamepad GP => Gamepad.current;

    private bool SouthDown() => GP != null && GP.buttonSouth.wasPressedThisFrame;
    private bool SouthHeld() => GP != null && GP.buttonSouth.isPressed;

    private bool EastDown() => GP != null && GP.buttonEast.wasPressedThisFrame;
    private bool EastHeld() => GP != null && GP.buttonEast.isPressed;

    private bool WestDown() => GP != null && GP.buttonWest.wasPressedThisFrame;
    private bool WestHeld() => GP != null && GP.buttonWest.isPressed;

    private bool NorthDown() => GP != null && GP.buttonNorth.wasPressedThisFrame;
    private bool NorthHeld() => GP != null && GP.buttonNorth.isPressed;

    private bool StartDown() => GP != null && GP.startButton.wasPressedThisFrame;
    private bool StartHeld() => GP != null && GP.startButton.isPressed;

    private bool SelectDown() => GP != null && GP.selectButton.wasPressedThisFrame;
    private bool SelectHeld() => GP != null && GP.selectButton.isPressed;

    private bool LShoulderDown() => GP != null && GP.leftShoulder.wasPressedThisFrame;
    private bool LShoulderHeld() => GP != null && GP.leftShoulder.isPressed;

    private bool RShoulderDown() => GP != null && GP.rightShoulder.wasPressedThisFrame;
    private bool RShoulderHeld() => GP != null && GP.rightShoulder.isPressed;

    // Triggers are analog in the new system; we implement Pressed/Held semantics ourselves.
    private float LTValue() => GP != null ? GP.leftTrigger.ReadValue() : 0f;
    private float RTValue() => GP != null ? GP.rightTrigger.ReadValue() : 0f;

    private bool LTriggerHeld() => LTValue() >= triggerHeldThreshold;
    private bool RTriggerHeld() => RTValue() >= triggerHeldThreshold;

    private bool LTriggerPressedThisFrame()
    {
        bool held = LTValue() >= triggerPressedThreshold;
        bool pressed = held && !_ltWasHeld;
        _ltWasHeld = held;
        return pressed;
    }

    private bool RTriggerPressedThisFrame()
    {
        bool held = RTValue() >= triggerPressedThreshold;
        bool pressed = held && !_rtWasHeld;
        _rtWasHeld = held;
        return pressed;
    }

    private Vector2 ReadMove()
    {
        Vector2 v = Vector2.zero;

        // Keyboard digital movement
        if (KeyHeld(leftKey))  v.x -= 1f;
        if (KeyHeld(rightKey)) v.x += 1f;
        if (KeyHeld(downKey))  v.y -= 1f;
        if (KeyHeld(upKey))    v.y += 1f;

        // Gamepad analog movement
        var gp = GP;
        if (gp != null)
        {
            v += gp.leftStick.ReadValue();

            if (includeDpadInJoystick)
                v += gp.dpad.ReadValue();
        }

        // Deadzone + clamp
        if (v.magnitude < deadzone) v = Vector2.zero;
        return Vector2.ClampMagnitude(v, 1f);
    }

    // --- Your existing API (kept) ---
    public bool okPressed()
    {
        bool keyboard = KeyDown(okKey);
        bool gamepad = useGamepadButtons && gamepadOkUsesSouth && SouthDown();
        return keyboard || gamepad;
    }

    public bool okHeld()
    {
        bool keyboard = KeyHeld(okKey);
        bool gamepad = useGamepadButtons && gamepadOkUsesSouth && SouthHeld();
        return keyboard || gamepad;
    }

    public bool cancelPressed()
    {
        bool keyboard = KeyDown(cancelKey);
        bool gamepad = useGamepadButtons && gamepadCancelUsesEast && EastDown();
        return keyboard || gamepad;
    }

    public bool cancelHeld()
    {
        bool keyboard = KeyHeld(cancelKey);
        bool gamepad = useGamepadButtons && gamepadCancelUsesEast && EastHeld();
        return keyboard || gamepad;
    }

    public bool leftPressed()
    {
        bool keyboard = KeyDown(leftKey);
        bool gamepad = useGamepadButtons && GP != null && GP.dpad.left.wasPressedThisFrame;
        return keyboard || gamepad;
    }

    public bool leftHeld()
    {
        bool keyboard = KeyHeld(leftKey);
        bool gamepad = useGamepadButtons && GP != null && GP.dpad.left.isPressed;
        return keyboard || gamepad;
    }

    public bool rightPressed()
    {
        bool keyboard = KeyDown(rightKey);
        bool gamepad = useGamepadButtons && GP != null && GP.dpad.right.wasPressedThisFrame;
        return keyboard || gamepad;
    }

    public bool rightHeld()
    {
        bool keyboard = KeyHeld(rightKey);
        bool gamepad = useGamepadButtons && GP != null && GP.dpad.right.isPressed;
        return keyboard || gamepad;
    }

    public bool upPressed()
    {
        bool keyboard = KeyDown(upKey);
        bool gamepad = useGamepadButtons && GP != null && GP.dpad.up.wasPressedThisFrame;
        return keyboard || gamepad;
    }

    public bool upHeld()
    {
        bool keyboard = KeyHeld(upKey);
        bool gamepad = useGamepadButtons && GP != null && GP.dpad.up.isPressed;
        return keyboard || gamepad;
    }

    public bool downPressed()
    {
        bool keyboard = KeyDown(downKey);
        bool gamepad = useGamepadButtons && GP != null && GP.dpad.down.wasPressedThisFrame;
        return keyboard || gamepad;
    }

    public bool downHeld()
    {
        bool keyboard = KeyHeld(downKey);
        bool gamepad = useGamepadButtons && GP != null && GP.dpad.down.isPressed;
        return keyboard || gamepad;
    }

    public bool startPressed()
    {
        return useGamepadButtons && StartDown();
    }

    public bool startHeld()
    {
        return useGamepadButtons && StartHeld();
    }

    public bool selectPressed()
    {
        return useGamepadButtons && SelectDown();
    }

    public bool selectHeld()
    {
        return useGamepadButtons && SelectHeld();
    }

    public bool lt1Pressed()
    {
        return useGamepadButtons && LShoulderDown();
    }

    public bool lt1Held()
    {
        return useGamepadButtons && LShoulderHeld();
    }

    public bool rt1Pressed()
    {
        return useGamepadButtons && RShoulderDown();
    }

    public bool rt1Held()
    {
        return useGamepadButtons && RShoulderHeld();
    }

    public bool lt2Pressed()
    {
        return useGamepadButtons && LTriggerPressedThisFrame();
    }

    public bool lt2Held()
    {
        return useGamepadButtons && LTriggerHeld();
    }

    public bool rt2Pressed()
    {
        return useGamepadButtons && RTriggerPressedThisFrame();
    }

    public bool rt2Held()
    {
        return useGamepadButtons && RTriggerHeld();
    }

    public bool anyKeyPressed()
    {
        var kb = Keyboard.current;
        if (kb != null && kb.anyKey.wasPressedThisFrame) return true;

        var gp = GP;
        if (useGamepadButtons && gp != null)
        {
            // Approximation: common face buttons + start/select + shoulders + dpad
            if (gp.buttonSouth.wasPressedThisFrame || gp.buttonEast.wasPressedThisFrame ||
                gp.buttonWest.wasPressedThisFrame || gp.buttonNorth.wasPressedThisFrame ||
                gp.startButton.wasPressedThisFrame || gp.selectButton.wasPressedThisFrame ||
                gp.leftShoulder.wasPressedThisFrame || gp.rightShoulder.wasPressedThisFrame ||
                gp.dpad.up.wasPressedThisFrame || gp.dpad.down.wasPressedThisFrame ||
                gp.dpad.left.wasPressedThisFrame || gp.dpad.right.wasPressedThisFrame)
                return true;

            // Trigger edge as "any"
            if (LTriggerPressedThisFrame() || RTriggerPressedThisFrame())
                return true;
        }

        return false;
    }

    public Vector3 Joystick3D()
    {
        Vector2 v = ReadMove();
        return new Vector3(v.x, 0f, v.y);
    }

    public Vector2 Joystick2D()
    {
        return ReadMove();
    }

    public bool JoystickMoved()
    {
        return ReadMove() != Vector2.zero;
    }

    public float vertical() => ReadMove().y;
    public float horizontal() => ReadMove().x;

    public override void start() { }

    public override void Update()
    {
        // Keep trigger edge detection stable even if you never call lt2/rt2 this frame.
        // (If you *only* call lt2Pressed/rt2Pressed when checking triggers, you can remove this.)
        _ = LTriggerPressedThisFrame();
        _ = RTriggerPressedThisFrame();
    }
}
