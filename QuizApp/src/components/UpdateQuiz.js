import axios from "axios";
import { useState } from "react";
import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
function UpdateQuiz() {
  const location = useLocation();
  const [quiz, setQuiz] = useState(location.state || {});
  const { quizId, title, description, category, timelimit } = quiz;
  const navigate=useNavigate();
  const clickUpdate = () => {
    if (!quizId) {
      alert('Quiz ID is required for updating.');
      return;
    }
    const token=localStorage.getItem("token");
    const updatedQuiz = {
      quizId,
      title,
      description,
      category,
      timelimit,
    };

    axios.put("http://localhost:5252/api/Quiz/update", updatedQuiz,{
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    }).then(() => {
        alert('Quiz Updated');
        navigate("/quizList");
      })
      .catch((e) => {
        if(e.response.data.title==="One or more validation errors occurred."){
          alert('Please check the data and timeLimit should be an integer');
        }
        console.log(e);
      });
  };

  return (
    <div className="inputcontainer">
      <h1 className="alert alert-quiz">UpdateQuiz</h1>
      <div class="form-floating mb-3">
      <input id="floatingInput" type="number" className="form-control" placeholder="QuizId" value={quizId} readOnly />
      <label htmlFor="floatingInput">Quiz ID</label>
      </div>
      <div class="form-floating mb-3">
      <input type="text" className="form-control" id="floatingInput" placeholder="Quiz Title" value={title} onChange={(e) => setQuiz({ ...quiz, title: e.target.value })} />
      <label htmlFor="floatingInput">Quiz Title</label>
      </div>
      <div class="form-floating mb-3">
      <input type="text" className="form-control" id="floatingInput" placeholder="Quiz Description" value={description} onChange={(e) => setQuiz({ ...quiz, description: e.target.value })} />
      <label htmlFor="floatingInput">Quiz Description</label>
      </div>
      <div class="form-floating mb-3">
      <input id="floatingInput" type="text" className="form-control" placeholder="Quiz Cateory" value={category} readOnly/>
      <label htmlFor="floatingInput">Quiz Category</label>
      </div>
      <div class="form-floating mb-3">
      <input id="floatingInput" type="number" className="form-control" placeholder="Please provide integer value in minutes." value={timelimit} onChange={(e) => setQuiz({ ...quiz, timelimit: e.target.value })} />
      <label htmlFor="floatingInput">Integer TimeLimit in Minutes</label>
      </div>
      <button onClick={clickUpdate} className="btn btn-primary">Update Quiz</button>
    </div>
  );
}

export default UpdateQuiz;
