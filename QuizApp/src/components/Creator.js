import { useNavigate } from "react-router-dom";
import './Creator.css';

function Creator() {
  const role = localStorage.getItem("role");
  const navigate = useNavigate();

  if (role !== "Creator") {
    alert("You don't have access to this page");
    setTimeout(() => {
      navigate("/quizs");
    }, 0);
    return null;
  }

  const handleQuestions = () => {
    navigate("/questions");
  };

  const handleQuizs = () => {
    navigate("/quizList");
  };

  return (
    <div className="input-container-controller">
      <h2 className="alert alert-success">Creator Operations</h2>
      <h5>Navigate to the Questions page to add, update, and delete them.</h5>
      <button className="btn btn-question" onClick={handleQuestions}>
        Manage Questions
      </button>
      <h5>Navigate to the Quizs page to add, update, and delete them.</h5>
      <button className="btn btn-quiz" onClick={handleQuizs}>
        Manage Quizs
      </button>
    </div>
  );
}

export default Creator;
