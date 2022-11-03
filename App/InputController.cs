namespace Checkpoint2_Productlist.App
{
    public class InputController
    {
        private readonly Action<View> changeViewAction;

        public InputController(Action<View> changeViewAction) => this.changeViewAction = changeViewAction;

        public void ChangeView(View view) => changeViewAction?.Invoke(view);
        public static void AwaitTextCommand(ConsoleColor textColor, params (string text, string command, Action callback)[] commands)
        {
            string text = string.Empty;
            Array.ForEach(commands, x => {
                text += x.text + ";";
            });
            text = text[..^1];
            text = text.Replace(";", " | ");

            ConsoleEx.WriteColoredLine(text, textColor);

            string? input = Console.ReadLine();
            commands.FirstOrDefault((x) => x.command.ToLower() == input?.ToLower()).callback?.Invoke();
        }

        public static void AwaitTextInput(string message, Action<string?> Callback, ConsoleColor textColor = ConsoleColor.White)
        {
            ConsoleEx.WriteColored(message, textColor);
            Callback?.Invoke(Console.ReadLine());
        }
    }

}