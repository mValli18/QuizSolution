using QuizApp.Models.DTOs;

public class QuizResultsWithTotalScoreDTO
{
    public IList<QuizResultDTO> QuizResults { get; set; }
    public int TotalScore { get; set; }
}
