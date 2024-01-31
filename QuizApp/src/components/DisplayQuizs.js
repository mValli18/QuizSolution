import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { useDispatch } from "react-redux";
import { useSelector } from "react-redux";
import { addItem } from "../QuizSlice";
function DisplayQuizs(props) {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [selectedQuizId, setSelectedQuizId] = useState(null);
  const quizList = useSelector((state) => state.quizList);

  const addItem = () => {
    if (props.quiz) {
      var myQuiz = {
        quizId: props.quiz.quizId,
        title: props.quiz.title,
        description: props.quiz.description,
        category: props.quiz.category,
        timeLimit: props.quiz.timeLimit,
      };
  
      dispatch(addItem(myQuiz));
      console.log(quizList);
    }
  };
  const handleTakeQuiz = (quizId) => {
    setSelectedQuizId(quizId);
    navigate("/questions");
  };

  return (
    <div>
      Quiz Id: {props.quiz.quizId}
      <br />
      Quiz Title: {props.quiz.title}
      <br />
      Quiz Category: {props.quiz.category}
      <br />
      Quiz Description:{props.quiz.description}
      <br/>
      Quiz TimeLimit: {props.quiz.timeLimit}
      <br />
      <button
        className="btn btn-primary"
        onClick={() => handleTakeQuiz(props.quiz.quizId)}
      >
        Take Quiz
      </button>
    </div>
  );
}

export default DisplayQuizs;
