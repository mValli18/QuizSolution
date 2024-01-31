import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import "./Quiz.css";
function Quizs() {
  const [quizList, setQuizList] = useState([]);
  const navigate = useNavigate();
  const [categoryList, setCategoryList] = useState([]);
  const [categoryInput, setCategoryInput] = useState("");

  useEffect(() => {
    // Fetch the list of categories when the component mounts
    fetch("http://localhost:5252/api/Quiz/categories", {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    })
      .then(async (data) => {
        var categories = await data.json();
        setCategoryList(categories);
      })
      .catch((e) => {
        console.log(e);
      });
  }, []);

  useEffect(() => {
    getQuizs();
  }, []); 

  const getQuizs = () => {
    fetch('http://localhost:5252/api/Quiz', {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    })
      .then(async (data) => {
        var myData = await data.json();
        console.log(myData);
        setQuizList(myData);
      })
      .catch((e) => {
        console.log(e);
      });
  }

  const handleTakeQuiz = async (quizId,timelimit) => {
    // Pass the quizId as state to the QuestionsByQuizId component
    navigate("/questionsbyid", { state: { quizId,timelimit } });
  };
  const handleCategoryChange = (e) => {
    // Update the categoryInput state when the dropdown selection changes
    setCategoryInput(e.target.value);
  };
  const searchQuiz = () => {
    // Navigate to "/search" with category value as state
    navigate("/search", { state: { category: categoryInput } });
  };


  return (
    <div className="quiz">
      <h1 className="alert alert-quiz">Quizzes</h1>
      <div className="d-flex align-items-center flex">
        <select className="form-select" value={categoryInput} onChange={handleCategoryChange}>
          <option value="">Select a category</option>
          {categoryList.map((category) => (
            <option key={category} value={category}>
              {category}
            </option>
          ))}
        </select>
        <button className="btn btn-success" style={{ maxWidth: "45%", marginBottom: "15px" }} onClick={searchQuiz}>
          Search
        </button>
      </div>
      <hr />
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
              <button
                className="btn btn-takequiz"
                onClick={() => handleTakeQuiz(quiz.quizId,quiz.timelimit)}
              >
                Take Quiz
              </button>
            </div>
          ))}
        </div>
      ) : (
        <div>No quizzes available yet</div>
      )}
    </div>
  );
}
export default Quizs;
