using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: <tab> to autocomplete the first part of ambiguities
//TODO: disambiguation
//TODO: keep a list of previous commands, manage it with up and down arrow

namespace Terminal
{
    public class TerminalController : MonoBehaviour
    {
        private bool _showConsole;
        private string _input;
        private bool _showHelp;
        private Vector2 _scroll;
    
        public List<object> commandList;
        public static TerminalCommand KILL_ALL;
        public static TerminalCommand<int> SET_GOLD;
        public static TerminalCommand HELP;
    
        [SerializeField] private Vector2Int _topLeftOffset = new Vector2Int(0,  0);
        // [SerializeField] private Vector2Int _bottomRightOffset = new Vector2Int(0,  0);
    
        [SerializeField] private InputAction toggleDebugAction;
        [SerializeField] private InputAction returnAction;
    
        [HideInInspector][SerializeField] private InputActionMap debugActionMap;
    
        private void Awake()
        {
            SetupInput();
        
            KILL_ALL = new TerminalCommand("kill", "Remove all heroes", "kill", () => Debug.Log("Kill Action"));
            SET_GOLD = new TerminalCommand<int>("set_gold", "set gold amount", "set_gold <gold_amount>", (x) => Debug.Log($"Set gold to {x}"));
            HELP = new TerminalCommand("help", "Show all possible commands", "help", () => _showHelp = true);
            commandList = new List<object>
            {
                KILL_ALL,
                SET_GOLD,
                HELP
            };
        }

        private void MockupListTerminalCommands()
        {
            
        }

        private void OnGUI()
        {
            if (!_showConsole) { return; }

            if (_showHelp)
            {
                GUI.Box(new Rect(_topLeftOffset.x, _topLeftOffset.y, Screen.width, 100), string.Empty);
                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
                _scroll = GUI.BeginScrollView(new Rect(_topLeftOffset.x, _topLeftOffset.y + 5f, Screen.width, 90), _scroll,
                    viewport);
            
                for (int i = 0; i < commandList.Count; i++)
                {
                    TerminalCommandBase command = commandList[i] as TerminalCommandBase;
                    string label = $"{command.Command.Format} - {command.Command.Description}";
                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                    GUI.Label(labelRect, label);
                }

                GUI.EndScrollView();
            }
        
            GUI.Box(new Rect(_topLeftOffset.x, _topLeftOffset.y + 100f, Screen.width, 30), string.Empty);
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            
            // Set the internal name of the textfield to get focus
            GUI.SetNextControlName("InputText");
            _input = GUI.TextField(new Rect(_topLeftOffset.x + 10f, _topLeftOffset.y + 100f + 5f, Screen.width - 20f, 20f),
                _input);
            GUI.FocusControl("InputText");
        }

        private void HandleInput()
        {
            if (string.IsNullOrEmpty(_input))
            {
                Debug.Log("No input to process");
                return;
            }
        
            string[] properties = _input.Split(' ');
        
            // maybe use a dict?
            for (int i = 0; i < commandList.Count; i++)
            {
                var command = commandList[i];
            
                TerminalCommandBase commandBase = command as TerminalCommandBase;

                if (_input.Contains(commandBase.Command.Id))
                {
                    switch (command)
                    {
                        case TerminalCommand terminalCommand:
                            terminalCommand?.Invoke();
                            break;
                        case TerminalCommand<int> terminalCommand:
                            if (int.TryParse(properties[i], out var intParsed))
                            {
                                terminalCommand?.Invoke(intParsed);
                            }
                            else
                            {
                                Debug.Log("[DebugController] Syntax error! Expected integer value");
                            }
                            break;
                    }
                }
            }
        
        }

        private void SetupInput()
        {
            debugActionMap.AddAction(toggleDebugAction.name,
                toggleDebugAction.type,
                toggleDebugAction.bindings.ToString());
        
            debugActionMap.AddAction(returnAction.name,
                returnAction.type,
                returnAction.bindings.ToString());
        
            toggleDebugAction.performed += ToggleDebugAction;
            returnAction.performed += OnReturnAction;
        }

        private void ToggleDebugAction(InputAction.CallbackContext context)
        {
            _showConsole = !_showConsole;
        }

        private void OnReturnAction(InputAction.CallbackContext context)
        {
            if (!_showConsole) return;
            HandleInput();
            _input = string.Empty;
        }

        private void OnEnable()
        {
            toggleDebugAction.Enable();
            returnAction.Enable();
            debugActionMap.Enable();
        }

        private void OnDisable()
        {
            _showConsole = false;
            toggleDebugAction.Disable();
            returnAction.Disable();
            debugActionMap.Disable();
        }
    }
}
