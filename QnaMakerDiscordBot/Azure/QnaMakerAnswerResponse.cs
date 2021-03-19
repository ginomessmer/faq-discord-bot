public class QnaMakerAnswerResponse
{
    public object[] Questions { get; set; }
    public string Answer { get; set; }
    public float Score { get; set; }
    public int Id { get; set; }
    public bool IsDocumentText { get; set; }
    public object[] Metadata { get; set; }
    
    public override string ToString() => Answer;
}