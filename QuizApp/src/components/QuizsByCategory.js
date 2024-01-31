// import { useNavigate } from "react-router-dom";
// import { useState, useEffect } from "react";

// function QuizsByCategory() {
//   const [quizList, setQuizList] = useState([]);
//   const [categoryList, setCategoryList] = useState([]);
//   const [categoryInput, setCategoryInput] = useState("");
//   const navigate = useNavigate();

//   useEffect(() => {
//     // Fetch the list of categories when the component mounts
//     fetch("http://localhost:5057/api/Quiz/categories", {
//       method: 'GET',
//       headers: {
//         'Accept': 'application/json',
//         'Content-Type': 'application/json'
//       }
//     })
//       .then(async (data) => {
//         var categories = await data.json();
//         setCategoryList(categories);
//       })
//       .catch((e) => {
//         console.log(e);
//       });
//   }, []);

//   const getQuizs = () => {
//     // Use the categoryInput in the fetch URL
//     fetch(`http://localhost:5057/api/Quiz/category/${categoryInput}`, {
//       method: 'GET',
//       headers: {
//         'Accept': 'application/json',
//         'Content-Type': 'application/json'
//       }
//     })
//       .then(async (data) => {
//         var myData = await data.json();
//         console.log(myData);
//         setQuizList(myData);
//       })
//       .catch((e) => {
//         console.log(e);
//       });
//   };

//   const handleCategoryChange = (e) => {
//     // Update the categoryInput state when the dropdown selection changes
//     setCategoryInput(e.target.value);
//   };

//   const handleTakeQuiz = async (quizId) => {
//     // Pass the quizId as state to the QuestionsByQuizId component
//     navigate("/questionsbyid", { state: { quizId } });
//   };
//   const checkQuizs = quizList.length > 0 ? true : false;
//   return (
//     <div className="inputcontainer">
//       <h1 className="alert alert-question">Search Quizs</h1>
//       {/* Dropdown menu for the category */}

//       <div className="d-flex align-items-center flex">
//         <select className="form-select" value={categoryInput} onChange={handleCategoryChange}>
//           <option value="">Select a category</option>
//           {categoryList.map((category) => (
//             <option key={category} value={category}>
//               {category}
//             </option>
//           ))}
//         </select>
//         <button className="btn btn-success" style={{ maxWidth: "45%", marginBottom: "15px" }} onClick={getQuizs}>
//           Search
//         </button>
//       </div>

//       <hr />
//       {checkQuizs ? (
//         <div>
//           {quizList.map((quiz) => (
//             <div key={quiz.quizId} className="alert alert-question">
//               Quiz Id:{quiz.quizId}
//               <br/>
//               Quiz Title: {quiz.title}
//               <br />
//               Quiz Description: {quiz.description}
//               <br />
//               Quiz Category: {quiz.category}
//               <br />
//               Quiz TimeLimit: {quiz.timeLimit}
//               <br/>                   
//                     <button
//                         className="btn btn-takequiz"
//                         onClick={() => handleTakeQuiz(quiz.quizId)}> 
//                         Take Quiz
//                     </button>
//             </div>
//           ))}
//         </div>
//       ) : (
//         <div>No quizs available for the provided category</div>
//       )}
//     </div>
//   );
// }

// export default QuizsByCategory;

import { useNavigate, useLocation } from "react-router-dom";
import { useState, useEffect } from "react";

function QuizsByCategory() {
  const [quizList, setQuizList] = useState([]);
  const navigate = useNavigate();
  const location = useLocation();
  const [categoryInput, setCategoryInput] = useState(location.state?.category || "");
  const [categoryList, setCategoryList] = useState([]);

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
    // Fetch quizzes when the category input changes
    if (categoryInput) {
      getQuizs();
    }
  }, [categoryInput]);

  const getQuizs = () => {
    // Use the categoryInput in the fetch URL
    fetch(`http://localhost:5252/api/Quiz/category/${categoryInput}`, {
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
  };

  const handleTakeQuiz = async (quizId,timeLimit) => {
    // Pass the quizId as state to the QuestionsByQuizId component
    navigate("/questionsbyid", { state: { quizId,timeLimit } });
  };
  const handleCategoryChange = (e) => {
    // Update the categoryInput state when the dropdown selection changes
    setCategoryInput(e.target.value);
  };
  const checkQuizs = quizList.length > 0 ? true : false;

  return (
    <div className="inputcontainer">
      <h1 className="alert alert-question">Quizzes</h1>

      {/* Display the selected category */}
      <p>Selected Category: {categoryInput}</p>

      <hr />
      {checkQuizs ? (
        <div>
          <div className="d-flex align-items-center flex">
        <select className="form-select" value={categoryInput} onChange={handleCategoryChange}>
          <option value="">Select a category</option>
          {categoryList.map((category) => (
            <option key={category} value={category}>
              {category}
            </option>
          ))}
        </select>
      </div>
      <hr />
          {quizList.map((quiz) => (
            <div key={quiz.quizId} className="alert alert-question">
              Quiz Id: {quiz.quizId}
              <br/>
              Quiz Title: {quiz.title}
              <br />
              Quiz Description: {quiz.description}
              <br />
              Quiz Category: {quiz.category}
              <br />
              Quiz TimeLimit: {quiz.timeLimit}
              <br/>                   
              <button
                className="btn btn-takequiz"
                onClick={() => handleTakeQuiz(quiz.quizId,quiz.timeLimit)}
              > 
                Take Quiz
              </button>
            </div>
          ))}
        </div>
      ) : (
        <div>No quizzes available for the selected category</div>
      )}
    </div>
  );
}

export default QuizsByCategory;
