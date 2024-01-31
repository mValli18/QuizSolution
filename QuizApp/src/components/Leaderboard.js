import React, { useState, useEffect } from "react";
import {
  Button,
  TextField,
  Typography,
  Container,
  Paper,
  Grid,
  Card,
  CardContent,
  CardMedia,
} from "@mui/material";
import logo from "./profile.png";
import goldBadge from "./gold-badge.jpg";
import silverBadge from "./silver-badge.jpg";
import bronzeBadge from "./bronze-badge.jpg";

function Leaderboard() {
  const [quizId, setQuizId] = useState("");
  const [leaderboard, setLeaderboard] = useState(null);
  const [titleList, setTitleList] = useState([]);
  const [titleInput, setTitleInput] = useState("");
  const token = localStorage.getItem("token");

  const fetchLeaderboard = (quizId) => {
    if (quizId) {
      fetch(`http://localhost:5252/api/Quiz/leaderboard/${quizId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then(async (response) => {
          if (response.status === 404) {
            setLeaderboard(null);
            alert(`No leaderboard records available for ${quizId}`);
          } else {
            const data = await response.json();
            setLeaderboard(data);
          }
        })
        .catch((error) => console.error("Error fetching leaderboard:", error));
    } else {
      alert("Please provide a quizId");
    }
  };

  useEffect(() => {
    fetch("http://localhost:5252/api/Quiz/titles", {
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
    setTitleInput(e.target.value);
  };

  const searchResult = async () => {
    try {
      const response = await fetch(`http://localhost:5252/api/Quiz/quizId?title=${titleInput}`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
          Accept: "application/json",
          "Content-Type": "application/json",
        },
      });

      if (response.ok) {
        const selectedQuizId = await response.text();
        console.log("Selected QuizId:", selectedQuizId);
        console.log("Fetching leaderboard...");

        const parsedQuizId = parseInt(selectedQuizId, 10);

        if (!isNaN(parsedQuizId)) {
          fetchLeaderboard(parsedQuizId);
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

  const highlightStyle = (position) => {
    let style = {};
    let badge = null;

    switch (position) {
      case 1:
        style = {  fontWeight: "bold" };
        badge = goldBadge;
        break;
      case 2:
        style = {  fontWeight: "bold" };
        badge = silverBadge;
        break;
      case 3:
        style = { fontWeight: "bold" };
        badge = bronzeBadge;
        break;
      default:
        style = {};
        badge = null;
    }

    return { style, badge };
  };

  const renderTopThree = () => {
    return (
      <Grid container spacing={2}>
        {leaderboard.slice(0, 3).map((entry, index) => (
          <Grid item xs={4} key={index}>
            <Card>
              <CardContent>
                <CardMedia
                  component="img"
                  height="140"
                  image={highlightStyle(index + 1).badge}
                  alt={`Badge for Rank ${index + 1}`}
                />
                <center>
                <Typography variant="body1" style={{ textAlign: "center" }}>
                  Rank: {index + 1}
                </Typography>
                <Typography variant="h6"  style={highlightStyle(index + 1).style}>
                  {entry.username}
                </Typography>
                <Typography variant="h6">
                  Score: {entry.score}
                </Typography>
                </center>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    );
  };

  const renderOtherEntries = () => {
    return leaderboard.slice(3).map((entry, index) => (
      <Grid item xs={12} key={index + 3}>
        <Paper
          elevation={3}
          style={{
            padding: "10px",
            marginTop:"20px",
            display: "flex",
            justifyContent: "space-between",
            backgroundColor:"rgba(255,255,255,0.8)",
            color:"black",
          }}
        >
          <div style={{ flex: 1, minWidth: 80 ,fontWeight: "bold"}}>
            <Typography variant="body1">
             Rank: {index + 4}
            </Typography>
          </div>
          <div style={{ flex: 1, minWidth: 200 }}>
            <Typography variant="h6" color="primary">
              {entry.username}
            </Typography>
          </div>
          <div style={{ flex: 1, minWidth: 80 }}>
            <Typography variant="body1">
              Score: {entry.score}
            </Typography>
          </div>
          
        </Paper>
      </Grid>
    ));
  };

  return (
    <div className="inputcontainer">
      <h2 className="alert alert-quiz">Leaderboard</h2>
      <div className="d-flex align-items-center flex">
        <select
          className="form-select"
          value={titleInput}
          onChange={handleTitleChange}
        >
          <option value="">Select a title</option>
          {titleList.map((title) => (
            <option key={title} value={title}>
              {title}
            </option>
          ))}
        </select>
        <button
          className="btn btn-success"
          style={{ maxWidth: "45%", marginBottom: "15px" }}
          onClick={searchResult}
        >
          Search
        </button>
      </div>

      <hr />

      {leaderboard !== null && (
        <div>
          {leaderboard.length > 0 ? (
            <div>
              {renderTopThree()}
              {renderOtherEntries()}
            </div>
          ) : (
            <p>No leaderboard records for this quiz.</p>
          )}
        </div>
      )}
    </div>
  );
}

export default Leaderboard;