import { useLocation,useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import "./Quiz.css";

function QuizList() {
  const [quizList, setQuizList] = useState([]);
  const navigate = useNavigate();
  const role = localStorage.getItem("role");
  const token=localStorage.getItem("token");
  const location = useLocation();
  const [quizResults, setQuizResults] = useState(null);

  useEffect(() => {
    if (role === "Creator") {
      getQuizs();
    } else {
      alert("You don't have access to this page");
      setTimeout(() => {
        navigate("/quizs");
      }, 0);
    }
  }, [role]);

  const getQuizs = () => {
    fetch("http://localhost:5252/api/Quiz", {
      method: "GET",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
    })
      .then(async (data) => {
        var myData = await data.json();
        console.log(myData);
        setQuizList(myData);
      })
      .catch((e) => {
        console.log(e);
      });
  };

  const handleDeleteQuiz = async (quizId) => {
    if (role !== "Creator") {
      alert("You don't have access to this page");
      navigate("/quizs");
    } else {
      const userConfirmed = window.confirm(
        `Do you really want to delete the quiz with ID ${quizId}?`
      );

      if (userConfirmed) {
        navigate("/deleteQuiz", { state: { quizId } });
      }
    }
  };

  const handleUpdateQuiz = (quiz) => {
    navigate("/updateQuiz", { state: quiz });
  };

  const handleAddQuiz = () => {
    navigate("/addQuiz");
  };
  const ShowQuizReport= (quizId)=>{
    navigate("/QuizIdReport", { state: { quizId } });
  }

  return (
    <div className="quiz">
      <h1 className="alert alert-quiz">Quizzes</h1>
      {role === "Creator" && (
        <button
          className="btn btn-primary"
          onClick={() => handleAddQuiz()}
        >
          AddQuiz
        </button>
      )}
      <hr className="line"/>
      {quizList.length > 0 ? (
        <div>
          {quizList.map((quiz) => (
            <div key={quiz.quizId} className="alert alert-quiz">
              Quiz Id: {quiz.quizId}
              <br />
              Quiz Title: {quiz.title}
              <br />
              Quiz Description: {quiz.description}
              <br />
              Quiz Category: {quiz.category}
              <br />
              Quiz TimeLimit: {quiz.timelimit}
              <br />
              {role === "Creator" && (
                <>
                  <button
                    className="btn btn-delete"
                    onClick={() => handleDeleteQuiz(quiz.quizId)}
                  >
                    Delete
                  </button>
                  <button
                    className="btn btn-update"
                    onClick={() => handleUpdateQuiz(quiz)}
                  >
                    Update
                  </button>
                  <button
                    className="btn btn-success"
                    onClick={() => ShowQuizReport(quiz.quizId)}
                  >
                    QuizReport
                  </button>
                </>
              )}
            </div>
          ))}
        </div>
      ) : (
        <div>No quizzes available yet</div>
      )}
    </div>
  );
}

export default QuizList;
