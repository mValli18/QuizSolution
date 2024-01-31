import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Questions.css";

function Questions() {
  const [questionList, setQuestionList] = useState([]);
  const navigate = useNavigate();
  const token=localStorage.getItem("token");
  const role=localStorage.getItem("role");
  const [quizId, setQuizId] = useState("");
  if (role !== "Creator") {
    alert("You don't have access to this page");
    setTimeout(() => {
      navigate("/quizs");
    }, 0);
    return null;
  }
  var getQuestions = () => {
    fetch("http://localhost:5252/api/Questions/getAll", {
      method: "GET",
      headers: {
        Accept: "application/json",
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    })
      .then(async (data) => {
        var myData = await data.json();
        await console.log(myData);
        await setQuestionList(myData);
      })
      .catch((e) => {
        if(e.response.request.statusText==="Forbidden"){
          alert('Oops this operation is not meant for all users');
          navigate("/quizs");
        }
        if (e.response && e.response.status === 403) {
          // Access forbidden, handle accordingly
          alert('You don\'t have access to this page.');
          // Redirect the user to the home page or login page
          navigate("/quizs");
        }
        console.log(e);
      });
  };

  const handleDelete = async (questionId) => {
    // Display a confirmation dialog
    const userConfirmed = window.confirm(
      `Do you really want to delete the question with ID ${questionId}?`
    );

    // If user confirms, proceed with deletion
    if (userConfirmed) {
      // Navigate to the DeleteQuestion component with questionId in the state
      navigate("/deleteQuestions", { state: { questionId } });
    }
  };

  const addQuestion = () => {
    navigate("/addQuestions");
  };

  const updateQuestion = (question) => {
    navigate("/updateQuestions", { state: question });
  };
  var checkQuestions = questionList.length > 0 ? true : false;

  return (
    <div className="question">
      <h1 className="alert alert-question">Questions</h1>
      <button className="btn btn-success" onClick={getQuestions}>
        Get All Questions
      </button>
      <button className="btn btn-primary" onClick={addQuestion}>
        Add Question
      </button>
      <hr/>
      {checkQuestions ? (
        <div>
          {questionList.map((question) => (
            <div key={question.questionId} className="alert alert-question">
              Question ID: {question.questionId}{" "}
              <button
                className="btn btn-delete"
                onClick={() => handleDelete(question.questionId)}
              >
                Delete
              </button>
              <button className="btn btn-update" onClick={() => updateQuestion(question)}>
                Update
              </button>
              <br />
              Question: {question.questionTxt}
              <br />
              Option A: {question.option1}
              <br />
              Option B: {question.option2}
              <br />
              Option C: {question.option3}
              <br />
              Option D: {question.option4}
              <br/>
              Answer:{question.answer}
              <br />
              Quiz ID: {question.quizId}
            </div>
          ))}
        </div>
      ) : (
        <div>No questions available yet</div>
      )}
    </div>
  );
}

export default Questions;