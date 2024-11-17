namespace _MAIN.Scripts.Core.Feature_Panels.ChoicePanel
{
    public class ChoicePanelDecision
    {
        public string Question;
        public int AnswerIndex;
        public string[] Choices;

        public ChoicePanelDecision(string question, string[] choices)
        {
            Question = question;
            Choices = choices;
            AnswerIndex = -1;
        }
    }
}
