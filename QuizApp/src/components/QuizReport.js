import React, { useState, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";

function QuizReport() {
  const location = useLocation();
  const [quizResults, setQuizResults] = useState(null);
  const navigate = useNavigate();
  const token=localStorage.getItem("token");
  const role=localStorage.getItem("role");
  useEffect(()=>{
  if (role !== "Creator") {
    alert("You don't have access to this page");
    setTimeout(() => {
      navigate("/quizs");
    }, 0);
    return ()=>{

    };
  }
},[role,navigate]);
  useEffect(() => {

    // Fetch quiz results based on username and quizId
    fetch(`http://localhost:5252/api/QuizResult/AllquizResults`, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
      }) .then(async (response) => {
        const data = await response.json();
        setQuizResults(data);
      })
      .catch((error) => console.error("Error fetching quiz results:", error));
  }, []); // Empty dependency array ensures that this effect runs only once when the component mounts
  
  return (
    <div className="inputcontainer">
      {quizResults && (
        <div>
          <h1>Quiz Report</h1>
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
        </div>
      )}
    </div>
  );
}

export default QuizReport;