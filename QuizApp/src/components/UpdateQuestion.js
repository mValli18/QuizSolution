import axios from "axios";
import { useState} from "react";
import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";

function UpdateQuestion() {
  const location = useLocation();
  const [question, setQuestion] = useState(location.state || {});
  const navigate = useNavigate();
  const { questionId, quizId, questionTxt, option1, option2, option3, option4, answer } = question;


  const clickUpdate = () => {
    if (!quizId || !questionId) {
      alert('Quiz ID and Question ID are required for updating.');
      return;
    }
    const token = localStorage.getItem("token");
    const updatedQuestion = {
      questionId,
      questionTxt,
      option1,
      option2,
      option3,
      option4,
      answer,
      quizId,
    };

    axios.put(`http://localhost:5252/api/Questions/update`, updatedQuestion,{
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      })
      .then(() => {
        alert('Question Updated Successfully');
        navigate("/questions");
      })
      .catch((e) => {
        console.log(e);
      });
  };

  return (
    <div className="inputcontainer">
      <h1 className="alert alert-quiz">UpdateQuestion</h1>
      <div className="form-floating mb-3">
      <input id="floatingInput" type="number" className="form-control" value={questionId} readOnly />
      <label  htmlFor="floatingInput">Question ID</label>
      </div>
      <div className="form-floating mb-3">
        <input id="floatingInput" type="number" className="form-control" value={quizId} readOnly />
        <label htmlFor="floatingInput">Quiz ID</label>
      </div>
      <div className="form-floating mb-3">
        <input id="floatingInput" type="text" className="form-control" value={questionTxt} onChange={(e) => setQuestion({ ...question, questionTxt: e.target.value })} />
        <label htmlFor="floatingInput">Question</label>
      </div>
      <div className="form-floating mb-3">
        <input id="floatingInput" type="text" className="form-control" value={option1} onChange={(e) => setQuestion({ ...question, option1: e.target.value })} />
        <label htmlFor="floatingInput">Option A</label>
      </div>
      <div className="form-floating mb-3">
        <input id="floatingInput" type="text" className="form-control" value={option2} onChange={(e) => setQuestion({ ...question, option2: e.target.value })} />
        <label htmlFor="floatingInput">Option B</label>
      </div>
      <div className="form-floating mb-3">
        <input id="floatingInput" type="text" className="form-control" value={option3} placeholder="Option C" onChange={(e) => setQuestion({ ...question, option3: e.target.value })} />
        <label htmlFor="floatingInput">Option C</label>      
      </div>
      <div className="form-floating mb-3">
        <input id="floatingInput" type="text" className="form-control" value={option4} placeholder="Option D" onChange={(e) => setQuestion({ ...question, option4: e.target.value })} />
        <label htmlFor="floatingInput">Option D</label>
      </div>
      <div className="form-floating mb-3">
        <input id="floatingInput" type="text" className="form-control" value={answer} onChange={(e) => setQuestion({ ...question, answer: e.target.value })} />
        <label htmlFor="floatingInput">Answer</label>
      </div>
      <button onClick={clickUpdate} className="btn btn-primary">Update Question</button>
    </div>
  );
}

export default UpdateQuestion;
