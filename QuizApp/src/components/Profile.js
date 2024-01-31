import { useState, useEffect } from "react";
import axios from "axios";

function Profile() {
  const [totalScore, setTotalScore] = useState(null);
  const [error, setError] = useState(null);
  const [userResults, setUserResults] = useState([]);
  const username = localStorage.getItem("username");
  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role");
  const [titleList, setTitleList] = useState([]);
  const [titleInput, setTitleInput] = useState("");

  const handleSearch = async (quizId) => {
    try {
      console.log("Searching for total score...");

      // Ensure both quizId and username are available
      if (!quizId || !username) {
        console.error("Invalid quizId or username");
        setError("Invalid quizId or username");
        return;
      }

      const response = await axios.get(`http://localhost:5252/api/QuizResult/totalscore/${quizId}/${username}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      console.log("Full response:", response);

      // Access data directly from the response
      const totalScoreString = response.data;
      const totalScore = parseInt(totalScoreString.match(/\d+/)[0], 10);

      // Check if the totalScore is a valid number
      if (!isNaN(totalScore)) {
        console.log("Parsed Total Score:", totalScore);
        setTotalScore(totalScore);
        setError(null);
      } else {
        console.error("Invalid totalScore:", totalScoreString);
        setError("Failed to fetch total score. Unexpected response structure.");
      }
    } catch (error) {
      console.error("Error fetching total score:", error);

      setTotalScore(null);
      setError("Failed to fetch total score. Please check your input and try again.");
    }
  };

  const fetchUserResults = async () => {
    try {
      console.log("Username",username);
      // Fetch answered quiz IDs for the user
      const quizIdsResponse = await axios.get(`http://localhost:5252/api/QuizResult/answered-quiz-ids/${username}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
  
      const answeredQuizIds = quizIdsResponse.data;

      console.log("Answered Quiz IDs:", answeredQuizIds);
      
      // Fetch total quiz results based on the answered quiz IDs
      const totalResults = [];
      
      for (const quizId of answeredQuizIds) {
        try {
          const totalResultResponse = await axios.get(`http://localhost:5252/api/QuizResult/totalscore/${quizId}/${username}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
      
          const totalScoreString = totalResultResponse.data;
          const totalScore = parseInt(totalScoreString.match(/\d+/)[0], 10);
      
          // Check if the totalScore is a valid number
          if (!isNaN(totalScore)) {
            totalResults.push({
              quizId,
              totalScore,
            });
          } else {
            console.error("Invalid totalScore:", totalScoreString);
            // Handle invalid totalScore if needed
          }
        } catch (error) {
          console.error("Error fetching total score for Quiz ${quizId}:", error);
          // Handle error if needed
        }
      }
      
  
      // Set user results in state
      setUserResults(totalResults);
    } catch (error) {
      console.error("Error fetching user results:", error);
    }
  };
  useEffect(() => {
    // Fetch the list of categories when the component mounts
    fetch(`http://localhost:5252/api/Quiz/titles`, {
      method: "GET",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
    })
      .then(async (data) => {
        const titles = await data.json();
        setTitleList(titles);
      })
      .catch((e) => {
        console.log(e);
      });
  }, []);

  const handleTitleChange = (e) => {
    // Update the categoryInput state when the dropdown selection changes
    setTitleInput(e.target.value);
  };

  const searchResult = async () => {
    try {
      // Fetch the quizId based on the selected title
      const response = await fetch(`http://localhost:5252/api/Quiz/quizId?title=${titleInput}`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
          Accept: "application/json",
          "Content-Type": "application/json",
        },
      });

      console.log("Response:", response);

      if (response.ok) {
        const selectedQuizId = await response.text();

        // Log the selectedQuizId
        console.log("Selected QuizId:", selectedQuizId);

        // Ensure that the selectedQuizId is an integer
        const parsedQuizId = parseInt(selectedQuizId, 10);

        if (!isNaN(parsedQuizId)) {
          // Pass the parsedQuizId to fetchLeaderboard
          handleSearch(parsedQuizId);
        } else {
          console.log("Invalid quizId format");
          alert("Invalid quizId format");
        }
      } else {
        console.log("Failed to fetch quizId for the selected title");
        alert("Failed to fetch quizId for the selected title");
      }
    } catch (error) {
      console.error("Error fetching quizId:", error);
    }
  };

  useEffect(() => {
    console.log("Profile component mounted");
    // Fetch user results when the component mounts
    fetchUserResults();
  }, []);

  return (
    <div className="quiz">
      <div className="alert alert-quiz">
        <h1>Profile</h1>
        <p>Username: {username}</p>
        <p>Role: {role}</p>
        <div className="d-flex align-items-center flex">
          <select className="form-select" value={titleInput} onChange={handleTitleChange}>
            <option value="">Select a title</option>
            {titleList.map((title) => (
              <option key={title} value={title}>
                {title}
              </option>
            ))}
          </select>
          <button className="btn btn-success" style={{ maxWidth: "45%", marginBottom: "15px" }} onClick={searchResult}>
            Search
          </button>
        </div>
        <br />
        <p>Total Score is: {totalScore !== null ? totalScore : "Not available"}</p>
        {error && <p style={{ color: "red" }}>{error}</p>}
      </div>
      {/* Table to display user results */}
      <table className="table">
        <thead>
          <tr>
            <th>QuizId</th>
            <th>Total Score</th>
          </tr>
        </thead>
        <tbody>
          {userResults.map((result) => (
            <tr key={result.quizId}>
              <td>{result.quizId}</td>
              <td>{result.totalScore}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Profile;
