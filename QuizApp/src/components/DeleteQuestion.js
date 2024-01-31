import axios from "axios";
import { useEffect } from "react";
import { useLocation,useNavigate } from "react-router-dom";

function DeleteQuestion() {
  const location = useLocation();
  const questionId = location.state && location.state.questionId;
  const token=localStorage.getItem("token");
  const navigate=useNavigate();
  useEffect(() => {
    const clickDelete = async () => {
      if (!questionId) {
        alert("QuestionId is required for deleting.");
        return;
      }

      try {
        await axios.delete(`http://localhost:5252/api/Questions/Remove?questionid=${questionId}`,{
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }).then(
          ()=>{
              alert("Question Deleted Successfully");
              navigate("/questions");
          }
      )} catch (error) {
        console.log(error);
        alert("Error deleting question");
      }
    };

    // Execute deletion logic upon mounting
    clickDelete();
  }, [questionId]); // Only run the effect when questionId changes

  return (
    <div className="inputcontainer">
      <h1 className="alert alert-quiz">DeleteQuestion</h1>
      <label className="form-control" htmlFor="questionId">
        Question ID: {questionId}
      </label>
    </div>
  );
}

export default DeleteQuestion;
