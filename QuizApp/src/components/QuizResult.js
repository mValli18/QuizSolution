// Import necessary React and Material-UI components and dependencies
import React, { useState, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { Grid, Card, CardContent, Typography, Paper, Table, TableContainer, TableHead, TableBody, TableRow, TableCell } from "@mui/material";
import { Doughnut } from "react-chartjs-2";
import Confetti from "react-confetti";
import "./QuizResults.css"; // Import your styles
import 'chart.js/auto';
import InputBase from '@mui/material/InputBase';



// Define the CelebrationEffect component
const CelebrationEffect = () => (
  <div className="celebration-effect">
    <Confetti />
  </div>
);

// Define the QuizResults functional component
function QuizResults() {
  const location = useLocation();
  const [quizResults, setQuizResults] = useState(null);
  const [quizTitle, setQuizTitle] = useState(null);
  const [chartData, setChartData] = useState(null);
  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  // useEffect hook to fetch quiz results and title
  useEffect(() => {
    const state = location.state;

    // Check if location.state is not null before destructuring
    if (state) {
      const { username, quizId } = state;

      fetch(`http://localhost:5252/api/QuizResult/results-with-total-score/${username}/${quizId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then(async (response) => {
          if (response.ok) {
            const data = await response.json();
            setQuizResults(data);

            // Fetch all quizzes
            fetch("http://localhost:5252/api/Quiz", {
              method: "GET",
              headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
              },
            })
              .then(async (data) => {
                const myData = await data.json();

                // Filter quizzes based on quizId
                const matchingQuiz = myData.find((quiz) => quiz.quizId === quizId);

                if (matchingQuiz) {
                  setQuizTitle(matchingQuiz.title);
                } else {
                  console.error("Quiz not found with id:", quizId);
                }
              })
              .catch((titleError) => console.error("Error fetching quiz title:", titleError));
          } else {
            console.error("Error fetching quiz results:", response.statusText);
          }
        })
        .catch((error) => console.error("Error fetching quiz results:", error));
    }
  }, [location.state, token]);

  // useEffect hook to update chartData based on quizResults
  useEffect(() => {
    if (quizResults) {
      const correctCount = quizResults.quizResults.filter((result) => result.isCorrect).length;
      const incorrectCount = quizResults.quizResults.length - correctCount;

      setChartData({
        labels: ["Correct", "Incorrect"],
        datasets: [
          {
            data: [correctCount, incorrectCount],
            backgroundColor: ["#4CAF50", "#FF5252"],
          },
        ],
      });
    }
  }, [quizResults]);

  const GoToQuizs = () => {
    navigate("/navbar");
  };

  return (
    <div className="inputcontainer">
     
      {quizResults && (
        <Paper elevation={3} sx={{ padding: 3, marginTop: 2 }}>
          {/* Use Grid to display quiz name, time taken, score, and pie chart in a row */}
          <Grid container spacing={2}>
            {/* Quiz Info */}
            <Grid item xs={12} md={6}>
              <Card sx={{ height: "100%" }}>
                <CardContent>
                  <Typography variant="h4" component="div" color="primary" sx={{ marginBottom: 2 }}>
                     {quizTitle}
                  </Typography>
                  <Typography variant="h6" color="secondary" sx={{ marginBottom: 2 }}>
                    Total Score: {quizResults.totalScore}
                  </Typography>
                  
                  {/* Conditional rendering for celebration effect */}
                  {quizResults.totalScore > quizResults.quizResults.length / 2 && <CelebrationEffect />}
                </CardContent>
              </Card>
            </Grid>

            {/* Pie Chart */}
            <Grid item xs={12} md={6}>
              <Card sx={{ height: "100%" }}>
                <CardContent>
                  <Typography variant="h6" color="primary">
                    Pie Chart - Correct vs Incorrect
                  </Typography>
                  {/* Render Doughnut chart if chartData is available */}
                  {chartData && (
                    <Doughnut
                      data={chartData}
                      options={{
                        plugins: {
                          legend: {
                            display: true,
                          },
                        },
                      }}
                    />
                  )}
                </CardContent>
              </Card>
            </Grid>
          </Grid>

          {/* Table with user answers, correctness, and scores */}
          <TableContainer component={Paper} sx={{ marginTop: 2 }}>
            <Table>
              <TableHead sx={{ background: "#2196f3", color: "white" }}>
                <TableRow>
                  <TableCell>User Answer</TableCell>
                  <TableCell>Correct Answer</TableCell>
                  <TableCell>Score</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {quizResults.quizResults.map((result, index) => (
                  <TableRow key={index}>
                    <TableCell>{result.userAnswer}</TableCell>
                    <TableCell>{result.isCorrect ? "Correct" : "Incorrect"}</TableCell>
                    <TableCell>{result.score}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>
      )}
    </div>
  );
}

export default QuizResults;
