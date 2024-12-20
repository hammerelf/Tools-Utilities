//Created by: Ryan King

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HammerElf.Tools.Utilities
{
    //Requires an Event system in scene to be able to type in input field.
    public class DebugConsole : Singleton<DebugConsole>
    {
        [SerializeField]
        private GameObject consoleEntryPanel, consoleOutputPanel, t, outputTextObject;
        [SerializeField]
        private TMP_InputField entryInputField;
        [SerializeField]
        private Transform outputContent;
        [LabelText("FPSCounter")]
        public TextMeshProUGUI fpsCounter;

        private int windowState;
        private string previousEntry;
        [HideInInspector]
        public bool consoleLogOutputting;
        [HideInInspector]
        public bool unityLogOutputting = false;
        private bool initialMethodCaching = false;

        private void Start()
        { 
            //Keep FPSCounter updated
            StartCoroutine(UpdateFPSCounter());
        }

        private void Update()
        {
            //If BackQuote/Tilde is pressed it cycles through the window states.
            if(Input.GetKeyDown(KeyCode.BackQuote))
            {
                if(!initialMethodCaching)
                {
                    // Cache all methods with the custom attribute once
                    CacheDebugAttributeMethods();
                    initialMethodCaching = true;
                }

                WindowStateToggle(windowState++);

                entryInputField.text = entryInputField.text.Replace("`", "");
            }
            
            //Pressing enter or clicking the submit button submits the command entered and tries to run it.
            if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && EventSystem.current.currentSelectedGameObject
                && EventSystem.current.currentSelectedGameObject.name == entryInputField.name)
            {
                //SubmitEntryInput();
                SubmitAttributeEntryInput();
            }

            //Pressing up will set the entry field to the previously entered command.
            if(Input.GetKeyDown(KeyCode.UpArrow) && EventSystem.current.currentSelectedGameObject
                && EventSystem.current.currentSelectedGameObject.name == entryInputField.name)
            {
                entryInputField.text = previousEntry;
                FocusInputField();
            }

            //Pressing down will clear the entry field.
            if(Input.GetKeyDown(KeyCode.DownArrow) && EventSystem.current.currentSelectedGameObject
                && EventSystem.current.currentSelectedGameObject.name == entryInputField.name)
            {
                entryInputField.text = "";
            }
        }

        private IEnumerator UpdateFPSCounter()
        {
            //reusing variable to keep the counter as lightweight as reasonably possible
            int startingFrameCount;

            while (Application.isPlaying)
            {
                startingFrameCount = Time.frameCount;

                yield return new WaitForSeconds(1f);

                //how many frames happened in the past second
                if(fpsCounter) fpsCounter.text = (Time.frameCount - startingFrameCount).ToString();
            }
        }

        //Needed for button functionality.
        public void ToggleOutputWindow()
        {
            consoleOutputPanel.SetActive(!consoleOutputPanel.activeSelf);
        }

        public void WindowStateToggle(int state)
        {
            switch(state)
            {
                case 0:
                {
                    consoleEntryPanel.SetActive(true);
                    consoleOutputPanel.SetActive(false);
                    FocusInputField();
                    break;
                }
                case 1:
                {
                    consoleEntryPanel.SetActive(true);
                    consoleOutputPanel.SetActive(true);
                    FocusInputField();
                    break;
                }
                default:
                {
                    consoleEntryPanel.SetActive(false);
                    consoleOutputPanel.SetActive(false);
                    windowState = 0;
                    break;
                }
            }
        }

        IEnumerator GoT()
        {
            t.SetActive(true);
            yield return new WaitForSeconds(5);
            t.SetActive(false);
        }

        /// <summary>
        /// Tries to run entry as a command. Must be entered exactly correct. Parameters can be included and separated with spaces.
        /// Use quotes to allow for spaces in parameters. Will add a returned string value from the command as well as the command 
        /// to the output window.
        /// </summary>
        public void SubmitEntryInput()
        {
            previousEntry = entryInputField.text;
            string outputBuilder = entryInputField.text;

            string[] splitCommandText = QuotesAllowSpaces();
            entryInputField.text = "";

            MethodInfo method = typeof(Commands.DebugCommands).GetMethod(splitCommandText[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);

            if(method == null)
            {
                ConsoleLog.Log("Method is null. splitCommandText[0]: " + splitCommandText[0]);
            }

            // Create an array to hold the parsed arguments
            object[] parameterArray = new object[method.GetParameters().Length];

            for (int i = 0; i < method.GetParameters().Length; i++)
            {
                Type parameterType = method.GetParameters()[i].ParameterType;

                try
                {
                    object value = Convert.ChangeType(splitCommandText[i + 1], parameterType);
                    parameterArray[i] = value;
                }
                catch (TargetParameterCountException ex)
                {
                    Console.WriteLine($"Error parsing parameter {i}: {ex.Message}");
                    parameterArray[i] = System.Type.Missing;
                }
            }

            try
            {
                if(typeof(Commands.DebugCommands).GetMethod(splitCommandText[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static) != null)
                {
                    StartCoroutine(GoT());
                    outputBuilder += " | " + typeof(Commands.DebugCommands).GetMethod(splitCommandText[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static)?.Invoke(null, parameterArray);
                }
            }
            catch (TargetParameterCountException)
            {
                ConsoleLog.LogWarning("Wrong number of parameters for method. Amount: " + parameterArray.Length, true);
            }

            AddOutputEntry(outputBuilder);

            FocusInputField();
        }

        //Adds a new entry to the debug console output window.
        public void AddOutputEntry(string entryValue)
        {
            GameObject outputEntry = Instantiate(outputTextObject, outputContent);
            outputEntry.GetComponent<TextMeshProUGUI>().text = entryValue;
        }

        //Sets focus for EventSystem to entryInputField and moves text cursor to end of line.
        private void FocusInputField()
        {
            entryInputField.Select();
            entryInputField.ActivateInputField();
            entryInputField.caretPosition = entryInputField.text.Length;
        }

        //Allows quotes to use spaces.
        private string[] QuotesAllowSpaces()
        {
            List<string> spaceEscape = new List<string>();
            string currentWord = "";
            bool inQuotes = false;

            foreach (char c in entryInputField.text)
            {
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ' ' && !inQuotes)
                {
                    spaceEscape.Add(currentWord);
                    currentWord = "";
                }
                else
                {
                    currentWord += c;
                }
            }

            spaceEscape.Add(currentWord);

            List<string> combinedList = new List<string>();

            for (int i = 0; i < spaceEscape.Count; i++)
            {
                if (spaceEscape[i].EndsWith("\\"))
                {
                    string currentConsecutive = spaceEscape[i].TrimEnd('\\');

                    while (i + 1 < spaceEscape.Count && spaceEscape[i + 1].EndsWith("\\"))
                    {
                        i++;
                        currentConsecutive += " " + spaceEscape[i].TrimEnd('\\');
                    }

                    if (i + 1 < spaceEscape.Count)
                    {
                        currentConsecutive += " " + spaceEscape[i + 1];
                        i++;
                    }

                    combinedList.Add(currentConsecutive);
                }
                else
                {
                    combinedList.Add(spaceEscape[i]);
                }
            }

            return combinedList.ToArray();
        }

        private static Dictionary<string, MethodInfo> cachedMethods = new Dictionary<string, MethodInfo>();
        
        public static void CacheDebugAttributeMethods()
        {
            //Filter to only include assemblies from the project (e.g., Assembly-CSharp)
            IEnumerable<Assembly> projectAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
                assembly.GetName().Name.StartsWith("Assembly-CSharp"));

            foreach (Assembly assembly in projectAssemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (MethodInfo method in
                             type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                             BindingFlags.Static | BindingFlags.Instance))
                    {
                        if (method.GetCustomAttribute<DebugCommand>() != null)
                        {
                            // Add to cache with the method name as the key (you could also use a custom string)
                            cachedMethods[method.Name.ToLower()] = method;

                            //Console.WriteLine($"Calling method: {method.Name}");

                            //// Check if the method is static or instance
                            //if (method.IsStatic)
                            //{
                            //    // Call static method
                            //    method.Invoke(null, null);
                            //}
                            //else
                            //{
                            //    // Create an instance of the class and call the method
                            //    var instance = Activator.CreateInstance(method.DeclaringType);
                            //    method.Invoke(instance, null);
                            //}
                        }
                    }
                }
            }

            ConsoleLog.Log("Caching methods complete. Total cached is " + cachedMethods.Count);
        }

        public static MethodInfo GetCachedMethod(string methodName)
        {
            methodName = methodName.ToLower();
            if (cachedMethods.TryGetValue(methodName, out MethodInfo method))
            {
                return method;
            }
            return null;
        }

        public void SubmitAttributeEntryInput()
        {
            previousEntry = entryInputField.text;
            string outputBuilder = entryInputField.text;

            string[] splitCommandText = QuotesAllowSpaces();
            entryInputField.text = "";

            //MethodInfo method = typeof(Commands.DebugCommands).GetMethod(splitCommandText[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
            MethodInfo method = GetCachedMethod(splitCommandText[0]);

            if (method == null)
            {
                ConsoleLog.Log("Method is null. splitCommandText[0]: " + splitCommandText[0]);
                return;
            }

            // Create an array to hold the parsed arguments
            object[] parameterArray = new object[method.GetParameters().Length];

            for (int i = 0; i < method.GetParameters().Length; i++)
            {
                Type parameterType = method.GetParameters()[i].ParameterType;

                try
                {
                    object value = Convert.ChangeType(splitCommandText[i + 1], parameterType);
                    parameterArray[i] = value;
                }
                catch (TargetParameterCountException ex)
                {
                    Console.WriteLine($"Error parsing parameter {i}: {ex.Message}");
                    parameterArray[i] = System.Type.Missing;
                }
            }

            try
            {
                object targetInstance = null;

                if (method.IsStatic)
                {
                    // Static method, no instance needed
                    StartCoroutine(GoT());
                    outputBuilder += " | " + method.Invoke(null, parameterArray);
                }
                else
                {
                    // Non-static method
                    if (typeof(MonoBehaviour).IsAssignableFrom(method.DeclaringType))
                    {
                        // Find or create a MonoBehaviour instance
                        var existingInstance = GameObject.FindFirstObjectByType(method.DeclaringType);
                        if (existingInstance == null)
                        {
                            // Optionally create a new instance
                            GameObject newGameObject = new GameObject(method.DeclaringType.Name);
                            existingInstance = newGameObject.AddComponent(method.DeclaringType) as MonoBehaviour;
                        }
                        targetInstance = existingInstance;
                    }
                    else
                    {
                        // Create a new instance for regular classes
                        targetInstance = Activator.CreateInstance(method.DeclaringType);
                    }

                    StartCoroutine(GoT());
                    outputBuilder += " | " + method.Invoke(targetInstance, parameterArray);
                }
            }
            catch (Exception ex)
            {
                ConsoleLog.LogWarning($"Error invoking method: {ex.Message}", true);
            }

            AddOutputEntry(outputBuilder);

            FocusInputField();
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class DebugCommand : Attribute { }
}