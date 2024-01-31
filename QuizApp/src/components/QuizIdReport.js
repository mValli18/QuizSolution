import React, { useState, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";

function QuizIdReport() {
  const location = useLocation();
  const [quizResults, setQuizResults] = useState(null);
  const navigate = useNavigate();
  const token=localStorage.getItem("token")
  useEffect(() => {
    // Use the username and quizId from the location state
    const { quizId } = location.state;

    // Fetch quiz results based on username and quizId
    fetch(`http://localhost:5252/api/QuizResult/byQuiz/${quizId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
      }) .then(async (response) => {
        const data = await response.json();
        setQuizResults(data);
        console.log("quizId:",quizId);
      })
      .catch((error) => console.error("Error fetching quiz results:", error));
  }, []); // Empty dependency array ensures that this effect runs only once when the component mounts
  const GoToQuizs=()=>{
    navigate("/quizList");
  }
  
  return (
    <div className="inputcontainer">
      {console.log("quizResults:",quizResults)}
        <div>
          <h2>Quiz Results</h2>
          <button className="btn btn-success" onClick={GoToQuizs}>GoBack</button>

          {quizResults && Array.isArray(quizResults) && quizResults.length > 0 ? (
          <table className="table table-bordered border-primary">
                    <thead className="thead-dark">
                      <tr>
                        <th scope="col">QuizId</th>
                        <th scope="col">Username</th>
                        <th scope="col">QuestionId</th>
                        <th scope="col">User Answer</th>
                        <th scope="col">Result</th>
                        <th scope="col">Score</th>
                      </tr>
                    </thead>
                    <tbody>
                      {quizResults.map((result, index) => (
                        <tr key={index}>
                          <td>{result.quizId}</td>
                          <td>{result.username}</td>
                          <td>{result.questionId}</td>
                          <td>{result.userAnswer}</td>
                          <td>{result.isCorrect ? "Correct" : "Incorrect"}</td>
                          <td>{result.score}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
          ):(
            <p>No quiz results available.</p>
            )}
        </div>
    </div>
  );
}

export default QuizIdReport;