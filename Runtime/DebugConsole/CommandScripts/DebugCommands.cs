//Created by: Ryan King
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// All methods in this class will be accessible as commands. They should return a string value to be output to the console.
/// The parameters will be received from the what is typed into the input field. They are converted in the DebugConsole class but not 
/// sure if this works with non-primitive types.
/// General commands that can be used in any app can go here. Game specific commands should be in a separate partial class.
/// </summary>
namespace HammerElf.Tools.Utilities.Commands
{
    public static partial class DebugCommands
    {
        private static bool logToggle;
        private static bool timeStopped;
        private static bool fpsVisible;

        #region Commands

        public static string FPS()
        {
            if (fpsVisible)
            {
                DebugConsole.Instance.fpsCounter.gameObject.SetActive(false);
            }
            else
            {
                DebugConsole.Instance.fpsCounter.gameObject.SetActive(true);
            }
            fpsVisible = !fpsVisible;

            return "FPS counter is " + (fpsVisible ? "visible." : "hidden.");
        }

        /// <summary>
        /// Example: Tests printing to debug console.
        /// </summary>
        /// <returns>Response to hello world</returns>
        public static string HelloWorld()
        {
            ConsoleLog.Log("Hello World!!!", true);
            return ".... Why hello there!";
        }

        public static string Help()
        {
            string messageBuilder = "Current commands available: ";
            MethodInfo[] methodInfo = typeof(DebugCommands).GetMethods(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);

            foreach (MethodInfo method in methodInfo)
            {
                messageBuilder += method.Name + ", ";
            }
            messageBuilder = messageBuilder.Substring(0, messageBuilder.Length - 2);
            messageBuilder += "\nFor more information type \"HelpDoc\".";

            return messageBuilder;
        }

        public static string HelpDoc()
        {
            Application.OpenURL("https://docs.google.com/document/d/1C4Hlpcg7jJezwYmXpgF19enlxtItT8G48EKQW1ozJDc/edit?usp=sharing");
            return "Documentation opened!";
        }

        public static string LoadScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);

            return "Loading scene " + sceneId;
        }

        public static string SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, Screen.fullScreenMode);
            return "Resolution set to " + width + " x " + height;
        }

        public static string StopTime()
        {
            if (timeStopped)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
            timeStopped = !timeStopped;

            return "Time is " + (timeStopped ? "stopped." : "resumed.");
        }

        /// <summary>
        /// Will toggle whether logs from ConsoleLog will also be displayed in the debug console.
        /// </summary>
        /// <param name="overrideConsole">Whether to override toConsole parameter option in ConsoleLog.</param>
        /// <returns></returns>
        public static string ToggleLog(bool overrideConsole = false)
        {
            logToggle = !logToggle;
            DebugConsole.Instance.consoleLogOutputting = logToggle && overrideConsole;

            return (logToggle ? "Logging in console and " : "Not logging in console and ") + 
                   (overrideConsole ? "overriding." : "not overriding.");
        }

        //Enables directly showing the unity log in the console. Great for standalone build debugging.
        //TODO: add flags for what kind of logs to enable (errors, warnings, logs, etc)
        public static string UnityLog() 
        {
            bool shouldEnable = !DebugConsole.Instance.unityLogOutputting;

            if(shouldEnable)
                UnityEngine.Application.logMessageReceived += PushToConsole;
            else
                UnityEngine.Application.logMessageReceived -= PushToConsole;

            DebugConsole.Instance.unityLogOutputting = shouldEnable;

            return $"Now {(shouldEnable ? "" : "no longer ")}showing Unity log output in DebugConsole.";
        }

        #endregion Commands

        #region Non-Commands

        //Callback that pushes the log to the console
        private static void PushToConsole(string logString, string stackTrace, LogType type)
        {
            string output = $"[{type}]| " + logString;
            if(type == LogType.Exception)
            {
                output += "\n" + stackTrace.Replace("\n", "\t\n"); //indent the stack trace
            }

            DebugConsole.Instance.AddOutputEntry(output);
        }
        #endregion Non-Commands
    }
}
