using System.Diagnostics;

namespace WebexMessageExtractor {
    public static class LoggingHelper {

        public static bool InitializeLogFile() {
            bool rvalue = false;
            TextWriter logFile = null;
            try {
                logFile = new StreamWriter(string.Format("log-file-{0}.txt", DateTime.Now.ToString("yyyy-MMM-dd")), true);
                Console.SetOut(logFile);
                Console.Out.NewLine = Environment.NewLine;
                rvalue = true;
            }
            catch(Exception ex) {
                if(logFile != null) {
                    logFile.Flush();
                    logFile.Dispose();
                }
                Console.Error.WriteLine(ex.Message);
            }
            return rvalue;
        }

        public static bool InitializeErrorLogFile() {
            bool rvalue = false;
            TextWriter errorFile = null;
            try {
                errorFile = new StreamWriter(string.Format("error-file-{0}.txt", DateTime.Now.ToString("yyyy-MMM-dd")), true);
                Console.SetError(errorFile);
                Console.Error.NewLine = Environment.NewLine;
                rvalue = true;
            }
            catch(Exception ex) {
                if(errorFile != null) {
                    errorFile.Flush();
                    errorFile.Dispose();
                }
                Console.Error.WriteLine(ex.Message);
            }
            return rvalue;
        }

        public static void Log(string text) {
            Console.WriteLine($"time: {DateTime.Now}, text: {text}");
            Console.Out.Flush();
        }

        public static void Error(string text) {
            Error(text, stackFrame: 2);
        }

        public static void Error(Exception ex) {
            Error(ex: ex, stackFrame: 2);
        }

        public static void Error(string text = null, Exception ex = null, int stackFrame = 1) {
            string fileName = string.Empty;
            int lineNumber = -1;
            string methodName = string.Empty;
            string className = string.Empty;
            try {
                StackFrame frame = new StackFrame(stackFrame + 1, true);
                lineNumber = frame?.GetFileLineNumber() ?? -1;
                methodName = frame?.GetMethod()?.Name ?? string.Empty;
                className = frame?.GetMethod()?.DeclaringType?.Name ?? string.Empty;
                fileName = frame?.GetFileName() ?? string.Empty;
            }
            catch(Exception exception) {
                Console.Error.WriteLine(exception.Message);
            }

            //if someone directly runs the exe then fileName will have full path hence not printing it
            Console.Error.WriteLine($"time: {DateTime.Now}, lineNumber: {lineNumber}, methodName: {methodName}, className: {className}, text: {text}, ex: {ex?.Message}");
            Console.Error.Flush();
        }
    }
}
